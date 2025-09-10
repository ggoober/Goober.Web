using System.Threading;
using System.Threading.Tasks;

namespace Indusoft.WebJobs.Abstractions
{
    public interface IIterateJobService
    {
        Task ExecuteIterationAsync(CancellationToken stoppingToken);
    }
}
