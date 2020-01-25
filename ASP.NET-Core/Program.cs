using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ASPNETCore
{
    /// <summary>
    /// Entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// entry mehod
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                var isDebugging = Debugger.IsAttached || args.Contains("--debug");
                var contentRoot = Directory.GetCurrentDirectory();

                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false);

                if (isDebugging)
                    config.AddJsonFile("appsettings.development.json", optional: true);
                else
                    config.AddJsonFile("appsettings.production.json", optional: true);

                var host = BuildHost(args, contentRoot);//.Build().Run();

                logger.Debug("init main");
                logger.Debug($"path to appsetting.json file set to: {System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath)}");

                    Console.Title = "ASPNETCore";

                    logger.Debug("Starting app");
                    host.Run();
            }
            catch (Exception e)
            {
                //NLog: catch setup errors
                logger.Error(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// create host CreateHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <param name="contentRoot"></param>
        /// <returns></returns>
        public static IHost BuildHost(string[] args, string contentRoot) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(contentRoot)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                })
                .ConfigureAppConfiguration((hostContext, c) =>
                {
                    c.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()  // NLog: Setup NLog for Dependency injection
                .Build();

    }
}
