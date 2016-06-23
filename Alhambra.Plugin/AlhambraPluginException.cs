using System;

namespace Alhambra
{
    internal class AlhambraPluginException : Exception
    {
        public AlhambraPluginException(string message)
            : base(message) { }

        public AlhambraPluginException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
