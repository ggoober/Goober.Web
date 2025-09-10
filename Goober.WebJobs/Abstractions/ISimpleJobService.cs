using System.Threading;
using System.Threading.Tasks;

namespace Indusoft.WebJobs.Abstractions
{
    public interface ISimpleJobService
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
