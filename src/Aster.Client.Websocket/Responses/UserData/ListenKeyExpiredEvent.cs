using System;
using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.UserData
{
    public class ListenKeyExpiredEvent : MessageBase
    {
        internal static bool TryHandle(JObject? response, ISubject<ListenKeyExpiredEvent> subject)
        {
            var stream = response?["e"]?.Value<string>();
            if (!string.Equals(stream, "listenKeyExpired", StringComparison.OrdinalIgnoreCase))
                return false;

            var parsed = response!.ToObject<ListenKeyExpiredEvent>(AsterJsonSerializer.Serializer);
            if (parsed != null)
            {
                subject.OnNext(parsed);
            }

            return true;
        }
    }
}
