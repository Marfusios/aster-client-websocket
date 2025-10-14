using System.Linq;
using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.MarkPrice
{
    public class FundingResponse : ResponseBase<Funding>
    {
        internal static bool TryHandle(JObject response, ISubject<FundingResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();

            if (stream == null)
                return false;

            if (!stream.Contains("markPrice"))
                return false;

            var parsed = response!.ToObject<FundingResponse>(AsterJsonSerializer.Serializer);
            if (parsed != null)
            {
                parsed.Data.Symbol = stream.Split('@').FirstOrDefault();
                subject.OnNext(parsed);
            }

            return true;
        }
    }
}