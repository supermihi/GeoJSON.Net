using GeoJSON.Net.CoordinateReferenceSystem;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GeoJSON.Net.Tests.CoordinateReferenceSystem
{
    [TestFixture]
    public class UnspecifiedCRSTests : TestBase
    {
        [Test]
        public void Has_Correct_Type()
        {
            var crs = new UnspecifiedCRS();

            Assert.AreEqual(CRSType.Unspecified, crs.Type);
        }

        [Test]
        public void Can_Serialize_To_Null()
        {
            var collection = new FeatureCollection<IFeature> { CRS = new UnspecifiedCRS() };
            var expectedJson = "{\"type\":\"FeatureCollection\",\"crs\":null,\"features\":[] }";
            var actualJson = JsonConvert.SerializeObject(collection);
            
            JsonAssert.AreEqual(expectedJson, actualJson);
        }

        [Test]
        public void Can_Deserialize_From_Null()
        {
            var json = "{\"type\":\"FeatureCollection\",\"crs\":null,\"features\":[] }";
            var featureCollection = JsonConvert.DeserializeObject<FeatureCollection<IFeature>>(json);

            Assert.IsInstanceOf<UnspecifiedCRS>(featureCollection.CRS);
        }
    }
}