using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Models
{
    public interface IImageClassifierWorker
    {
        Task<string> Classify(Image image);

        Task<int> StartWorker(CancellationToken cancellationToken);

        void StopWorker();
    }
}