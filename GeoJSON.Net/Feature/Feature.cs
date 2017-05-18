// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Feature.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2016
// </copyright>
// <summary>
//   Defines the Feature type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace GeoJSON.Net.Feature
{
    public static class Feature
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Feature" /> class.
        /// </summary>
        /// <param name="geometry">The Geometry Object.</param>
        /// <param name="properties">
        ///     Class used to fill feature properties. Any public member will be added to feature
        ///     properties
        /// </param>
        /// <param name="id">The (optional) identifier.</param>
        public static Feature<TGeometry> Generic<TGeometry>(TGeometry geometry, object properties = null, string id = null)
            where TGeometry : IGeometryObject
        {
            var props = properties == null
            ? new Dictionary<string, object>()
            : properties.GetType().GetTypeInfo().DeclaredProperties
                    .Where(propertyInfo => propertyInfo.GetMethod.IsPublic)
                    .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(properties, null));
            return new Feature<TGeometry>(geometry, props, id);
        }

        public static Feature<TGeo, TProp> Create<TGeo, TProp>(TGeo geometry, TProp props, string id = null)
            where TGeo : IGeometryObject
        {
            return new Feature<TGeo, TProp>(geometry, props);
        }
    }

    public interface IFeature
    {
        IGeometryObject Geometry { get; }
        
    }
    public interface IFeature<out TGeometry> : IFeature where TGeometry : IGeometryObject
    {
        new TGeometry Geometry { get; }
    }

    /// <summary>
    ///     A GeoJSON <see cref="http://geojson.org/geojson-spec.html#feature-objects">Feature Object</see>.
    /// </summary>
    public class Feature<TGeometry, TProp> : GeoJSONObject, IFeature<TGeometry>
        where TGeometry : IGeometryObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Feature" /> class.
        /// </summary>
        /// <param name="geometry">The Geometry Object.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="id">The (optional) identifier.</param>
        [JsonConstructor]
        public Feature(TGeometry geometry, TProp properties, string id = null)
        {
            Geometry = geometry;
            Properties = properties;
            Id = id;
        }
        

        public override GeoJSONObjectType Type => GeoJSONObjectType.Feature;

        /// <summary>
        ///     Gets or sets the geometry.
        /// </summary>
        /// <value>
        ///     The geometry.
        /// </value>
        [JsonProperty(PropertyName = "geometry", Required = Required.AllowNull)]
        [JsonConverter(typeof(GeometryConverter))]
        public TGeometry Geometry { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The handle.</value>
        [JsonProperty(PropertyName = "id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        ///     Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        [JsonProperty(PropertyName = "properties", Required = Required.AllowNull)]
        public TProp Properties { get; }

        [JsonIgnore]
        IGeometryObject IFeature.Geometry => Geometry;
    }

    public class Feature<TGeo> : Feature<TGeo, IDictionary<string, object>> where TGeo : IGeometryObject
    {
        public Feature(TGeo geometry, IDictionary<string, object> properties = null, string id = null)
            : base(geometry, properties ?? new Dictionary<string, object>(), id)
        {
        }
    }
}