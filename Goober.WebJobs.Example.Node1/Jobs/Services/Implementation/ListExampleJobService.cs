using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Indusoft.Base.Extensions;
using Microsoft.Extensions.Logging;

namespace Indusoft.WebJobs.Example.Jobs.Services.Implementation
{
    class ListExampleJobService : IListExampleJobService
    {
        private readonly ILogger<ListExampleJobService> _logger;

        public ListExampleJobService(ILogger<ListExampleJobService> logger)
        {
            this._logger = logger;
        }

        public async Task ExecuteAwake()
        {
            _logger.LogTrace($"{GetType().Name}.{nameof(ExecuteAwake)}: awake job");
        }

        public async Task<List<int>> GetItemsAsync()
        {
            var ret = new List<int>();

            var rnd = new Random();

            var max = rnd.Next(100, 1000);

            for (int i = 0; i < max; i++)
            {
                ret.Add(i);
            }

            _logger.LogTrace($"{GetType().Name}.{nameof(GetItemsAsync)}: {ret.Serialize()}");

            return ret;
        }

        public async Task ProcessItemAsync(int item, CancellationToken stoppinngToken)
        {
            _logger.LogTrace($"{GetType().Name}.{nameof(ProcessItemAsync)} start processing item: {item.Serialize()}");

            var random = new Random();
            

            var sleep = random.Next(5, 20) * 1000;
            
            await Task.Delay(millisecondsDelay: sleep);

            Console.WriteLine($"ready {item}");

            _logger.LogTrace($"{GetType().Name}.{nameof(ProcessItemAsync)} finish processing item: {item.Serialize()}");
        }
    }
}
