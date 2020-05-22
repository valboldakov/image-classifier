using System;

namespace Core.Exceptions
{
    public abstract class ImageClassifierFactoryException : Exception
    {
        protected ImageClassifierFactoryException(string msg) : base(msg)
        {
        }
    }

    public class UnknownClassifierTypeException : ImageClassifierFactoryException
    {
        public UnknownClassifierTypeException(string type) : base($"Unknown {type} classifier type.")
        {
        }
    }
}