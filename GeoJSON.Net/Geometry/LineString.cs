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
using GeoJSON.Net.Converters;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry
{
    /// <summary>
    ///     Defines the <see cref="http://geojson.org/geojson-spec.html#linestring">LineString</see> type.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class LineString : GeoJSONObject, IGeometryObject
    {
        [JsonConstructor]
        protected internal LineString()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
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
                    "According to the GeoJSON v1.0 spec a LineString must have at least two or more positions.");
            }
            Coordinates = coordsList;
        }
        public override GeoJSONObjectType Type => GeoJSONObjectType.LineString;

        /// <summary>
        ///     Gets the Positions.
        /// </summary>
        /// <value>The Positions.</value>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        [JsonConverter(typeof(LineStringConverter))]
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((LineString)obj);
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode();
        }

        /// <summary>
        ///     Determines whether this instance has its first and last coordinate at the same position and thereby is closed.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is closed; otherwise, <c>false</c>.
        /// </returns>
        public bool IsClosed()
        {
            return Coordinates.First().Equals(Coordinates.Last());
        }

        /// <summary>
        ///     Determines whether this LineString is a
        ///     <see cref="http://geojson.org/geojson-spec.html#linestring">LinearRing</see>.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if it is a linear ring; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLinearRing()
        {
            return Coordinates.Count >= 4 && IsClosed();
        }

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