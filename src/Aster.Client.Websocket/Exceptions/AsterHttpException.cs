using System;
using System.Collections.Generic;

namespace Aster.Client.Websocket.Exceptions
{
    /// <summary>
    /// Aster exception class for any errors throw as a result of communication via http.
    /// </summary>
    public class AsterHttpException : Exception
    {
        public AsterHttpException()
        : base()
        {
        }

        public AsterHttpException(string message)
        : base(message)
        {
        }

        public AsterHttpException(string message, Exception innerException)
        : base(message, innerException)
        {
        }

        public int StatusCode { get; set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; set; }
    }
}