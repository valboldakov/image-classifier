namespace Core.Models
{
    /// <summary>
    /// Class <c>ImageClassifierConfiguration</c> is for getting values from appsetings config at
    /// startup time.
    /// </summary>
    public class ImageClassifierConfiguration
    {
        public string[] Labels;
        public int ImageWidth;
        public int ImageHeight;
        public string PathToModel;
    }
}