using OverPassRequester;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestProject1
{
    public class NWR : RootObject<Element<TagsGe>>
    {
    }

    public class TagsGe
    {
        [JsonPropertyName("place")]
        public string? Place { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object>? ExtensionTags { set; get; }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestOneName()
        {
            OverPassClient over = new(new Uri("https://maps.mail.ru/osm/tools/overpass/api/interpreter"));
            var river = "Сена";
            var city = "Париж";
            string responseTxt = $"way['name:ru'~'{river}',i]['waterway'='river'](48.5366276064,1.89894557,49.0954664277,3.0497634411)->.river;(node(around.river:9150)['name:ru'~'{city}',i]['place'~'(city|village|town|hamlet)'];);";
            var value = over.GetJsonAsync<RootObject<Element<Tags>>>(responseTxt).Result;
            Debug.WriteLine(value);
            var name = value.Elements.Select(x => x.Tags).Select(y => y.Name);
            Assert.AreEqual("Paris", "Paris");
        }

        [TestMethod]
        public void TestOneNameEn()
        {
            OverPassClient over = new(new Uri("https://maps.mail.ru/osm/tools/overpass/api/interpreter"));
            var river = "Sena";
            var city = "Paris";
            string responseTxt = $"way['name:en'~'{river}',i]['waterway'='river'](48.5366276064,1.89894557,49.0954664277,3.0497634411)->.river;(node(around.river:9150)['name:en'~'{city}',i]['place'~'(city|village|town|hamlet)'];);";
            var value = over.GetJsonAsync<RootObject<Element<Tags>>>(responseTxt).Result;
            Debug.WriteLine(value);
            var name = value.Elements.Select(x => x.Tags).Select(y => y.Name);
            Assert.AreEqual("Paris", "Paris");
        }

        [TestMethod]
        public void TestMethodJsonDocument()
        {
            OverPassClient over = new(new Uri("https://maps.mail.ru/osm/tools/overpass/api/interpreter"));
            var river = "Сена";
            var city = "Париж";
            string responseTxt = $"way['name:ru'~'{river}',i]['waterway'='river'](48.5366276064,1.89894557,49.0954664277,3.0497634411)->.river;(node(around.river:9150)['name:ru'~'{city}',i]['place'~'(city|village|town|hamlet)'];);";
            var value = over.GetJsonDocumentAsync(responseTxt).Result;
            JsonElement root = value.RootElement;
            JsonElement elements = root.GetProperty("elements");
            JsonElement tags = elements[0].GetProperty("tags");
            JsonElement name = tags.GetProperty("name");
            Debug.WriteLine(name);
            Assert.AreEqual(name.ToString(), "Paris");
        }

        [TestMethod]
        public void TestMethodManualRecord()
        {
            OverPassClient over = new(new Uri("https://maps.mail.ru/osm/tools/overpass/api/interpreter"));
            var river = "Сена";
            var city = "Париж";
            string responseTxt = $"way['name:ru'~'Сена',i]['waterway'='river'](48.5366276064,1.89894557,49.0954664277,3.0497634411)->.river;(node(around.river:9150)['name:ru'~'Париж',i]['place'~'(city|village|town|hamlet)'];);";
            var value = over.GetJsonAsync<NWR>(responseTxt).Result;
            Debug.WriteLine(value);
            var name = value.Elements.Select(x => x.Tags).Select(y => y.Place).First();
            Assert.AreEqual(name.ToString(), "city");
        }

        [TestMethod]
        public void TestMethodExtension()
        {
            OverPassClient over = new(new Uri("https://overpass-api.de/api/interpreter"));
            var river = "Сена";
            var city = "Париж";
            string responseTxt = $"way['name:ru'~'Сена',i]['waterway'='river'](48.5366276064,1.89894557,49.0954664277,3.0497634411)->.river;(node(around.river:9150)['name:ru'~'Париж',i]['place'~'(city|village|town|hamlet)'];);";
            var value = over.GetJsonAsync<RootObject<Element<Tags>>>(responseTxt).Result;
            Debug.WriteLine(value);
            var name = value.Elements.Select(x => x.Tags).Select(y => y.ExtensionTags).First()["place"];
            Assert.AreEqual(name.ToString(), "city");
        }
    }
}