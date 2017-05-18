using System;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace GeoJSON.Net.Converters
{
    public class GeographicPositionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is GeographicPosition position)
            {
                serializer.Serialize(writer, position.Coordinates);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var coordinate = existingValue == null
                ? serializer.Deserialize<double[]>(reader)
                : (double[])existingValue;

            var longitude = coordinate[0];
            var latitude = coordinate[1];
            double? altitude = null;

            if (coordinate.Length >= 3)
            {
                altitude = coordinate[2];
            }

            return new GeographicPosition(latitude, longitude, altitude);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GeographicPosition);
        }
    }
}