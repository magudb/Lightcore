using System;
using System.Diagnostics;

namespace Lightcore
{
    internal static class Requires
    {
        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty(string instance, string paramName)
        {
            IsNotNull(instance, paramName);

            if (instance.Length == 0)
            {
                throw new ArgumentException("Value can not be empty.", paramName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNull(object instance, string paramName)
        {
            if (instance == null)
            {
                ThrowArgumentNullException(paramName);
            }
        }

        private static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}