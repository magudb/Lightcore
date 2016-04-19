using System;

namespace Lightcore.Configuration
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException(string message) : base(message)
        {
        }
    }
}