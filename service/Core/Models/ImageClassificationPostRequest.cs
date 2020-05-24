namespace Core.Models
{
    public class ImageClassificationPostRequest
    {
        public string Base64EncodedImage { get; set; }
        public string ClassifierType { get; set; }
    }
}