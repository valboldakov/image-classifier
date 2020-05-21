using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Core.Models
{
    public interface IImageClassifier
    {
        public string ClassifyImage(Image image);
        public void LoadModel();

        [DllImport("imageclassifier.so", CallingConvention = CallingConvention.Cdecl, EntryPoint = "load_model")]
        protected static extern int LibLoadModel(string modelConfig);

        [DllImport("imageclassifier.so", CallingConvention = CallingConvention.Cdecl, EntryPoint = "eval_model")]
        protected static extern unsafe int LibEvalModel(void* data, int size, int labelsAmount, void* labelNum);

        [DllImport("imageclassifier.so", CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_last_error")]
        protected static extern int LibGetLastError(StringBuilder msg, int maxMsgSize);
    }
}