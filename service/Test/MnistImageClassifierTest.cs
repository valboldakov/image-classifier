using System.Drawing;
using Core.Exceptions;
using Core.Models;
using Xunit;

namespace Test
{
    public class MnistImageClassifierTest
    {
        private readonly ImageClassifierConfiguration _configuration;

        public MnistImageClassifierTest()
        {
            _configuration = new ImageClassifierConfiguration
            {
                ImageHeight = 28, ImageWidth = 28,
                Labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
            };
        }

        [Fact]
        public void ModelNotLoadedExceptionThrownTest()
        {
            var classifier = new MnistImageClassifier(_configuration);
            Assert.Throws<ModelNotLoadedException>(() => classifier.LoadModel("TestData/not_exist.pt"));
        }
        
        [Fact]
        public void RecognizeNumberTest()
        {
            var classifier = new MnistImageClassifier(_configuration);
            classifier.LoadModel("TestData/traced_model.pt");
            var image = Image.FromFile("TestData/seven.png");
            var label = classifier.ClassifyImage(image);
            Assert.Contains(label, _configuration.Labels);
        }
    }
}