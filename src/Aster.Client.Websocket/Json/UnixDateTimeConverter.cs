using System;
using System.Globalization;
using Aster.Client.Websocket.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Aster.Client.Websocket.Json
{
    /// <summary>
    /// Converter between unix date time (milliseconds as long type) and DateTime
    /// </summary>
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        /// <summary>
        /// Serialize DateTime into Unix milliseconds
        /// </summary>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null) { return; }
            var subtracted = ((DateTime)value).Subtract(AsterTime.UnixBase);
            writer.WriteRawValue(subtracted.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Deserialize Unix milliseconds into DateTime
        /// </summary>
        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return AsterTime.ConvertToTime((long)reader.Value);
        }
    }
}
