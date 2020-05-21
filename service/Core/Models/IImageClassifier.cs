using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Core.Models
{
    public interface IImageClassifier
    {
        public string ClassifyImage(Image image);
        public void LoadModel(string pathToModel);

        [DllImport("imageclassifier.so", CallingConvention = CallingConvention.Cdecl, EntryPoint = "load_model")]
        protected static extern int LibLoadModel(string modelConfig);

        [DllImport("imageclassifier.so", CallingConvention = CallingConvention.Cdecl, EntryPoint = "eval_model")]
        protected static extern unsafe int LibEvalModel(void* data, int size, int labelsAmount, void* labelNum);
    }
}