using Newtonsoft.Json;

namespace Aster.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// Self trade prevention mode
    /// </summary>
    public enum SelfTradePreventionMode
    {
        /// <summary>
        /// None
        /// </summary>
        [JsonProperty("NONE")]
        None,
        
        /// <summary>
        /// Expire taker
        /// </summary>
        [JsonProperty("EXPIRE_TAKER")]
        ExpireTaker,
        
        /// <summary>
        /// Exire maker
        /// </summary>
        [JsonProperty("EXPIRE_MAKER")]
        ExpireMaker,
        
        /// <summary>
        /// Exire both
        /// </summary>
        [JsonProperty("EXPIRE_BOTH")]
        ExpireBoth, }
}