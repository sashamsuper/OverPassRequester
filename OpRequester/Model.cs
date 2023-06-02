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


using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OverPassRequester
{
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
				public IDictionary<string,object>? ExtensionTags {set;get;}
    }



}
