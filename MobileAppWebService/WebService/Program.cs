using System;
using System.Diagnostics;
using System.IO;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SlotLogic.MobileAppWebService
{
    public class Program
    {

        public static IWebHostBuilder BuildWebHost(String[] args)
        {
            IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder(args);
            webHostBuilder.ConfigureAppConfiguration((hostingContext, configuration) => { configuration.SetBasePath("E:\\Java"/*"/etc/mobileapp/"*/); configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); configuration.AddJsonFile("errorMessage.json", optional: true, reloadOnChange: true); });
            webHostBuilder.ConfigureLogging((hostingContext, logging) => { logging.ClearProviders(); logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging")); logging.AddLog4Net("E:\\Java\\log4net.config"/*"/etc/mobileapp/log4net.config"*/); });
            webHostBuilder.UseUrls("http://10.9.8.139:5050");
            webHostBuilder.UseStartup<Startup>();

            return webHostBuilder;
        }

        public static void Main(String[] args)
        {
            String pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            String pathToContentRoot = Path.GetDirectoryName(pathToExe);

            IWebHost webHost = BuildWebHost(args).UseContentRoot(pathToContentRoot).Build();
            ILogger logger = webHost.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation(("Starting host"));
                webHost.Run();

                BuildWebHost(args).UseContentRoot(pathToContentRoot).Build().Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Web Host terminated unexpectedly");
            }

            return;
        }
    }
}
