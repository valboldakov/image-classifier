using Core.Exceptions;
using Core.Models;
using Xunit;

namespace Test
{
    public class ImageClassifierFactoryTest
    {
        [Fact]
        public void SmokeTestClassifierFactory()
        {
            var configuration = new ImageClassifierConfiguration
            {
                ClassifierType = "MNISTClassifier",
                ImageHeight = 28, ImageWidth = 28,
                Labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"},
                PathToModel = "TestData/traced_model.pt"
            };
            var factory = new ImageClassifierFactory();
            var classifier = factory.GetClassifier(configuration);
            Assert.IsType<MnistImageClassifier>(classifier);
        }

        [Fact]
        public void UnknowClassifierTypeExceptionThrownTest()
        {
            var configuration = new ImageClassifierConfiguration
            {
                ClassifierType = "Unknown",
                ImageHeight = 28, ImageWidth = 28,
                Labels = new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"},
                PathToModel = "TestData/traced_model.pt"
            };
            var factory = new ImageClassifierFactory();
            Assert.Throws<UnknownClassifierTypeException>(() => factory.GetClassifier(configuration));
        }
    }
}