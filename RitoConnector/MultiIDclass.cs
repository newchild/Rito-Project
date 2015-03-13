
using Newtonsoft.Json;

namespace RitoConnector
{
    class MultiIDclass
    {
         [JsonProperty("user")]
        public User User { get; set; }
    }
}
