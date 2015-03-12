﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using Newtonsoft.Json;

namespace RitoConnector
{
    internal abstract class Items
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("sanitizedDescription")]
        public string SanitizedDescription { get; set; }

        [JsonProperty("plaintext")]
        public string Plaintext { get; set; }

        [JsonProperty("into")]
        public string[] Into { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("stats")]
        public ItemStats Stats { get; set; }

        [JsonProperty("gold")]
        public Gold Gold { get; set; }
    }
}