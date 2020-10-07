using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RicardoLourencoWebStore.Data;
using RicardoLourencoWebStore.Data.Entities;

[assembly: HostingStartup(typeof(RicardoLourencoWebStore.Areas.Identity.IdentityHostingStartup))]
namespace RicardoLourencoWebStore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}