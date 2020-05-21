using System;
using System.Drawing;
using System.Text;
using Core.Exceptions;

namespace Core.Models
{
    public class MnistImageClassifier : IImageClassifier
    {
        private readonly ImageClassifierConfiguration _configuration;

        private float[] PreprocessImage(Image image)
        {
            var resizedImage = new Bitmap(image, _configuration.ImageWidth, _configuration.ImageHeight);
            var resultData = new float[resizedImage.Height * resizedImage.Width];
            var mean = 0.5f;
            var std = 0.5f;
            var i = 0;
            for (var x = 0; x < resizedImage.Width; ++x)
            {
                for (var y = 0; y < resizedImage.Height; ++y)
                {
                    var color = resizedImage.GetPixel(x, y);
                    var value = (float) (color.R + color.G + color.B) / 3;
                    resultData[i] = (value - mean) / std;
                    i++;
                }
            }

            return resultData;
        }

        public MnistImageClassifier(ImageClassifierConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ClassifyImage(Image image)
        {
            var data = new Memory<float>(PreprocessImage(image));
            var labelNum = 0;
            int res;
            unsafe
            {
                using var memoryHandle = data.Pin();
                res = IImageClassifier.LibEvalModel(memoryHandle.Pointer,
                    _configuration.ImageHeight * _configuration.ImageWidth,
                    _configuration.Labels.Length, &labelNum);
            }

            if (res == 0) return _configuration.Labels[labelNum];
            var msg = new StringBuilder(512);
            IImageClassifier.LibGetLastError(msg, 512);
            throw new ModelEvaluationFailedException(msg.ToString());
        }

        public void LoadModel()
        {
            var res = IImageClassifier.LibLoadModel(_configuration.PathToModel);
            if (res != 0)
            {
                throw new ModelNotLoadedException();
            }
        }
    }
}