using System;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs.Example.Services.Implementation
{
    public class IterateExampleJobService : IIterateExampleJobService
    {
        public async Task ExecuteIterationAsync(CancellationToken stoppingToken)
        {
            System.Threading.Thread.Sleep(10000);
        }
    }
}
