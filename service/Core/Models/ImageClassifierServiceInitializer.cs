using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Core.Models
{
    public class ImageClassifierServiceInitializer : IHostedService
    {
        private IImageClassifierService _service;

        public ImageClassifierServiceInitializer(IImageClassifierService service)
        {
            _service = service;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _service.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _service.StopAsync(cancellationToken);
        }
    }
}