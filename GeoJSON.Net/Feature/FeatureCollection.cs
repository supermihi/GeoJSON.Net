// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeatureCollection.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2016
// </copyright>
// <summary>
//   Defines the FeatureCollection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GeoJSON.Net.Feature
{
    /// <summary>
    ///     Defines the FeatureCollection type.
    /// </summary>
    public class FeatureCollection<T> : GeoJSONObject where T : IFeature
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FeatureCollection{T}" /> class.
        /// </summary>
        public FeatureCollection() : this(new List<T>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FeatureCollection{T}" /> class.
        /// </summary>
        /// <param name="features">The features.</param>
        public FeatureCollection(IReadOnlyList<T> features)
        {
            Features = features?.ToList() ?? throw new ArgumentNullException(nameof(features));
        }

        public override GeoJSONObjectType Type => GeoJSONObjectType.FeatureCollection;

        /// <summary>
        ///     Gets the features.
        /// </summary>
        /// <value>The features.</value>
        [JsonProperty(PropertyName = "features", Required = Required.Always)]
        public IList<T> Features { get; private set; }
    }
}