// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeographicPosition.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2016
// </copyright>
// <summary>
//   Defines the Geographic Position type a.k.a. <see cref="http://geojson.org/geojson-spec.html#positions">Geographic Coordinate Reference System</see>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry
{
    /// <summary>
    ///     Defines the Geographic Position type a.k.a.
    ///     <see cref="http://geojson.org/geojson-spec.html#positions">Geographic Coordinate Reference System</see>.
    /// </summary>
    [JsonConverter(typeof(GeographicPositionConverter))]
    public class GeographicPosition : IEquatable<GeographicPosition>
    {
        private static readonly DoubleTenDecimalPlaceComparer DoubleComparer = new DoubleTenDecimalPlaceComparer();

        /// <summary>
        ///     Initializes a new instance of the <see cref="GeographicPosition" /> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="altitude">The altitude in m(eter).</param>
        public GeographicPosition(double latitude, double longitude, double? altitude = null)
        {
            Coordinates = altitude.HasValue
                ? new[] {longitude, latitude, altitude.Value}
                : new[] {longitude, latitude};
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GeographicPosition" /> class.
        /// </summary>
        /// <param name="latitude">The latitude, e.g. '38.889722'.</param>
        /// <param name="longitude">The longitude, e.g. '-77.008889'.</param>
        /// <param name="altitude">The altitude in m(eters).</param>
        public static GeographicPosition Parse(string latitude, string longitude, string altitude = null)
        {
            if (latitude == null)
            {
                throw new ArgumentNullException(nameof(latitude));
            }

            if (longitude == null)
            {
                throw new ArgumentNullException(nameof(longitude));
            }

            if (string.IsNullOrWhiteSpace(latitude))
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "May not be empty.");
            }

            if (string.IsNullOrWhiteSpace(longitude))
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "May not be empty.");
            }


            if (!double.TryParse(latitude, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat) || Math.Abs(lat) > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be a proper lat (+/- double) value between -90 and 90.");
            }

            if (!double.TryParse(longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon) || Math.Abs(lon) > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be a proper lon (+/- double) value between -180 and 180.");
            }

            

            if (altitude != null)
            {
                if (!double.TryParse(altitude, NumberStyles.Float, CultureInfo.InvariantCulture, out double alt))
                {
                    throw new ArgumentOutOfRangeException(nameof(altitude), "Altitude must be a proper altitude (m(eter) as double) value, e.g. '6500'.");
                }
                return new GeographicPosition(lat, lon, alt);
            }
            return new GeographicPosition(lat, lon);
        }

        /// <summary>
        ///     Gets the altitude.
        /// </summary>
        public double? Altitude => Coordinates.Length > 2 ? (double?)Coordinates[2] : null;

        /// <summary>
        ///     Gets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude => Coordinates[1];

        /// <summary>
        ///     Gets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude => Coordinates[0];

        /// <summary>
        ///     Gets or sets the coordinates, is a 2-size array
        /// </summary>
        /// <value>
        ///     The coordinates.
        /// </value>
        internal double[] Coordinates { get; }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
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

            return obj.GetType() == GetType() && Equals((GeographicPosition)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Coordinates != null ? Coordinates.GetHashCode() : 0;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(GeographicPosition left, GeographicPosition right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(GeographicPosition left, GeographicPosition right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Altitude == null
                ? string.Format(CultureInfo.InvariantCulture, "Latitude: {0}, Longitude: {1}", Latitude, Longitude)
                : string.Format(CultureInfo.InvariantCulture, "Latitude: {0}, Longitude: {1}, Altitude: {2}", Latitude, Longitude, Altitude);
        }

        /// <summary>
        /// Determines whether the specified <see cref="GeographicPosition" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="GeographicPosition" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="GeographicPosition" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(GeographicPosition other)
        {
            return Coordinates.SequenceEqual(other.Coordinates, DoubleComparer);
        }

        public bool StrictlyEquals(GeographicPosition other)
        {
            return Coordinates.SequenceEqual(other.Coordinates);
        }
    }
}