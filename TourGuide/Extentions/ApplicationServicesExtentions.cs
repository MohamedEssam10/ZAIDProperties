using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Services;
using ApplicationLayer.Services.Auth;
using InfrastructureLayer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace PresentationLayer.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(IServiceCollection Services, IConfiguration Configuration)
        {

            Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Add services to the container.

            Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();

            #region Swagger
            Services.AddSwaggerGen(options =>
            {
                // Add Swagger doc
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Tour Guide API",
                    Version = "v1",
                    Description = "API for Trips",
                    Contact = new OpenApiContact
                    {
                        Name = "ToureGuide Team",
                        Email = "support@TourGuideproject.com"
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "Enter only your JWT token (e.g., abc123). 'Bearer' prefix is added automatically."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                     {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    },
                    In = ParameterLocation.Header,
                    Name = "Authorization"
                },
                     Array.Empty<string>()
                }
                });

                options.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
            });
            #endregion

            // Add CORS policy
            Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            Services.AddDbContext<PropertyDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            Services.AddScoped<IMessageServices, MessageServices>();
            Services.AddScoped<ITokenService, TokenService>();

            return Services;
        }
    }
}
