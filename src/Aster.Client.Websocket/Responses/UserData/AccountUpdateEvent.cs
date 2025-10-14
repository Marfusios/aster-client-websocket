using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.UserData
{
    public class AccountUpdateEvent : MessageBase
    {
        internal static bool TryHandle(JObject? response, ISubject<AccountUpdateEvent> subject)
        {
            var stream = response?["e"]?.Value<string>();
            if (!string.Equals(stream, "ACCOUNT_UPDATE", StringComparison.OrdinalIgnoreCase))
                return false;

            var parsed = response!.ToObject<AccountUpdateEvent>(AsterJsonSerializer.Serializer);
            if (parsed != null)
            {
                subject.OnNext(parsed);
            }

            return true;
        }

        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? TransactionTime { get; set; }

        /// <summary>
        /// Account update payload
        /// </summary>
        [JsonProperty("a")]
        public AccountUpdateData Data { get; set; } = new AccountUpdateData();
    }

    public class AccountUpdateData
    {
        [JsonProperty("m")]
        public string Reason { get; set; } = string.Empty;

        [JsonProperty("B")]
        public IReadOnlyCollection<AccountBalance> Balances { get; set; } = Array.Empty<AccountBalance>();

        [JsonProperty("P")]
        public IReadOnlyCollection<AccountPosition> Positions { get; set; } = Array.Empty<AccountPosition>();
    }

    public class AccountBalance
    {
        [JsonProperty("a")]
        public string Asset { get; set; } = string.Empty;

        [JsonProperty("wb")]
        public string WalletBalance { get; set; } = string.Empty;

        [JsonProperty("cw")]
        public string CrossWalletBalance { get; set; } = string.Empty;

        [JsonProperty("bc")]
        public string BalanceChange { get; set; } = string.Empty;
    }

    public class AccountPosition
    {
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        [JsonProperty("pa")]
        public string PositionAmount { get; set; } = string.Empty;

        [JsonProperty("ep")]
        public string EntryPrice { get; set; } = string.Empty;

        [JsonProperty("cr")]
        public string AccumulatedRealized { get; set; } = string.Empty;

        [JsonProperty("up")]
        public string UnrealizedPnL { get; set; } = string.Empty;

        [JsonProperty("mt")]
        public string MarginType { get; set; } = string.Empty;

        [JsonProperty("iw")]
        public string IsolatedWallet { get; set; } = string.Empty;

        [JsonProperty("ps")]
        public string PositionSide { get; set; } = string.Empty;
    }
}
