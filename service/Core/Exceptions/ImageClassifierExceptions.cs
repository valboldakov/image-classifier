using System;

namespace Core.Exceptions
{
    public abstract class ImageClassifierException : Exception
    {
        public string Information;

        protected ImageClassifierException(string message, string information) : base($"{message} {information}")
        {
            Information = information;
        }
    }

    public class ModelNotLoadedException : ImageClassifierException
    {
        public ModelNotLoadedException(string information = "") : base("Couldn't load model.", information)
        {
        }
    }

    public class ModelEvaluationFailedException : ImageClassifierException
    {
        public ModelEvaluationFailedException(string information = "") : base("Couldn't evaluate model.", information)
        {
        }
    }
}