﻿
using InfrastructureLayer.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Extentions
{
    public static class IdentityServicesExtention
    {
        public static IServiceCollection AddIdentityServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddEntityFrameworkStores<TourGuideDbContext>();

            return services;
        }
    }
}
