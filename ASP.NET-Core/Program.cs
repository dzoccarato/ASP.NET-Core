using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ASPNETCore
{
    using ASPNETCore.Configuration;
    using ASPNETCore.MethodExtensions;

    /// <summary>
    /// Entry point
    /// </summary>
    public class Program
    {
        private static readonly string _defaultAvailableUrl = $"http://+:5001/";

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
                var currentProcess = Process.GetCurrentProcess();
                var isDebugging = Debugger.IsAttached || args.Contains("--console");
                var contentRoot = isDebugging ? Directory.GetCurrentDirectory() : Path.GetDirectoryName(currentProcess.MainModule.FileName);

                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false);

                if (isDebugging)
                    config.AddJsonFile("appsettings.development.json", optional: true);
                else
                    config.AddJsonFile("appsettings.production.json", optional: true);

                var urls = GetAvailableUrls();
                var host = BuildHost(args.Where(arg => arg != "--console").ToArray(), contentRoot, urls);

                var log = new StringBuilder()
                    .AppendLine($"init main / starting app: ")
                    .AppendLine($"- isDebugging: {isDebugging}")
                    .AppendLine($"- contentRoot: {contentRoot}")
                    .AppendLine($"- currentProcess.MainModule.FileName: {currentProcess.MainModule.FileName}")
                    .AppendLine($"- currentProcess.MainModule.FileVersionInfo: {currentProcess.MainModule.FileVersionInfo.FileVersion}")
                    .AppendLine($"- path to appsetting.json file set to: {contentRoot}\\appsetting.json")
                    .AppendLine($"- listen on urls:");
                foreach (var u in urls)
                    log.AppendLine($"  - {u}");
                logger.Debug(log.ToString());

                Console.Title = $"{currentProcess.MainModule.ModuleName} - v.{currentProcess.MainModule.FileVersionInfo.FileVersion}";

                if (!isDebugging)
                {
                    logger.Debug($"Start as Windows service");
                    host.RunAsCustomService();
                }
                else
                {
                    logger.Debug($"Start as Console");
                    host.Run();
                }
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
        /// <param name="urls"></param>
        /// <returns></returns>
        public static IWebHost BuildHost(string[] args, string contentRoot, string[] urls) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(contentRoot)
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls(urls)
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

        /// <summary>
        /// read configuration and get availabe urls
        /// </summary>
        /// <returns></returns>
        private static string[] GetAvailableUrls()
        {
            IConfigurationRoot configuration = ConfigurationUtils.BuildConfiguration();
            var serviceSettings = configuration.Get<ServiceSettings>(ConfigurationKeys.ServiceSettings);

            return serviceSettings?.Urls == null || serviceSettings?.Urls.Length > 0 ? serviceSettings.Urls : new List<string> { _defaultAvailableUrl }.ToArray();
        }

    }
}
