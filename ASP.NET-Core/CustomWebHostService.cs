using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASPNETCore
{
    internal class CustomWebHostService : WebHostService
    {
        private ILogger _logger;

        public CustomWebHostService(IWebHost host) : base(host)
        {
            _logger = host.Services
                .GetRequiredService<ILogger<CustomWebHostService>>();
        }

        protected override void OnStarting(string[] args)
        {
            _logger.LogInformation($"Starting");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            _logger.LogInformation($"Started");
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation($"Stopping");
            base.OnStopping();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation($"Stopped");
            base.OnStopped();
        }
    }
}
