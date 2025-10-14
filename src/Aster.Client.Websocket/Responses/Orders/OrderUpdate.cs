using System;
using System.Reactive.Subjects;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aster.Client.Websocket.Responses.Orders
{
    public class OrderUpdate : MessageBase
    {
        internal static bool TryHandle(JObject? response, ISubject<OrderUpdate> subject)
        {
            var stream = response?["e"]?.Value<string>();
            if (!string.Equals(stream, "ORDER_TRADE_UPDATE", StringComparison.OrdinalIgnoreCase))
                return false;

            var parsed = response!.ToObject<OrderUpdate>(AsterJsonSerializer.Serializer);
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
        /// Order update payload
        /// </summary>
        [JsonProperty("o")]
        public OrderUpdateData Order { get; set; } = new OrderUpdateData();
    }

    public class OrderUpdateData
    {
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        [JsonProperty("c")]
        public string ClientOrderId { get; set; } = string.Empty;

        [JsonProperty("S")]
        public OrderSide Side { get; set; }

        [JsonProperty("o")]
        public OrderType Type { get; set; }

        [JsonProperty("f")]
        public TimeInForce TimeInForce { get; set; }

        [JsonProperty("q")]
        public double Quantity { get; set; }

        [JsonProperty("p")]
        public double Price { get; set; }

        [JsonProperty("ap")]
        public double AveragePrice { get; set; }

        [JsonProperty("sp")]
        public double StopPrice { get; set; }

        [JsonProperty("x")]
        public ExecutionType ExecutionType { get; set; }

        [JsonProperty("X")]
        public OrderStatus Status { get; set; }

        [JsonProperty("i")]
        public long OrderId { get; set; }

        [JsonProperty("l")]
        public double LastQuantityFilled { get; set; }

        [JsonProperty("z")]
        public double QuantityFilled { get; set; }

        [JsonProperty("L")]
        public double LastPriceFilled { get; set; }

        [JsonProperty("N")]
        public string? CommissionAsset { get; set; }

        [JsonProperty("n")]
        public double? CommissionAmount { get; set; }

        [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? OrderTradeTime { get; set; }

        [JsonProperty("t")]
        public long TradeId { get; set; }

        [JsonProperty("b")]
        public double BidsNotional { get; set; }

        [JsonProperty("a")]
        public double AsksNotional { get; set; }

        [JsonProperty("m")]
        public bool IsMaker { get; set; }

        [JsonProperty("R")]
        public bool IsReduceOnly { get; set; }

        [JsonProperty("wt")]
        public string WorkingType { get; set; } = string.Empty;

        [JsonProperty("ot")]
        public string OriginalOrderType { get; set; } = string.Empty;

        [JsonProperty("ps")]
        public string PositionSide { get; set; } = string.Empty;

        [JsonProperty("cp")]
        public bool? CloseAll { get; set; }

        [JsonProperty("AP")]
        public double? ActivationPrice { get; set; }

        [JsonProperty("cr")]
        public double? CallbackRate { get; set; }

        [JsonProperty("rp")]
        public double? RealizedProfit { get; set; }
    }
}
