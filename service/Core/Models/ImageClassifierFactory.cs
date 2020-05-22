using Core.Exceptions;

namespace Core.Models
{
    public class ImageClassifierFactory : IImageClassifierFactory
    {
        public IImageClassifier GetClassifier(ImageClassifierConfiguration configuration)
        {
            if (configuration.ClassifierType == "MNISTClassifier")
            {
                return new MnistImageClassifier(configuration);
            }

            throw new UnknownClassifierTypeException(configuration.ClassifierType);
        }
    }
}