©sashamsuper, 2023

OverPassRequester

Library for suggestions for the overpass turbo.

Now work only with overpass QL.

Example.

OverPassClient over = new(new Uri("https://overpass-api.de/api/interpreter"));
var river = "Sena";
var city = "Paris";
string responseTxt = $"way['name:en'~'{river}',i]['waterway'='river'](48.5366276064,1.89894557,49.0954664277,3.0497634411)->.river;(node(around.river:9150)['name:en'~'{city}',i]['place'~'(city|village|town|hamlet)'];);";
var value = over.GetJsonAsync<RootObject<Element<Tags>>>(responseTxt).Result;
Debug.WriteLine(value);
var name = value.Elements.Select(x => x.Tags).Select(y => y.Name);


