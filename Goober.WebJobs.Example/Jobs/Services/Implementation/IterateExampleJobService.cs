using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs.Example.Jobs.Services.Implementation
{
    public class IterateExampleJobService : IIterateExampleJobService
    {
        public async Task ExecuteIterationAsync(CancellationToken stoppingToken)
        {
            Debug.WriteLine($"{GetType().Name}.{nameof(ExecuteIterationAsync)} start processing iteration");

            var random = new Random();

            var sleep = random.Next(5, 60) * 1000;

            await Task.Delay(millisecondsDelay: sleep);

            Debug.WriteLine($"{GetType().Name}.{nameof(ExecuteIterationAsync)} finish processing iteration");
        }
    }
}
