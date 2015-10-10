using System;

namespace Lightcore.Kernel.Pipelines.Request.Processors.Exceptions
{
    public class InvalidContentPathException : Exception
    {
        public InvalidContentPathException(string message) : base(message)
        {
        }
    }
}