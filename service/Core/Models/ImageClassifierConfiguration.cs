namespace Core.Models
{
    /// <summary>
    /// Class <c>ImageClassifierConfiguration</c> is for getting values from appsetings config at
    /// startup time.
    /// </summary>
    public class ImageClassifierConfiguration
    {
        public string ClassifierType { get; set; }
        public string[] Labels { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public string PathToModel { get; set; }
    }
}