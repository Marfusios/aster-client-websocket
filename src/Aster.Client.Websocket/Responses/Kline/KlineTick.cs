using Newtonsoft.Json;

namespace Aster.Client.Websocket.Responses.Kline
{
    /// <summary>
    /// Represents the nested <c>k</c> object in the kline stream payload.
    /// </summary>
    public class KlineTick
    {
        [JsonProperty("t")]
        public double StartTime { get; set; }

        [JsonProperty("T")]
        public double CloseTime { get; set; }

        [JsonProperty("s")]
        public string? Symbol { get; set; }

        [JsonProperty("i")]
        public string? Interval { get; set; }

        [JsonProperty("f")]
        public double FirstTradeId { get; set; }

        [JsonProperty("L")]
        public double LastTradeId { get; set; }

        [JsonProperty("o")]
        public double OpenPrice { get; set; }

        [JsonProperty("c")]
        public double ClosePrice { get; set; }

        [JsonProperty("h")]
        public double HighPrice { get; set; }

        [JsonProperty("l")]
        public double LowPrice { get; set; }

        [JsonProperty("v")]
        public double BaseAssetVolume { get; set; }

        [JsonProperty("n")]
        public double NumberTrades { get; set; }

        [JsonProperty("x")]
        public bool IsClose { get; set; }

        [JsonProperty("q")]
        public double QuoteAssetVolume { get; set; }

        [JsonProperty("V")]
        public double TakerBuyBaseAssetVolume { get; set; }

        [JsonProperty("Q")]
        public double TakerBuyQuoteAssetVolume { get; set; }

        [JsonProperty("B")]
        public double Ignore { get; set; }
    }
}
