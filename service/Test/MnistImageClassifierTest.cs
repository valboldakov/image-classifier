using System.Drawing;
using Core.Exceptions;
using Core.Models;
using Xunit;

namespace Test
{
    public class MnistImageClassifierTest
    {
        [Fact]
        public void ModelNotLoadedExceptionThrownTest()
        {
            var configuration = new ImageClassifierConfiguration
            {
                ImageHeight = 28, ImageWidth = 28,
                Labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"},
                PathToModel = "TestData/not_exist.pt"
            };
            var classifier = new MnistImageClassifier(configuration);
            Assert.Throws<ModelNotLoadedException>(() => classifier.LoadModel());
        }

        [Fact]
        public void RecognizeNumberTest()
        {
            var configuration = new ImageClassifierConfiguration
            {
                ImageHeight = 28, ImageWidth = 28,
                Labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"},
                PathToModel = "TestData/traced_model.pt"
            };
            var classifier = new MnistImageClassifier(configuration);
            classifier.LoadModel();
            var image = Image.FromFile("TestData/seven.png");
            var label = classifier.ClassifyImage(image);
            Assert.Contains(label, configuration.Labels);
        }
    }
}