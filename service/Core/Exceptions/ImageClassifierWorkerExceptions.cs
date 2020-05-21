using System;

namespace Core.Exceptions
{
    public abstract class ImageClassifierWorkerException : Exception
    {
        protected ImageClassifierWorkerException(string message) : base(message)
        {
        }
    }

    public class UsingCanceledWorkerException : ImageClassifierWorkerException
    {
        public UsingCanceledWorkerException() : base("Can't use canceled worker.")
        {
        }
    }
}