// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiPoint.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2016
// </copyright>
// <summary>
//   Defines the MultiPoint type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry
{
    /// <summary>
    ///     Contains an array of <see cref="Point" />s.
    /// </summary>
    /// <seealso cref="http://geojson.org/geojson-spec.html#multipoint" />
    public class MultiPoint : GeoJSONObject, IGeometryObject, IEquatable<MultiPoint>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public MultiPoint(IEnumerable<Point> coordinates)
            : this(coordinates.Select(c => c.Coordinates))
        {
        }

        [JsonConstructor]
        public MultiPoint(IEnumerable<GeographicPosition> coordinates)
        {
            Coordinates = coordinates.ToArray();
        }

        public override GeoJSONObjectType Type => GeoJSONObjectType.MultiPoint;

        /// <summary>
        ///     Gets the Coordinates.
        /// </summary>
        /// <value>The Coordinates.</value>
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
            return obj.GetType() == GetType() && Equals((MultiPoint)obj);
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode();
        }

        public static bool operator ==(MultiPoint left, MultiPoint right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MultiPoint left, MultiPoint right)
        {
            return !Equals(left, right);
        }

        public bool Equals(MultiPoint other)
        {
            return base.Equals(other) && Coordinates.SequenceEqual(other.Coordinates);
        }
    }
}