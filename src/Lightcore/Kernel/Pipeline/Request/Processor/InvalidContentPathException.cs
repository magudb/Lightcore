using System;

namespace Lightcore.Kernel.Pipeline.Request.Processor
{
    public class InvalidContentPathException : Exception
    {
        public InvalidContentPathException(string message) : base(message)
        {
        }
    }
}