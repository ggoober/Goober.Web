using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs.Abstractions
{
    public interface IIterateJobService
    {
        Task ExecuteIterationAsync(CancellationToken stoppingToken);
    }
}
