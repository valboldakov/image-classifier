using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Models
{
    public interface IImageClassifierWorkersPool
    {
        public Task<string> Classify(Image image);

        public Task StartWorkers(CancellationToken cancellationToken);

        public Task StopWorkers();
    }
}