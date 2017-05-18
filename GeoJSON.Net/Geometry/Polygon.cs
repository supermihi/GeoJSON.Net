// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polygon.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2016
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#polygon">Polygon</see> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry
{
    /// <summary>
    ///     Defines the <see cref="http://geojson.org/geojson-spec.html#polygon">Polygon</see> type.
    ///     Coordinates of a Polygon are a list of
    ///     <see cref="http://geojson.org/geojson-spec.html#linestring">linear rings</see>
    ///     coordinate arrays. The first element in the array represents the exterior ring. Any subsequent elements
    ///     represent interior rings (or holes).
    /// </summary>
    /// <seealso cref="http://geojson.org/geojson-spec.html#polygon" />
    public class Polygon : GeoJSONObject, IGeometryObject, IEquatable<Polygon>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="coordinates">
        ///     The <see cref="http://geojson.org/geojson-spec.html#linestring">linear rings</see> with the first element
        ///     in the array representing the exterior ring. Any subsequent elements represent interior rings (or holes).
        /// </param>
        public Polygon(IReadOnlyCollection<LineString> coordinates) : this(coordinates.Select(c => c.Coordinates))
        {
        }

        [JsonConstructor]
        public Polygon(IEnumerable<IEnumerable<GeographicPosition>> coordinates)
        {
            Coordinates = coordinates?.Select(l => l.ToArray()).ToArray()
                ?? throw new ArgumentNullException(nameof(coordinates));
            

            if (Coordinates.Any(linearRing => !linearRing.IsLinearRing()))
            {
                throw new ArgumentException("All elements must be closed LineStrings with 4 or more positions" +
                                            " (see GeoJSON spec at 'http://geojson.org/geojson-spec.html#linestring').", nameof(coordinates));
            }
            
        }

        public override GeoJSONObjectType Type => GeoJSONObjectType.Polygon;

        /// <summary>
        ///     Gets the list of points outlining this Polygon.
        /// </summary>
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
            return obj.GetType() == GetType() && Equals((Polygon)obj);
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode();
        }

        public static bool operator ==(Polygon left, Polygon right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Polygon left, Polygon right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Polygon other)
        {
            return base.Equals(other) && Coordinates.SequenceEqual(other.Coordinates, PositonArrayComparer.Instance);
        }
    }
}