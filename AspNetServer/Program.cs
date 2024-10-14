#if UNUSED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Logging;

namespace AspNetServer
{
	class Program
	{
		static void Main(string[] args)
		{
			var host = new WebHostBuilder()
				.UseStartup<Startup>()
				.UseSetting("server.urls", "https://*:5001")
				.UseKestrel(options =>
				{
					options.UseHttps(new HttpsConnectionFilterOptions()
					{
						ServerCertificate = new X509Certificate2("server.pfx", "test"),
						SslProtocols = SslProtocols.Tls12
					});
				})
				.Build();


			using (var cts = new CancellationTokenSource())
			{
				Console.CancelKeyPress += (sender, eventArgs) =>
				{
					cts.Cancel();
					eventArgs.Cancel = true;
				};

				host.Run(cts.Token);
			}
		}
	}
}




using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("https://*:5001");
                    webBuilder.UseKestrel(options =>
                    {
                        options.ConfigureHttpsDefaults(httpsOptions =>
                        {
                            httpsOptions.ServerCertificate = new X509Certificate2("server.pfx", "test");
                            httpsOptions.SslProtocols = SslProtocols.Tls12;
                        });
                    });
                })
                .Build();

            using (var cts = new CancellationTokenSource())
            {
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    cts.Cancel();
                    eventArgs.Cancel = true;
                };

                await host.RunAsync(cts.Token);
            }
        }
    }
}
#endif



using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();  // This is for Web API controllers

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapControllers();  // This maps the routes for your API endpoints

app.Run();