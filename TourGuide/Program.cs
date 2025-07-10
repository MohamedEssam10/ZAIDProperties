using InfrastructureLayer.Data.Context;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Extentions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ApplicationServicesExtentions.AddApplicationServices(builder.Services, builder.Configuration);
        IdentityServicesExtention.AddIdentityServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}