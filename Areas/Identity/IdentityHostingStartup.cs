using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(HR.Areas.Identity.IdentityHostingStartup))]
namespace HR.Areas.Identity
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