
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RitoConnector
{
    class MultiIDclass
    {
         [JsonProperty("user")]
        public User user { get; set; }
    }
}
