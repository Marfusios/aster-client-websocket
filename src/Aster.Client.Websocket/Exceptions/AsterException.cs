using System;

namespace Aster.Client.Websocket.Exceptions
{
    /// <summary>
    /// Base exception for Aster client
    /// </summary>
    public class AsterException : Exception
    {
        public AsterException()
        {
        }

        public AsterException(string message)
            : base(message)
        {
        }

        public AsterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
