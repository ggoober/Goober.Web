using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs.Abstractions
{
    public interface ISimpleJobService
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
