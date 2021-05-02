using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs.Abstractions
{
    public interface IListJobService<TItem>
    {
        Task<List<TItem>> GetItemsAsync();

        Task ProcessItemAsync(TItem item, CancellationToken stoppinngToken);
    }
}
