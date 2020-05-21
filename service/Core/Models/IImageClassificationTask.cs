using System.Drawing;
using System.Threading.Tasks;

namespace Core.Models
{
    public interface IImageClassificationTask
    {
        Image Image { get; }
        TaskCompletionSource<string> TaskCompletionSource { get; }
    }
}