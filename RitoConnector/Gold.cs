﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using Newtonsoft.Json;

namespace RitoConnector
{
    internal class Gold
    {
        [JsonProperty("base")]
        public int Base { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("sell")]
        public int Sell { get; set; }

        [JsonProperty("purchasable")]
        public bool Purchasable { get; set; }
    }
}