using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RitoConnector
{
    class RankedHandler
    {
        private RankedDTO rankedStatus;
        public RankedHandler(int userid, string Region, string key)
        {
            string JSONRAW;
            WebResponse Response;
            string URI = "https://" + Region.ToLower() + ".api.pvp.net/api/lol/" + Region.ToLower() + "/v2.5/league/by-summoner/" + userid + "?api_key=" + key;
            WebRequest ConnectionListener = WebRequest.Create(URI);
            ConnectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                Response = ConnectionListener.GetResponse();
            }
            catch(WebException e){
                   System.Windows.MessageBox.Show(e.Message);
                   Response = null;
                   rankedStatus = null;
                   return;
            }
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }
            rankedStatus = (RankedDTO)JsonConvert.DeserializeObject<RankedDTO>(JSONRAW);
        }
        public string getRankedTier()
        {
           return rankedStatus.RankedID.FirstOrDefault().Tier;
        }
    }
}
