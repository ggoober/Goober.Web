using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Indusoft.WebJobs.Abstractions
{
    public interface IListJobService<TItem>
    {
        Task<List<TItem>> GetItemsAsync();
        Task ExecuteAwakeAsync();
        Task ProcessItemAsync(TItem item, CancellationToken stoppinngToken);
    }
}
