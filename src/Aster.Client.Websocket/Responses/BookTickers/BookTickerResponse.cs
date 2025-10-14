using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.BookTickers
{
    public class BookTickerResponse : ResponseBase<BookTicker>
    {
        internal static bool TryHandle(JObject response, ISubject<BookTickerResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            if (stream == null)
            {
                return false;
            }

            if (!stream.EndsWith("bookTicker"))
            {
                return false;
            }

            var parsed = response!.ToObject<BookTickerResponse>(AsterJsonSerializer.Serializer);
            if (parsed != null)
                subject.OnNext(parsed);

            return true;
        }
    }
}