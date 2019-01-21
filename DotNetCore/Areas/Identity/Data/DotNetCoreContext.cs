using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Areas.Identity;
using DotNetCore.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Models
{
    public class DotNetCoreContext : IdentityDbContext<DotNetCoreUser, IdentityRole,string>
    {
        public DotNetCoreContext(DbContextOptions<DotNetCoreContext> options)
            : base(options)
        {
        }
        //private readonly UserManager<DotNetCoreUser> _userManager;
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //const string ADMIN_ID = "446a60df-de66-4208-aa52-2c1075ea0bd9";
            //var hasher = new PasswordHasher<DotNetCoreUser>();
            //DotNetCoreUser user=new DotNetCoreUser{
            //    Id=ADMIN_ID,
            //    UserName = "admin",
            //    NormalizedUserName = "admin",
            //    Email = "biraj@sensus.dk",
            //    NormalizedEmail = "biraj@sensus.dk",
            //    EmailConfirmed = true,
            //    PasswordHash = hasher.HashPassword(null, "Fi$hb0ne"),
            //    Enabled = true,
            //    FromDate = DateTime.Now,
            //    ToDate = DateTime.Now.AddYears(1000)
            //};
            //builder.Entity<DotNetCoreUser>().HasData(new DotNetCoreUser
            //{
            //    Id = ADMIN_ID,
            //    UserName = "admin",
            //    NormalizedUserName = "admin",
            //    Email = "biraj@sensus.dk",
            //    NormalizedEmail = "biraj@sensus.dk",
            //    EmailConfirmed = true,
            //    PasswordHash = hasher.HashPassword(null, "Fi$hb0ne"),
            //    Enabled =true,
            //    FromDate=DateTime.Now,
            //    ToDate=DateTime.Now.AddYears(1000)
            //});

            //const string Role_ID_Admin = "fdc71aee-bc55-41f7-9b11-c32a314cd9b0";
            //const string Role_ID_Employee = "78f049ed-f427-46ab-ab01-502591f27576";
            //builder.Entity<IdentityRole>().HasData(new IdentityRole
            //{
            //    Id= Role_ID_Admin,
            //    Name="Administrator",
            //    ConcurrencyStamp=string.Empty,
            //    NormalizedName=null
            //});
            //builder.Entity<IdentityRole>().HasData(new IdentityRole
            //{
            //    Name = "Admin", NormalizedName = "Admin".ToUpper()
            //});
            //builder.Entity<IdentityRole>().HasData(new IdentityRole
            //{
            //    Id = Role_ID_Employee,
            //    Name = "User",
            //    ConcurrencyStamp = string.Empty,
            //    NormalizedName = null
            //});
            //builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            //{
            //    UserId=ADMIN_ID,
            //    RoleId=Role_ID_Admin
            //});
            //ApplicationDbInitializer.SeedUsers(_userManager);
        }
    }
}
