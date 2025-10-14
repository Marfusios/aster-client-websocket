using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aster.Client.Websocket.Client;
using Aster.Client.Websocket.Files;
using Aster.Client.Websocket.Responses.Trades;
using Xunit;

namespace Aster.Client.Websocket.Tests.Integration
{
    public class AsterFileCommunicatorTests
    {
        // ----------------------------------------------------------------
        // Don't forget to decompress gzip files before starting the tests
        // ----------------------------------------------------------------

        [SkippableFact]
        public async Task OnStart_ShouldStreamMessagesFromFile()
        {
            var files = new[]
            {
                "data/aster_raw_xbtusd_2018-11-13.txt"
            };
            foreach (var file in files)
            {
                var exist = File.Exists(file);
                Skip.If(!exist, $"The file '{file}' doesn't exist. Don't forget to decompress gzip file!");
            }

            var trades = new List<Trade>();

            var communicator = new AsterFileCommunicator();
            communicator.FileNames = files;
            communicator.Delimiter = ";;";

            var client = new AsterWebsocketClient(communicator);
            client.Streams.TradesStream.Subscribe(response =>
            {
                trades.Add(response.Data);
            });

            await communicator.Start();

            Assert.Equal(44259, trades.Count);
        }
    }
}
