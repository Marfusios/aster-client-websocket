using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.UserData
{
    public class MarginCallEvent : MessageBase
    {
        internal static bool TryHandle(JObject? response, ISubject<MarginCallEvent> subject)
        {
            var stream = response?["e"]?.Value<string>();
            if (!string.Equals(stream, "MARGIN_CALL", StringComparison.OrdinalIgnoreCase))
                return false;

            var parsed = response!.ToObject<MarginCallEvent>(AsterJsonSerializer.Serializer);
            if (parsed != null)
            {
                subject.OnNext(parsed);
            }

            return true;
        }

        /// <summary>
        /// Cross wallet balance (only for crossed positions)
        /// </summary>
        [JsonProperty("cw")]
        public string? CrossWalletBalance { get; set; }

        /// <summary>
        /// Positions under margin call
        /// </summary>
        [JsonProperty("p")]
        public IReadOnlyCollection<MarginCallPosition> Positions { get; set; } = Array.Empty<MarginCallPosition>();
    }

    public class MarginCallPosition
    {
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        [JsonProperty("ps")]
        public string PositionSide { get; set; } = string.Empty;

        [JsonProperty("pa")]
        public string PositionAmount { get; set; } = string.Empty;

        [JsonProperty("mt")]
        public string MarginType { get; set; } = string.Empty;

        [JsonProperty("iw")]
        public string IsolatedWallet { get; set; } = string.Empty;

        [JsonProperty("mp")]
        public string MarkPrice { get; set; } = string.Empty;

        [JsonProperty("up")]
        public string UnrealizedPnL { get; set; } = string.Empty;

        [JsonProperty("mm")]
        public string MaintenanceMargin { get; set; } = string.Empty;
    }
}
