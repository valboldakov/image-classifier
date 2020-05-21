using System;

namespace Core.Exceptions
{
    public abstract class ImageClassifierException : Exception
    {
        protected ImageClassifierException(string message) : base(message)
        {
        }
    }

    public class ModelNotLoadedException : ImageClassifierException
    {
        public ModelNotLoadedException() : base("Couldn't load model.")
        {
        }
    }

    public class ModelEvaluationFailedException : ImageClassifierException
    {
        public ModelEvaluationFailedException() : base("Couldn't evaluate model.")
        {
        }
    }
}