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
		private bool isUnranked = true;
        /// <summary>
        /// sends a request to the Riotserver
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="Region"></param>
        /// <param name="key"></param>
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
				   if (e.Message.Contains("404"))
				   {
					   isUnranked = true;
				   }
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
        /// <summary>
        /// returns the current Division
        /// </summary>
        /// <returns>string</returns>
        public string getRankedSoloLeague()
        {
			if (!isUnranked)
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
			}
            return "unkown";
        }
        /// <summary>
        /// Gets the current SoloQ League
        /// </summary>
        /// <returns>string</returns>
        public string getRankedSoloTier()
        {
			if (!isUnranked)
			{
				foreach (RankedID rank in rankedStatus.RankedID)
				{
					if (rank.Queue == "RANKED_SOLO_5x5")
					{
						return rank.Tier;
					}
				}
			}
            return "unranked";
        }
        /// <summary>
        /// checks if the Connection is val
        /// </summary>
        /// <returns>bool</returns>
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
/// <summary>
/// Get the Current List of League Participants
/// </summary>
/// <returns>Entry[]</returns>
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
