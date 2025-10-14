using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aster.Client.Websocket.Json
{
    /// <summary>
    /// Aster preconfigured JSON serializer
    /// </summary>
    public static class AsterJsonSerializer
    {
        /// <summary>
        /// JSON settings
        /// </summary>
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new List<JsonConverter>
            {
                new AsterStringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy()},
            }
        };

        /// <summary>
        /// Serializer instance
        /// </summary>
        public static readonly JsonSerializer Serializer = JsonSerializer.Create(Settings);

        /// <summary>
        /// Deserialize string into object
        /// </summary>
        public static T? Deserialize<T>(string data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(data, Settings);
        }

        /// <summary>
        /// Serialize object into JSON string
        /// </summary>
        public static string Serialize(object? data)
        {
            return JsonConvert.SerializeObject(data, Settings);
        }
    }
}
