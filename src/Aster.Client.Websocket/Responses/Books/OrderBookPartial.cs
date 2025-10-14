using Newtonsoft.Json;

namespace Aster.Client.Websocket.Responses.Books
{
    /// <summary>
    /// Partial order book
    /// </summary>
    public class OrderBookPartial
    {
        /// <summary>
        /// The symbol the update is for
        /// </summary>
        [JsonProperty("s")]
        public string? Symbol { get; set; }

        /// <summary>
        /// The ID of the last update
        /// </summary>
        [JsonProperty("lastUpdateId")]
        public long LastUpdateId { get; set; }

        /// <summary>
        /// Bid levels
        /// </summary>
        [JsonProperty("b")]
        public OrderBookLevel[]? Bids { get; set; }

        /// <summary>
        /// Asks levels
        /// </summary>
        [JsonProperty("a")]
        public OrderBookLevel[]? Asks { get; set; }
    }
}
