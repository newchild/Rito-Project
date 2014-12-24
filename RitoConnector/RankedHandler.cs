using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RitoConnector
{
    class RankedHandler
    {
        private int userids;
        private RankedDTO rankedStatus;
        public RankedHandler(int userid, string Region, string key)
        {
            userids = userid;
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
            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(JSONRAW);
            JSONRAW = tempjson[userid.ToString()].ToString();
            JSONRAW = "{ \"standard\" : " + JSONRAW + "}" ;
            rankedStatus = JsonConvert.DeserializeObject<RankedDTO>(JSONRAW);
        }
        public string getRankedSoloLeague()
        {
            foreach (RankedID rank in rankedStatus.RankedID)
            {
                if (rank.Queue == "RANKED_SOLO_5x5")
                {
                    foreach (Entry person in rank.Entries)
                    {
                        if (Convert.ToInt32(person.PlayerOrTeamId) == userids)
                        {
                            return person.Division;
                        }
                    }
                }
            }
            return "unkown";
        }
        public string getRankedSoloTier()
        {
            foreach (RankedID rank in rankedStatus.RankedID)
            {
                
                if (rank.Queue == "RANKED_SOLO_5x5")
                {
                    return rank.Tier;
                }
            }
            return "unranked";
        }
        public bool isValid()
        {
           
            if (rankedStatus != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Entry[] getSoloQueueLeague()
        {
            foreach (RankedID rank in rankedStatus.RankedID)
            {
                if (rank.Queue == "RANKED_SOLO_5x5")
                {
                    return rank.Entries;
                }
            }
            return null;
        }
        
    }
}
