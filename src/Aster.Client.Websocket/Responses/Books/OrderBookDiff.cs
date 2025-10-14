using System;
using Aster.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Aster.Client.Websocket.Responses.Books
{
    /// <summary>
    /// Order book difference
    /// </summary>
    public class OrderBookDiff : MessageBase
    {
        /// <summary>
        /// The symbol the update is for
        /// </summary>
        [JsonProperty("s")]
        public string? Symbol { get; set; }

        /// <summary>
        /// The ID of the last update (final)
        /// </summary>
        [JsonProperty("u")]
        public long LastUpdateId { get; set; }

        /// <summary>
        /// The id of this update, can be synced with REST API to update the order book
        /// </summary>
        [JsonProperty("U")]
        public long FirstUpdateId { get; set; }

        /// <summary>
        /// The final update id from the previous event
        /// </summary>
        [JsonProperty("pu")]
        public long PreviousFinalUpdateId { get; set; }

        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// The list of bids
        /// </summary>
        [JsonProperty("b")]
        public OrderBookLevel[]? Bids { get; set; }

        /// <summary>
        /// The list of asks
        /// </summary>
        [JsonProperty("a")]
        public OrderBookLevel[]? Asks { get; set; }
    }
}
