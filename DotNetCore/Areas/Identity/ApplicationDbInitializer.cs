using DotNetCore.Areas.Identity.Data;
using DotNetCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Areas.Identity
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedUsers(DotNetCoreContext context,UserManager<DotNetCoreUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            //creates the default user and roles
            context.Database.EnsureCreated();

            string roleAdmin = "Admin";
            string roleUser = "User";
            if(await roleManager.FindByNameAsync(roleAdmin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleAdmin));
            }

            if(await roleManager.FindByNameAsync(roleUser) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleUser));
            }
            if (userManager.FindByEmailAsync("biraj@sensus.dk").Result == null)
            {
                DotNetCoreUser user = new DotNetCoreUser
                {
                    UserName = "biraj@sensus.dk",
                    Email = "biraj@sensus.dk",
                    Enabled = true,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddYears(1000)
                };
                IdentityResult result = userManager.CreateAsync(user, "Fi$hb0ne").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, roleAdmin).Wait();
                }
            }
        }
    }
}
