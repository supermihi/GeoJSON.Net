// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineString.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2016
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#linestring">LineString</see> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry
{
    /// <summary>
    ///     Defines the <see cref="http://geojson.org/geojson-spec.html#linestring">LineString</see> type.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class LineString : GeoJSONObject, IGeometryObject
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        [JsonConstructor]
        public LineString(IEnumerable<GeographicPosition> coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }
            var coordsList = coordinates.ToList();

            if (coordsList.Count < 2)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(coordinates), 
                    "According to the GeoJSON v1.0 spec a LineString must have at least two positions.");
            }
            Coordinates = coordsList;
        }
        public override GeoJSONObjectType Type => GeoJSONObjectType.LineString;

        /// <summary>
        ///     Gets the Positions.
        /// </summary>
        /// <value>The Positions.</value>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        public IReadOnlyList<GeographicPosition> Coordinates { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((LineString)obj);
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode();
        }

        public bool IsClosed() => Coordinates.IsClosed();

        public static bool operator ==(LineString left, LineString right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LineString left, LineString right)
        {
            return !Equals(left, right);
        }

        private bool Equals(LineString other)
        {
            return base.Equals(other) && Coordinates.SequenceEqual(other.Coordinates);
        }
    }
}