using System.Linq;
using System.Reactive.Subjects;
using Aster.Client.Websocket.Communicator;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Aster.Client.Websocket.Responses.Books
{
    /// <summary>
    /// Partial order book response
    /// </summary>
    public class OrderBookPartialResponse : ResponseBase<OrderBookPartial>
    {

        internal static bool TryHandle(JObject response, ISubject<OrderBookPartialResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            if (stream == null)
                return false;

            if (!stream.Contains("depth"))
                return false;

            if (stream.EndsWith("depth"))
            {
                // ignore, not partial, but diff response
                return false;
            }

            var parsed = response.ToObject<OrderBookPartialResponse>(AsterJsonSerializer.Serializer);
            if (parsed?.Data != null)
            {
                parsed.Data.Symbol = stream.Split('@').FirstOrDefault();
                subject.OnNext(parsed);
            }

            return true;
        }

        /// <summary>
        /// Stream snapshot manually via communicator
        /// </summary>
        public static void StreamFakeSnapshot(OrderBookPartial snapshot, IAsterCommunicator communicator)
        {
            var symbolSafe = (snapshot?.Symbol ?? string.Empty).ToLower();
            var countSafe = snapshot?.Bids?.Length ?? 0;
            var response = new OrderBookPartialResponse();
            response.Data = snapshot;
            response.Stream = $"{symbolSafe}@depth{countSafe}";

            var serialized = JsonConvert.SerializeObject(response, AsterJsonSerializer.Settings);
            communicator.StreamFakeMessage(ResponseMessage.TextMessage(serialized));
        }
    }
}
