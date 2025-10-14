using System;

namespace Aster.Client.Websocket.Exceptions
{
    /// <summary>
    /// Invalid user input exception
    /// </summary>
    public class AsterBadInputException : AsterException
    {
        public AsterBadInputException()
        {
        }

        public AsterBadInputException(string message) : base(message)
        {
        }

        public AsterBadInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
