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


        builder.Services.AddRateLimiter(options =>
        {
            options.AddPolicy("ApiPolicy", httpContext =>
            {
                // 1) Extract the client’s IP address (or use "unknown" if null)
                var clientIp = httpContext.Connection.RemoteIpAddress?.ToString()
                               ?? "unknown";

                // 2) Use the Token-Bucket factory, keyed by that IP
                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: clientIp,
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 20,                             // burst size
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0,                              // no queuing
                        ReplenishmentPeriod = TimeSpan.FromMinutes(1),        // refill interval
                        TokensPerPeriod = 20,                             // tokens added per interval
                        AutoReplenishment = true
                    });
            });
        });





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
        app.MapGet("/api/data", () => "hello")
           .RequireRateLimiting("ApiPolicy");

        app.MapControllers();
        app.MapHub<NotificationHub>("/hubs/notifications");

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}