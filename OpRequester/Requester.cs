/* ==================================================================
Copyright 2023 sashamsuper

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
==========================================================================*/

//using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

//using Android.Net.Uri;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OverPassRequester
{

    public class Program
    {
        public void Main()
        {
            Uri nuri = new("https://maps.mail.ru/osm/tools/overpass/api/interpreter");
            OverPassClient over = new(nuri);
            var river = "Сена";
            var city = "Париж";
            string responseTxt = $"way['name:ru'~'Сена',i]['waterway'='river'](48.5366276064,1.89894557,49.0954664277,3.0497634411)->.river;(node(around.river:9150)['name:ru'~'Париж',i]['place'~'(city|village|town|hamlet)'];);";
            var value = over.GetJsonAsync<RootObject<Element<Tags>>>(responseTxt).Result;
            Console.WriteLine(value);
            var name = value.Elements.Select(x => x.Tags).Select(y => y.Name);
            var otherTags = value.Elements.Select(x => x.Tags).Select(y => y.ExtensionTags).First();
            Console.WriteLine(name.Count());
            Console.WriteLine(name.First());
            Console.WriteLine(otherTags["name:ru"]);
        }
    }

    public class BaseTuning
    {
        public TimeSpan ServerTimeOut { set; get; } = default;
    }

    public class OverPassClient
    {
        private static HttpClient client;
        private TimeSpan ClientTimeOut { get; set; } = TimeSpan.FromSeconds(60);
        private TimeSpan ServerTimeOut { set; get; } = default;
        public Uri BaseInterpreter { set; get; }
        public OverPassClient(Uri baseInterpreter,
                                        HttpClient clientHttp = default,
                                        TimeSpan clientTimeOut = default,
                                        TimeSpan serverTimeOut = default)
        {
            ClientTimeOut = clientTimeOut;
            ServerTimeOut = serverTimeOut;
            BaseInterpreter = baseInterpreter;
            if (clientHttp == default)
            {
                CreateHttpClient();
            }
            else
            {
                client.BaseAddress = BaseInterpreter;
            }
        }

        public virtual void CreateHttpClient()
        {
            var handler = new HttpClientHandler();
            if (ClientTimeOut == default)
            {
                ClientTimeOut = TimeSpan.FromSeconds(100);
            }
            client = new(handler)
            {
                BaseAddress = BaseInterpreter,
                Timeout = ClientTimeOut
            };
        }

        private async Task<Stream> GetResponseAsync(string request)
        {
            string requestToClient = ReturnRequestToClient(request);
            var repsonse = await client.GetStreamAsync(requestToClient);
            return repsonse;
        }

        private async Task<Stream> GetResponseAsync(XmlWriterTraceListener request)
        {
            throw new NotImplementedException("Not working");
            return default;
        }

        private string ReturnRequestToClient(string request)
        {
            var ServerTimeOutString = ServerTimeOut == default ? "" : $"[timeout:{ServerTimeOut.Seconds}]";
            if (request.Contains($"[out:json]{ServerTimeOutString}") || request.Contains("out meta"))
            {
                throw new ArgumentException("Add responseTxt without \"[out:json]\" and \"out meta\"");
            }

            return $"?data=[out:json];{request}out meta;";
        }

        public async Task<JsonDocument> GetJsonDocumentAsync(string request)
        {
            var response = await GetResponseAsync(request);
            return await Task.Run(() => JsonDocument.Parse(response));
        }
        public async Task<T> GetJsonAsync<T>(string request)
        {
            var response = await GetResponseAsync(request);
            return await JsonSerializer.DeserializeAsync<T>(response);
        }
    }
}