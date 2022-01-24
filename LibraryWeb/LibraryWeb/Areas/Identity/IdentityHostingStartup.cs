using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(LibraryWeb.Areas.Identity.IdentityHostingStartup))]
namespace LibraryWeb.Areas.Identity
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