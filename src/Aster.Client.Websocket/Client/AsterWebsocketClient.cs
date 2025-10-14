using System;
using System.Linq;
using Aster.Client.Websocket.Communicator;
using Aster.Client.Websocket.Exceptions;
using Aster.Client.Websocket.Json;
using Aster.Client.Websocket.Responses;
using Aster.Client.Websocket.Responses.AggregateTrades;
using Aster.Client.Websocket.Responses.Books;
using Aster.Client.Websocket.Responses.BookTickers;
using Aster.Client.Websocket.Responses.Kline;
using Aster.Client.Websocket.Responses.MarkPrice;
using Aster.Client.Websocket.Responses.MiniTicker;
using Aster.Client.Websocket.Responses.Orders;
using Aster.Client.Websocket.Responses.Trades;
using Aster.Client.Websocket.Responses.UserData;
using Aster.Client.Websocket.Subscriptions;
using Aster.Client.Websocket.Validations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Aster.Client.Websocket.Client
{
    /// <summary>
    /// Aster websocket client.
    /// Use method `Connect()` to start client and subscribe to channels.
    /// And `Streams` to subscribe. 
    /// </summary>
    public class AsterWebsocketClient : IDisposable
    {
        private readonly IAsterCommunicator _communicator;
        private readonly IDisposable _messageReceivedSubscription;
        private readonly ILogger<AsterWebsocketClient> _logger;

        /// <summary>
        /// Create instance of AsterWebsocketClient
        /// </summary>
        public AsterWebsocketClient(IAsterCommunicator communicator, ILogger<AsterWebsocketClient>? logger = null)
        {
            AsterValidations.ValidateInput(communicator, nameof(communicator));

            _communicator = communicator;
            _logger = logger ?? NullLogger<AsterWebsocketClient>.Instance;
            _messageReceivedSubscription = _communicator.MessageReceived.Subscribe(HandleMessage);
        }

        /// <summary>
        /// Provided message streams
        /// </summary>
        public AsterClientStreams Streams { get; } = new AsterClientStreams();

        /// <summary>
        /// Expose logger for this client
        /// </summary>
        public ILogger<AsterWebsocketClient> Logger => _logger;

        /// <summary>
        /// Cleanup everything
        /// </summary>
        public void Dispose()
        {
            _messageReceivedSubscription?.Dispose();
        }

        /// <summary>
        /// Combine url with subscribed streams
        /// </summary>
        public Uri PrepareSubscriptions(Uri baseUrl, params SubscriptionBase[] subscriptions)
        {
            AsterValidations.ValidateInput(baseUrl, nameof(baseUrl));

            if (subscriptions == null || !subscriptions.Any())
                throw new AsterBadInputException("Please provide at least one subscription");

            var streams = subscriptions.Select(x => x.StreamName).ToArray();
            var urlPart = string.Join("/", streams);
            var urlPartFull = $"/stream?streams={urlPart}";

            var currentUrl = baseUrl.ToString().Trim();

            if (currentUrl.Contains("stream?"))
            {
                // do nothing, already configured
                return baseUrl;
            }

            var newUrl = new Uri($"{currentUrl.TrimEnd('/')}{urlPartFull}");
            return newUrl;
        }

        /// <summary>
        /// Combine url with subscribed streams and set it into communicator.
        /// Then you need to call communicator.Start() or communicator.Reconnect()
        /// </summary>
        public void SetSubscriptions(params SubscriptionBase[] subscriptions)
        {
            var newUrl = PrepareSubscriptions(_communicator.Url, subscriptions);
            _communicator.Url = newUrl;
        }

        /// <summary>
        /// Serializes request and sends message via websocket communicator. 
        /// It logs and re-throws every exception. 
        /// </summary>
        /// <param name="request">Request/message to be sent</param>
        public bool Send<T>(T request)
        {
            try
            {
                AsterValidations.ValidateInput(request, nameof(request));

                var serialized = AsterJsonSerializer.Serialize(request!);
                return _communicator.Send(serialized);
            }
            catch (Exception e)
            {
                _logger.LogError(e, L("Exception while sending message '{request}'. Error: {error}"), request, e.Message);
                throw;
            }
        }

        private string L(string msg)
        {
            return $"[ASTER WEBSOCKET CLIENT] {msg}";
        }

        private void HandleMessage(ResponseMessage message)
        {
            try
            {
                bool handled;
                var messageSafe = (message.Text ?? string.Empty).Trim();

                if (messageSafe.StartsWith("{"))
                {
                    handled = HandleObjectMessage(messageSafe);
                    if (handled)
                        return;
                }

                handled = HandleRawMessage(messageSafe);
                if (handled)
                    return;

                _logger.LogWarning(L("Unhandled response:  '{message}'"), messageSafe);
            }
            catch (Exception e)
            {
                _logger.LogError(e, L("Exception while receiving message, error: {error}"), e.Message);
            }
        }

        private bool HandleRawMessage(string msg)
        {
            // ********************
            // ADD RAW HANDLERS BELOW
            // ********************

            return
                PongResponse.TryHandle(msg, Streams.PongSubject) ||
                LogUnhandled(msg);
        }

        private bool HandleObjectMessage(string msg)
        {
            var response = AsterJsonSerializer.Deserialize<JObject>(msg);

            // ********************
            // ADD OBJECT HANDLERS BELOW
            // ********************

            return
                TradeResponse.TryHandle(response, Streams.TradesSubject) ||
                AggregatedTradeResponse.TryHandle(response, Streams.TradeBinSubject) ||
                OrderBookPartialResponse.TryHandle(response, Streams.OrderBookPartialSubject) ||
                OrderBookDiffResponse.TryHandle(response, Streams.OrderBookDiffSubject) ||
                OrderUpdate.TryHandle(response, Streams.OrderUpdateSubject) ||
                MarginCallEvent.TryHandle(response, Streams.MarginCallSubject) ||
                AccountUpdateEvent.TryHandle(response, Streams.AccountUpdateSubject) ||
                AccountConfigUpdateEvent.TryHandle(response, Streams.AccountConfigUpdateSubject) ||
                ListenKeyExpiredEvent.TryHandle(response, Streams.ListenKeyExpiredSubject) ||
                FundingResponse.TryHandle(response, Streams.FundingSubject) ||
                BookTickerResponse.TryHandle(response, Streams.BookTickerSubject) ||
                KlineResponse.TryHandle(response, Streams.KlineSubject) ||
                MiniTickerResponse.TryHandle(response, Streams.MiniTickerSubject) ||
                AllMarketMiniTickerResponse.TryHandle(response, Streams.AllMarketMiniTickerSubject) ||
                LogUnhandled(msg);
        }
        
        private bool LogUnhandled(string message)
        {
            Logger.LogDebug("Received unhandled message: {message}", message);
            return true;
        }
    }
}
