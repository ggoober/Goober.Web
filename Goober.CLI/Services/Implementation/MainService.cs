using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.CLI.Services.Implementation
{
    public class MainService : IMainService
    {
        private readonly Func<string[], IServiceProvider, CancellationToken, Task<bool>> _mainDelegate;

        public MainService(
            Func<string[], IServiceProvider, CancellationToken, Task<bool>> mainDelegate
        )
        {
            _mainDelegate = mainDelegate;
        }

        public bool IsTerminated { get; set; }

        public async Task<bool> Main(IServiceProvider serviceProvider, CancellationToken token)
        {
            string[] args;
            var argsService = serviceProvider.GetService<IArgsContextService>();
            if (argsService is not null)
            {
                args = argsService.Args.ToArray();
            }
            else
            {
                args = Environment.GetCommandLineArgs();
            }


            var result = await _mainDelegate.Invoke(args, serviceProvider, token);
            IsTerminated = result;

            return result;
        }
    }
}
