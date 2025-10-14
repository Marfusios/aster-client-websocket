using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.MiniTicker
{
    public class MiniTickerResponse : ResponseBase<MiniTicker>
    {
        internal static bool TryHandle(JObject response, ISubject<MiniTickerResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            if (stream == null)
                return false;

            if (!stream.EndsWith("miniTicker"))
            {
                return false;
            }

            var parsed = response!.ToObject<MiniTickerResponse>(AsterJsonSerializer.Serializer);
            if (parsed != null)
                subject.OnNext(parsed);

            return true;
        }
    }
}