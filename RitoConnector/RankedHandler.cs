using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

namespace RitoConnector
{
    internal class RankedHandler
    {
        private readonly RankedDto _rankedStatus;
        private readonly string _region;
        private readonly int _userids;

        /// <summary>
        ///     sends a request to the Riotserver
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="region"></param>
        /// <param name="key"></param>
        public RankedHandler(int userid, string region, string key)
        {
            _userids = userid;
            _region = region;
            string jsonraw;
            WebResponse response;
            var uri = "https://" + region.ToLower() + ".api.pvp.net/api/lol/" + region.ToLower() +
                      "/v2.5/league/by-summoner/" + userid + "?api_key=" + key;
            var connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch (WebException e)
            {
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
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }

            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonraw);
            jsonraw = tempjson[userid.ToString()].ToString();
            jsonraw = "{ \"standard\" : " + jsonraw + "}";
            CacheManager.SaveJson(userid + string.Empty, jsonraw);
            _rankedStatus = JsonConvert.DeserializeObject<RankedDto>(jsonraw);
        }

        /// <summary>
        ///     returns the current Division
        /// </summary>
        /// <returns>string</returns>
        public string GetRankedSoloDivision()
        {
            foreach (var person in from rank in _rankedStatus.RankedId
                where rank.Queue == "RANKED_SOLO_5x5"
                from person in rank.Entries
                where Convert.ToInt32(person.PlayerOrTeamId) == _userids
                select person)
            {
                return person.Division;
            }
            return "unkown";
        }

        /// <summary>
        ///     Gets the current SoloQ League
        /// </summary>
        /// <returns>string</returns>
        public string GetRankedSoloTier()
        {
            foreach (var rank in _rankedStatus.RankedId.Where(rank => rank.Queue == "RANKED_SOLO_5x5"))
            {
                return rank.Tier;
            }

            return "Unranked";
        }

        public int GetLpByUser(string userName)
        {
            return
                GetSoloQueueLeague(GetRankedSoloDivision(), _region)
                    .Where(user => userName == user.PlayerOrTeamName)
                    .Select(user => user.LeaguePoints)
                    .FirstOrDefault();
        }

        private MiniSeries GetMiniSeriesByUser(string userName)
        {
            return
                GetSoloQueueLeague(GetRankedSoloDivision(), _region)
                    .Where(user => userName == user.PlayerOrTeamName)
                    .Select(user => user.MiniSeries)
                    .FirstOrDefault();
        }

        public string GetMiniSeriesUserId(string userName)
        {
            if (GetLpByUser(userName) != 100)
            {
                return null;
            }

            var helpervar = GetMiniSeriesByUser(userName);
            return helpervar.Progress.Replace("N", "_ ").Replace("L", "X").Replace("W", "✓");
        }

        /// <summary>
        ///     checks if the Connection is val
        /// </summary>
        /// <returns>bool</returns>
        public bool IsValid()
        {
            return _rankedStatus != null;
        }

        /// <summary>
        ///     Get the Current List of League Participants
        /// </summary>
        /// <returns>Entry[]</returns>
        public Entry[] GetSoloQueueLeague(string division, string region)
        {
            return
                (from rank in _rankedStatus.RankedId where rank.Queue == "RANKED_SOLO_5x5" select rank.Entries)
                    .FirstOrDefault();
        }

        public string GetLeagueName()
        {
            return
                (from rank in _rankedStatus.RankedId where rank.Queue == "RANKED_SOLO_5x5" select rank.Name)
                    .FirstOrDefault();
        }

        public string GetLeagueIdList(string division, string region)
        {
            return (from rank in _rankedStatus.RankedId
                where rank.Queue == "RANKED_SOLO_5x5"
                from user in rank.Entries
                where user.Division == division
                select user).Aggregate(string.Empty, (current, user) => current + (user.PlayerOrTeamId + ","));
        }
    }
}