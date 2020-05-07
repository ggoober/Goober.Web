using System.Threading;
using System.Threading.Tasks;

namespace Goober.BackgroundWorker.Abstractions
{
    public interface IIterateBackgroundService
    {
        Task ExecuteIterationAsync(CancellationToken stoppingToken);
    }
}
