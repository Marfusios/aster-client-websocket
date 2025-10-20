using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Aster.Client.Websocket.Communicator;
using Aster.Client.Websocket.Exceptions;
using Aster.Client.Websocket.Rest;
using Aster.Client.Websocket.Signing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Websocket.Client;

namespace Aster.Client.Websocket.Websockets
{
    /// <inheritdoc cref="WebsocketClient" />
    public class AsterWebsocketCommunicator : WebsocketClient, IAsterCommunicator
    {
        private readonly ILogger<AsterWebsocketCommunicator> _logger;
        private AsterUserRestApi? _userApi;
        private IDisposable? _disconnectionStream;
        private IDisposable? _timerStream;
        private string? _listenKey;

        /// <inheritdoc />
        public AsterWebsocketCommunicator(Uri url, Func<ClientWebSocket>? clientFactory = null)
            : base(url, clientFactory)
        {
            _logger = NullLogger<AsterWebsocketCommunicator>.Instance;
        }

        /// <inheritdoc />
        public AsterWebsocketCommunicator(Uri url, ILogger<AsterWebsocketCommunicator> logger, Func<ClientWebSocket>? clientFactory = null)
            : base(url, logger, clientFactory)
        {
            _logger = logger;
        }

        public async Task Authenticate(string apiKey, IAsterSignatureService signature)
        {
            // TODO: use IHttpClientFactory
            var http = new HttpClient();
            _userApi = new AsterUserRestApi(apiKey: apiKey, signatureService: signature, httpClient: http);

            await AuthenticateUrl();

            _disconnectionStream = DisconnectionHappened
                .Subscribe(x =>
            {
                if (x.Type == DisconnectionType.ByUser || x.Type == DisconnectionType.Exit)
                    return;
                AuthenticateUrl().Wait();
            });

            _timerStream = Observable
                .Timer(TimeSpan.FromMinutes(55), TimeSpan.FromMinutes(55))
                .Subscribe(x =>
                {
                    _ = RefreshAuthentication();
                });
        }

        public new void Dispose()
        {
            if (_userApi != null && !string.IsNullOrWhiteSpace(_listenKey))
            {
                _ = _userApi.CloseListenKey(_listenKey);
            }
            _userApi = null;
            _disconnectionStream?.Dispose();
            _timerStream?.Dispose();
            base.Dispose();
        }

        private async Task AuthenticateUrl()
        {
            if (_userApi == null)
                return;

            _logger.LogInformation("Getting a new listen key for authenticated websocket connection");
            var response = await _userApi.CreateListenKey();
            _listenKey = response.ListenKey;
            if (string.IsNullOrWhiteSpace(_listenKey))
                throw new AsterException("Listen key is empty, cannot authenticate websocket connection");

            var newUrl = AsterValues.UserWebsocketUrl(_listenKey);
            Url = newUrl;
            _logger.LogInformation("The new listen key was obtained, changing websocket url to {url}", Url);
        }

        private async Task RefreshAuthentication()
        {
            try
            {
                if (_userApi == null || string.IsNullOrWhiteSpace(_listenKey))
                    return;
                _logger.LogInformation("Refreshing listen key to keep it alive");
                await _userApi.KeepAliveListenKey(_listenKey);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Refreshing listen key failed, error: {errorMessage}", e.Message);
            }
        }
    }
}
