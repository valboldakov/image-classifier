using System.Drawing;
using System.Threading.Tasks;

namespace Core.Models
{
    public interface IImageClassifierService
    {
        public Task<string> Classify(string classifierType, Image image);
    }
}