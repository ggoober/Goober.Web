using System.Net;
using System.Threading.Tasks;

namespace Goober.BackgroundWorker.Abstractions
{
    public interface IUdpBackgroundService
    {
        Task<byte[]> OnReceivedAsync(EndPoint inboundEndPoint, byte[] buffer, IPEndPoint senderEndPoint);
    }
}
