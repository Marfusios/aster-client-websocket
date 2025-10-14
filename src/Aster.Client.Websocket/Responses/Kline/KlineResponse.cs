using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.Kline
{
    public class KlineResponse : ResponseBase<Kline>
    {
        internal static bool TryHandle(JObject response, ISubject<KlineResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            if (stream == null)
            {
                return false;
            }

            if (!stream.Contains("kline"))
            {
                return false;
            }

            var parsed = response!.ToObject<KlineResponse>(AsterJsonSerializer.Serializer);
            if (parsed != null)
                subject.OnNext(parsed);

            return true;
        }
    }
}