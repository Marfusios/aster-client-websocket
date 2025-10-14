using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.AggregateTrades
{
    /// <summary>
    /// Trades bin response, contains all trades executed in a selected time range
    /// </summary>
    public class AggregatedTradeResponse : ResponseBase<AggregateTrade>
    {

        internal static bool TryHandle(JObject response, ISubject<AggregatedTradeResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            if (stream == null || !stream.ToLower().EndsWith("@aggtrade"))
                return false;

            var parsed = response!.ToObject<AggregatedTradeResponse>(AsterJsonSerializer.Serializer);
            if (parsed != null)
                subject.OnNext(parsed);

            return true;
        }
    }
}
