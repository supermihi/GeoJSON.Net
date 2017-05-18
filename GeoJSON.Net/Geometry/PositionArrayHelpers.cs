using System.Collections.Generic;
using System.Linq;

namespace GeoJSON.Net.Geometry
{
    public class PositionArrayArrayComparer : IEqualityComparer<IReadOnlyList<IReadOnlyList<GeographicPosition>>>
    {
        public static PositionArrayArrayComparer Instance { get; } = new PositionArrayArrayComparer();
        public bool Equals(IReadOnlyList<IReadOnlyList<GeographicPosition>> x, IReadOnlyList<IReadOnlyList<GeographicPosition>> y)
        {
            return x.SequenceEqual(y, PositonArrayComparer.Instance);
        }

        public int GetHashCode(IReadOnlyList<IReadOnlyList<GeographicPosition>> list)
        {
            unchecked
            {
                return list.Aggregate(19, (current, pos) => current * 31 + PositonArrayComparer.Instance.GetHashCode(pos));
            }
        }
    }
    public class PositonArrayComparer : IEqualityComparer<IReadOnlyList<GeographicPosition>>
    {
        public static PositonArrayComparer Instance { get; } = new PositonArrayComparer();
        public bool Equals(IReadOnlyList<GeographicPosition> x, IReadOnlyList<GeographicPosition> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(IReadOnlyList<GeographicPosition> list)
        {
            unchecked
            {
                return list.Aggregate(19, (current, pos) => current * 31 + pos.GetHashCode());
            }
        }
    }
    public static class PositionArrayHelpers
    {

        /// <summary>
        ///     Determines whether this instance has its first and last coordinate at the same position and thereby is closed.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is closed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsClosed(this IReadOnlyList<GeographicPosition> array)
        {
            return array.First().StrictlyEquals(array.Last());
        }

        /// <summary>
        ///     Determines whether this position array is a
        ///     <see cref="http://geojson.org/geojson-spec.html#linestring">LinearRing</see>.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if it is a linear ring; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLinearRing(this IReadOnlyList<GeographicPosition> array)
        {
            return array.Count >= 4 && array.IsClosed();
        }
    }
}