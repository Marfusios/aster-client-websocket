using System;
using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.UserData
{
    public class AccountConfigUpdateEvent : MessageBase
    {
        internal static bool TryHandle(JObject? response, ISubject<AccountConfigUpdateEvent> subject)
        {
            var stream = response?["e"]?.Value<string>();
            if (!string.Equals(stream, "ACCOUNT_CONFIG_UPDATE", StringComparison.OrdinalIgnoreCase))
                return false;

            var parsed = response!.ToObject<AccountConfigUpdateEvent>(AsterJsonSerializer.Serializer);
            if (parsed != null)
            {
                subject.OnNext(parsed);
            }

            return true;
        }

        [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? TransactionTime { get; set; }

        [JsonProperty("ac")]
        public AccountConfigSymbol? SymbolConfig { get; set; }

        [JsonProperty("ai")]
        public AccountConfigInfo? AccountConfig { get; set; }
    }

    public class AccountConfigSymbol
    {
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        [JsonProperty("l")]
        public int Leverage { get; set; }
    }

    public class AccountConfigInfo
    {
        [JsonProperty("j")]
        public bool MultiAssetsMode { get; set; }
    }
}
