using Newtonsoft.Json;

namespace RitoConnector
{
    internal class MultiIDclass
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }
}