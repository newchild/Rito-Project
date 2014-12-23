using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RITO_PLOX
{
    class Champion
    {
        static string username;
        public Champion(string Nick)
        {
            username = Nick;
        }
        [JsonProperty(username)]
        public SummonerDTO Summoner { get; set; }
    
    }
}
