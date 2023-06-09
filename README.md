©sashamsuper, 2023

OverPassRequester

Library requests to overpass turbo on c sharp.

Now work only with overpass Ql.

Example.

    using OverPassRequester;
    
    OverPassClient over = new(new Uri("https://maps.mail.ru/osm/tools/overpass/api/interpreter"));
    var river = "La Seine";
    var city = "Paris";
    string responseTxt = $"way['name:fr'~'{river}',i]['waterway'='river'](48.5366276064,1.89894557,49.0954664277,3.0497634411)->.river;(node(around.river:9150)['name:fr'~'{city}',i]['place'~'(city|village|town|hamlet)'];);";
    var value = over.GetJsonAsync<RootObject<Element<Tags>>>(responseTxt).Result;
    var name = value?.Elements.FirstOrDefault()?.Tags?.Name;
    Assert.AreEqual(name, "Paris");
    Console.WriteLine(name);


The default data model received in response in the library

    public class Element<TTags>
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("id")]
        public object? Id { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("changeset")]
        public int Changeset { get; set; }

        [JsonPropertyName("user")]
        public string? User { get; set; }

        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("tags")]
        public TTags? Tags { get; set; }
    }

    public class Osm3s
    {
        [JsonPropertyName("timestamp_osm_base")]
        public DateTime TimestampOsmBase { get; set; }

        [JsonPropertyName("copyright")]
        public string? Copyright { get; set; }
    }

    public class RootObject<TElement>
    {
        [JsonPropertyName("version")]
        public double Version { get; set; }

        [JsonPropertyName("generator")]
        public string? Generator { get; set; }

        [JsonPropertyName("osm3s")]
        public Osm3s? Osm3s { get; set; }

        [JsonPropertyName("elements")]
        public List<TElement>? Elements { get; set; }
    }

    public class Tags
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object>? ExtensionTags { set; get; }
    }

OverPass returns 
    RootObject\<Element\<Tags\>\>
    

You can change over.GetJsonAsync<T> or get ExtensionTags by Dictionary

https://github.com/sashamsuper/OverPassRequester
  

![example workflow](https://github.com/sashamsuper/OverPassRequester/actions/workflows/dotnet.yml/badge.svg)
