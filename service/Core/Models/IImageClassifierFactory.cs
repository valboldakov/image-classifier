namespace Core.Models
{
    public interface IImageClassifierFactory
    {
        public IImageClassifier GetClassifier(ImageClassifierConfiguration configuration);
    }
}