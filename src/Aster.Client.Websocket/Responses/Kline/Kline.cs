using Newtonsoft.Json;

namespace Aster.Client.Websocket.Responses.Kline
{
    /// <summary>
    /// The current klines/candlestick event payload.
    /// </summary>
    public class Kline : MessageBase
    {
        /// <summary>
        /// Symbol the event refers to (top-level <c>s</c> field).
        /// </summary>
        [JsonProperty("s")]
        public string? Symbol { get; set; }

        /// <summary>
        /// Raw kline tick details as delivered within the nested <c>k</c> object.
        /// </summary>
        [JsonProperty("k")]
        public KlineTick Tick { get; set; } = new KlineTick();

        [JsonIgnore]
        public double StartTime
        {
            get => Tick.StartTime;
            set => Tick.StartTime = value;
        }

        [JsonIgnore]
        public double CloseTime
        {
            get => Tick.CloseTime;
            set => Tick.CloseTime = value;
        }

        [JsonIgnore]
        public string? Interval
        {
            get => Tick.Interval;
            set => Tick.Interval = value;
        }

        [JsonIgnore]
        public double FirstTradeId
        {
            get => Tick.FirstTradeId;
            set => Tick.FirstTradeId = value;
        }

        [JsonIgnore]
        public double LastTradeId
        {
            get => Tick.LastTradeId;
            set => Tick.LastTradeId = value;
        }

        [JsonIgnore]
        public double OpenPrice
        {
            get => Tick.OpenPrice;
            set => Tick.OpenPrice = value;
        }

        [JsonIgnore]
        public double ClosePrice
        {
            get => Tick.ClosePrice;
            set => Tick.ClosePrice = value;
        }

        [JsonIgnore]
        public double HighPrice
        {
            get => Tick.HighPrice;
            set => Tick.HighPrice = value;
        }

        [JsonIgnore]
        public double LowPrice
        {
            get => Tick.LowPrice;
            set => Tick.LowPrice = value;
        }

        [JsonIgnore]
        public double BaseAssetVolume
        {
            get => Tick.BaseAssetVolume;
            set => Tick.BaseAssetVolume = value;
        }

        [JsonIgnore]
        public double NumberTrades
        {
            get => Tick.NumberTrades;
            set => Tick.NumberTrades = value;
        }

        [JsonIgnore]
        public bool IsClose
        {
            get => Tick.IsClose;
            set => Tick.IsClose = value;
        }

        [JsonIgnore]
        public double QuoteAssetVolume
        {
            get => Tick.QuoteAssetVolume;
            set => Tick.QuoteAssetVolume = value;
        }

        [JsonIgnore]
        public double TakerBuyBaseAssetVolume
        {
            get => Tick.TakerBuyBaseAssetVolume;
            set => Tick.TakerBuyBaseAssetVolume = value;
        }

        [JsonIgnore]
        public double TakerBuyQuoteAssetVolume
        {
            get => Tick.TakerBuyQuoteAssetVolume;
            set => Tick.TakerBuyQuoteAssetVolume = value;
        }

        [JsonIgnore]
        public double Ignore
        {
            get => Tick.Ignore;
            set => Tick.Ignore = value;
        }
    }
}
