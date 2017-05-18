// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiLineString.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2016
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#multilinestring">MultiLineString</see> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry
{
    /// <summary>
    ///     Defines the <see cref="http://geojson.org/geojson-spec.html#multilinestring">MultiLineString</see> type.
    /// </summary>
    public class MultiLineString : GeoJSONObject, IGeometryObject, IEquatable<MultiLineString>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiLineString" /> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public MultiLineString(IEnumerable<LineString> coordinates)
            : this(coordinates.Select(l => l.Coordinates))
        {
            
        }

        [JsonConstructor]
        public MultiLineString(IEnumerable<IEnumerable<GeographicPosition>> coordinates)
        {
            Coordinates = coordinates.Select(s => s.ToArray()).ToArray();
        }

        public override GeoJSONObjectType Type => GeoJSONObjectType.MultiLineString;

        /// <summary>
        ///     Gets the Coordinates.
        /// </summary>
        /// <value>The Coordinates.</value>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        public IReadOnlyList<IReadOnlyList<GeographicPosition>> Coordinates { get; }

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

            return Equals((MultiLineString)obj);
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode();
        }

        public static bool operator ==(MultiLineString left, MultiLineString right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MultiLineString left, MultiLineString right)
        {
            return !Equals(left, right);
        }

        public bool Equals(MultiLineString other)
        {
            return base.Equals(other) && Coordinates.SequenceEqual(other.Coordinates, PositonArrayComparer.Instance);
        }
    }
}