using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.CLI.Services.Implementation
{
    public class AfterStartHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMainService _mainService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public AfterStartHostService(
            IServiceProvider serviceProvider,
            IMainService mainService,
            IHostApplicationLifetime hostApplicationLifetime
        )
        {
            _serviceProvider = serviceProvider;
            _mainService = mainService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _mainService.Main(_serviceProvider, cancellationToken);
            await StopAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _hostApplicationLifetime.StopApplication();
            return Task.CompletedTask;
        }
    }
}
