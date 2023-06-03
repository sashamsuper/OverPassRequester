©sashamsuper, 2023

OverPassRequester

Library for suggestions for the overpass turbo.

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
