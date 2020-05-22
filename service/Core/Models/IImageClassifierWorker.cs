using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Models
{
    public interface IImageClassifierWorker
    {
        Task<string> Classify(Image image);

        void StartWorker(CancellationToken cancellationToken);

        Thread Thread { get; }
    }
}