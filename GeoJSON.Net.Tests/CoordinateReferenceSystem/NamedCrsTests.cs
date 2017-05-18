using System;
using GeoJSON.Net.CoordinateReferenceSystem;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GeoJSON.Net.Tests.CoordinateReferenceSystem
{
    [TestFixture]
    public class NamedCRSTests : TestBase
    {
        [Test]
        public void Has_Correct_Type()
        {
            var name = "EPSG:31370";
            var crs = new NamedCRS(name);

            Assert.AreEqual(CRSType.Name, crs.Type);
        }

        [Test]
        public void Has_Name_Property_With_Name()
        {
            var name = "EPSG:31370";
            var crs = new NamedCRS(name);

            Assert.IsTrue(crs.Properties.ContainsKey("name"));
            Assert.AreEqual(name, crs.Properties["name"]);
        }

        [Test]
        public void Can_Serialize()
        {
            var collection = new FeatureCollection<IFeature> { CRS = new NamedCRS("EPSG:31370") };
            var actualJson = JsonConvert.SerializeObject(collection);

            JsonAssert.Contains("{\"properties\":{\"name\":\"EPSG:31370\"},\"type\":\"name\"}", actualJson);
        }

        [Test]
        public void Ctor_Throws_ArgumentNullExpection_When_Name_Is_Null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new FeatureCollection<IFeature> { CRS = new NamedCRS(null) }; });
        }

        [Test]
        public void Ctor_Throws_ArgumentNullExpection_When_Name_Is_Empty()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => { new FeatureCollection<IFeature>{ CRS = new NamedCRS(string.Empty) }; });
        }
    }
}