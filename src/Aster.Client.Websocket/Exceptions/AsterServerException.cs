using System;

namespace Aster.Client.Websocket.Exceptions
{
    /// <summary>
    /// Aster exception class for any errors throw as a result internal server issues.
    /// </summary>
    public class AsterServerException : AsterHttpException
    {
        public AsterServerException()
        : base()
        {
        }

        public AsterServerException(string message)
        : base(message)
        {
            this.Message = message;
        }

        public AsterServerException(string message, Exception innerException)
        : base(message, innerException)
        {
            this.Message = message;
        }

        public new string Message { get; protected set; }
    }
}