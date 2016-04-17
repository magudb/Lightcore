using System;

namespace Lightcore.Redis
{
    public class MissingIdInIndexException : Exception
    {
        public MissingIdInIndexException(string message) : base(message)
        {
        }
    }
}