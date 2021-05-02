using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs.Example.Services.Implementation
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

            var max = rnd.Next(10, 100);

            for (int i = 0; i < max; i++)
            {
                ret.Add(i);
            }

            return ret;
        }

        public async Task ProcessItemAsync(int item, CancellationToken stoppinngToken)
        {
            var random = new Random();

            var sleep = random.Next(5, 15) * 1000;

            System.Threading.Thread.Sleep(sleep);
        }
    }
}
