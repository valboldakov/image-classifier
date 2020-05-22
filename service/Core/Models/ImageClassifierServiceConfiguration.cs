namespace Core.Models
{
    public class ImageClassifierServiceConfiguration
    {
        public int WorkersPerModel { get; set; }
        public ImageClassifierConfiguration[] ImageClassifierConfigurations { get; set; }
    }
}