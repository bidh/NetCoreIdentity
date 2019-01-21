using DotNetCore.Areas.Identity.Data;
using DotNetCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(DotNetCore.Areas.Identity.IdentityHostingStartup))]
namespace DotNetCore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<DotNetCoreContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DotNetCoreContextConnection")));

                services.AddIdentity<DotNetCoreUser, IdentityRole>(
                        options => options.Stores.MaxLengthForKeys = 128)
                                .AddEntityFrameworkStores<DotNetCoreContext>()
                                .AddDefaultUI()
                                .AddDefaultTokenProviders();
            });
        }
    }
}