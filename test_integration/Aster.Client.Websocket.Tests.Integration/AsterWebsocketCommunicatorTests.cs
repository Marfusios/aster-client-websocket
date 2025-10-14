using System;
using System.Threading;
using System.Threading.Tasks;
using Aster.Client.Websocket;
using Aster.Client.Websocket.Websockets;
using Xunit;

namespace Aster.Client.Websocket.Tests.Integration
{
    public class AsterWebsocketCommunicatorTests
    {
        [Fact]
        public async Task OnStarting_ShouldGetInfoResponse()
        {
            var url = AsterValues.ApiWebsocketUrl;
            using var communicator = new AsterWebsocketCommunicator(url);
            var receivedEvent = new ManualResetEvent(false);

            communicator.MessageReceived.Subscribe(msg =>
            {
                receivedEvent.Set();
            });

            communicator.Url = new Uri(url + "stream?streams=btcusdt@trade");

            await communicator.Start();

            receivedEvent.WaitOne(TimeSpan.FromSeconds(30));
        }
    }
}
