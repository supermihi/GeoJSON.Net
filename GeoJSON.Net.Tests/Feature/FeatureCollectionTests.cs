using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GeoJSON.Net.Tests.Feature
{
    [TestFixture]
    public class FeatureCollectionTests : TestBase
    {
        [Test]
        public void Ctor_Throws_ArgumentNullException_When_Features_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new FeatureCollection<IFeature>(null); });
        }

        [Test]
        public void Can_Deserialize()
        {
            var json = GetExpectedJson();

            var featureCollection = JsonConvert.DeserializeObject<FeatureCollection<Feature<IGeometryObject>>>(json);

            Assert.IsNotNull(featureCollection.Features);
            Assert.AreEqual(featureCollection.Features.Count, 3);
            Assert.AreEqual(featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Point), 1);
            Assert.AreEqual(featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.MultiPolygon), 1);
            Assert.AreEqual(featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Polygon), 1);
        }

        [Test]
        public void FeatureCollectionSerialization()
        {
            var model = new FeatureCollection<Feature<LineString>>();
            for (var i = 10; i-- > 0;)
            {
                var geom = new LineString(new[]
                {
                    new GeographicPosition(51.010, -1.034), 
                    new GeographicPosition(51.010, -0.034)
                });

                var props = new Dictionary<string, object>
                {
                    { "test1", "1" }, 
                    { "test2", 2 }
                };

                var feature = new Feature<LineString>(geom, props);
                model.Features.Add(feature);
            }

            var actualJson = JsonConvert.SerializeObject(model);

            Assert.IsNotNull(actualJson);

            Assert.IsFalse(string.IsNullOrEmpty(actualJson));
        }
    }
}