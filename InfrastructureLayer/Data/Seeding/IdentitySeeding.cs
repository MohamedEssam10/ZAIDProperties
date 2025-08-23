using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Data.Seeding
{
    public static class IdentitySeeding
    {
        public static async Task IdentitySeedingOperation(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!await userManager.Users.AnyAsync())
            {
                ApplicationUser Admin = new ApplicationUser()
                {
                    Email = "Zaid@gmail.com",
                    UserName = "Zaid",
                    PhoneNumber = "+201019383550",
                };

                var result1 = await userManager.CreateAsync(Admin, "12345678");
            }
        }
    }
}
