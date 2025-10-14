using System;

namespace Aster.Client.Websocket
{
    /// <summary>
    /// Aster static urls
    /// </summary>
    public static class AsterValues
    {
        /// <summary>
        /// Market data websocket API url for spot
        /// </summary>
        public static readonly Uri SpotApiWebsocketUrl = new Uri("wss://sstream.asterdex.com");

        /// <summary>
        /// Market data websocket API url for futures
        /// </summary>
        public static readonly Uri FuturesApiWebsocketUrl = new Uri("wss://fstream.asterdex.com");

        /// <summary>
        /// User data websocket API url
        /// </summary>
        /// <param name="listenKey">Create user's listen key via separate API</param>
        public static Uri UserWebsocketUrl(string listenKey) =>
            new Uri($"wss://fstream.asterdex.com/ws/{listenKey}");
    }
}
