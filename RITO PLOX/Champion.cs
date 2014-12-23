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
        [JsonProperty("newchild")]
        public SummonerDTO Summoners { get; set; }
    
    }
}
