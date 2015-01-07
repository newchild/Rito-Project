using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

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
            var uri = "https://" + region.ToLower() + ".api.pvp.net/api/lol/" + region.ToLower() + "/v2.5/league/by-summoner/" + userid + "?api_key=" + key;
            var connectionListener = WebRequest.Create(uri);
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
                       MessageBox.Show(e.Message);
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
			
				foreach (var rank in _rankedStatus.RankedId)
				{
					if (rank.Queue == "RANKED_SOLO_5x5")
					{
						foreach (var person in rank.Entries)
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
			
				foreach (var rank in _rankedStatus.RankedId)
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
			foreach (var rank in _rankedStatus.RankedId)
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
			foreach (var rank in _rankedStatus.RankedId)
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
			foreach (var rank in _rankedStatus.RankedId)
			{
				if (rank.Queue == "RANKED_SOLO_5x5")
				{
					foreach (var user in rank.Entries)
					{
						if (user.Division == division)
						{
							idList += user.PlayerOrTeamId + ",";
						}
					}
				}
			}
		return idList;
		}
    }
}
