using Goober.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs.Example.Jobs.Services.Implementation
{
    class ListExampleJobService : IListExampleJobService
    {
        public ListExampleJobService()
        {
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

            Debug.WriteLine($"{GetType().Name}.{nameof(GetItemsAsync)}: {ret.Serialize()}");

            return ret;
        }

        public async Task ProcessItemAsync(int item, CancellationToken stoppinngToken)
        {
            Debug.WriteLine($"{GetType().Name}.{nameof(ProcessItemAsync)} start processing item: {item.Serialize()}");

            var random = new Random();

            var sleep = random.Next(5, 60) * 1000;

            await Task.Delay(millisecondsDelay: sleep);

            Debug.WriteLine($"{GetType().Name}.{nameof(ProcessItemAsync)} finish processing item: {item.Serialize()}");
        }
    }
}
