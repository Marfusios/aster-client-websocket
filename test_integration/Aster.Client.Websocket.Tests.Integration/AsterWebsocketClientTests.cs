using System;
using System.Threading;
using System.Threading.Tasks;
using Aster.Client.Websocket;
using Aster.Client.Websocket.Client;
using Aster.Client.Websocket.Responses.Trades;
using Aster.Client.Websocket.Subscriptions;
using Aster.Client.Websocket.Websockets;
using Xunit;

namespace Aster.Client.Websocket.Tests.Integration
{
    public class AsterWebsocketClientTests
    {
        [Fact(Skip = "Temporarily disable, not working in CI")]
        public async Task Connect_ShouldWorkAndReceiveResponse()
        {
            var url = AsterValues.ApiWebsocketUrl;
            using var communicator = new AsterWebsocketCommunicator(url);
            TradeResponse received = null;
            var receivedEvent = new ManualResetEvent(false);

            using var client = new AsterWebsocketClient(communicator);
            client.Streams.TradesStream.Subscribe(response =>
            {
                received = response;
                receivedEvent.Set();
            });

            client.SetSubscriptions(
                new TradeSubscription("btcusdt")
            );

            await communicator.Start();

            receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

            Assert.NotNull(received);
        }

    }
}
