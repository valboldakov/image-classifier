using System.Drawing;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ImageClassificationTask : IImageClassificationTask
    {
        public Image Image { get; set; }
        public TaskCompletionSource<string> TaskCompletionSource { get; set; }
    }
}