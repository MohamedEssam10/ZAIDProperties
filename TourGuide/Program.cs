using ApplicationLayer.Helper;
using ApplicationLayer.Hubs;
using InfrastructureLayer.Data.Context;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Extentions;
using System.Threading.RateLimiting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ApplicationServicesExtentions.AddApplicationServices(builder.Services, builder.Configuration);
        IdentityServicesExtention.AddIdentityServices(builder.Services, builder.Configuration);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSignalR();


        // 1) In Program.cs, configure DI
        builder.Services.AddRateLimiter(options =>
        {
            // Partition by IP for *all* requests
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: clientIp,
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 20,                     // max burst
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0,                      // no queueing
                        ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                        TokensPerPeriod = 20,                // refill
                        AutoReplenishment = true
                    });
            });

            // Optional: override the default 429 status if you like
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });



        builder.Services.AddReverseProxy()
      .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

        var app = builder.Build();
       
        app.UseRouting();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
        {
            app.UseDeveloperExceptionPage();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZAID PROPERTY API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var service = app.Services.CreateScope())
        {
            //await IdentitySeeding.IdentitySeedingOperation(service.ServiceProvider);
            URLResolver.Init(service.ServiceProvider.GetRequiredService<IHttpContextAccessor>());

        }

        app.UseCors("AllowAllOrigins");

        app.UseRateLimiter();

        var api = app.MapGroup("/api")
             .RequireRateLimiting("ApiPolicy");

        app.MapControllers();
        app.MapHub<NotificationHub>("/hubs/notifications");

        app.UseStaticFiles();
        app.MapReverseProxy();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}