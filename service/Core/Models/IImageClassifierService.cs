using System.Drawing;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Core.Models
{
    public interface IImageClassifierService : IHostedService
    {
        public Task<string> Classify(string classifierType, Image image);
    }
}