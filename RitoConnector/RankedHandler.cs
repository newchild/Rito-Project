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
        private readonly int _userids;
        private readonly RankedDto _rankedStatus;
        /// <summary>
        /// sends a request to the Riotserver
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="region"></param>
        /// <param name="key"></param>
        public RankedHandler(int userid, string region, string key)
        {
            _userids = userid;
            string jsonraw;
            WebResponse response;
            string uri = "https://" + region.ToLower() + ".api.pvp.net/api/lol/" + region.ToLower() + "/v2.5/league/by-summoner/" + userid + "?api_key=" + key;
            WebRequest connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch(WebException e){
                   
                   response = null;
                   _rankedStatus = null;
				   if (e.Message.Contains("404"))
				   {
                       _rankedStatus = null;
				   }
                   else
                   {
                       System.Windows.MessageBox.Show(e.Message);
                   }
                   return;
            }
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }
            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonraw);
            jsonraw = tempjson[userid.ToString()].ToString();
            jsonraw = "{ \"standard\" : " + jsonraw + "}" ;
            _rankedStatus = JsonConvert.DeserializeObject<RankedDto>(jsonraw);
        }
        /// <summary>
        /// returns the current Division
        /// </summary>
        /// <returns>string</returns>
        public string GetRankedSoloDivision()
        {
			
				foreach (RankedId rank in _rankedStatus.RankedId)
				{
					if (rank.Queue == "RANKED_SOLO_5x5")
					{
						foreach (Entry person in rank.Entries)
						{
							if (Convert.ToInt32(person.PlayerOrTeamId) == _userids)
							{
								return person.Division;
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
        public string GetRankedSoloTier()
        {
			
				foreach (RankedId rank in _rankedStatus.RankedId)
				{
					if (rank.Queue == "RANKED_SOLO_5x5")
					{
						return rank.Tier;
					}
				}
			
            return "Unranked";
        }
        /// <summary>
        /// checks if the Connection is val
        /// </summary>
        /// <returns>bool</returns>
        public bool IsValid()
        {
           
            if (_rankedStatus != null)
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
        public Entry[] GetSoloQueueLeague(string division, string region)
        {
			foreach (RankedId rank in _rankedStatus.RankedId)
            {
                if (rank.Queue == "RANKED_SOLO_5x5")
                {	
                    return rank.Entries;
                }
            }
            return null;
        }

		public string GetLeagueName()
		{
			foreach (RankedId rank in _rankedStatus.RankedId)
			{
				if (rank.Queue == "RANKED_SOLO_5x5")
				{
					return rank.Name;
				}
			}
			return null;
		}

		public string GetLeagueIdList(string division, string region)
		{
			string idList = "";
			foreach (RankedId rank in _rankedStatus.RankedId)
			{
				if (rank.Queue == "RANKED_SOLO_5x5")
				{
					foreach (Entry user in rank.Entries)
					{
						if (user.Division == division)
						{
							idList += user.PlayerOrTeamId + ",";
						}
					}
				}
				return idList;
			}
			return null;
		}
    }
}
