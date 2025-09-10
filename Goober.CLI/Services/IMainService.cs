using System;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.CLI.Services
{
    public interface IMainService
    {
        public bool IsTerminated { get; set; }
        public Task<bool> Main(
            IServiceProvider serviceProvider,
            CancellationToken token
        );
    }
}
