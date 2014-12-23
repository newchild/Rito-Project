using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RitoConnector
{
    class Riotconnect
    {
        private SummonerDTO User;
        private string CleanSummonerJSON;
        Riotconnect(string username, string Region,string key = "64becc79-cc38-40e1-afdf-a92b95b4c836")
        {
            string JSONRAW;
            string URI = "https://" + Region + ".api.pvp.net/api/lol/" + Region + "/v1.4/summoner/by-name/" + username + "?api_key=" + key;
            WebRequest ConnectionListener = WebRequest.Create(URI);
            ConnectionListener.ContentType = "application/json; charset=utf-8";
            WebResponse Response = ConnectionListener.GetResponse();
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }
            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(JSONRAW);
            CleanSummonerJSON = tempjson[username.ToLower().Replace(" ",string.Empty)].ToString();
            User = JsonConvert.DeserializeObject<SummonerDTO>(CleanSummonerJSON);
        }
        int GetProfileIcon()
        {
            return User.ProfileIconId;
        }
        string GetProfileIconURL()
        {
            string URL = "http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + User.ProfileIconId + ".png";
            return URL;
        }
        DateTime GetLastRefresh()
        {
            long unixDate = User.RevisionDate;
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date= start.AddMilliseconds(unixDate).ToLocalTime();
            return date;
        }
        int GetSummonerLevel()
        {
            return User.SummonerLevel;
        }
        int GetUserID()
        {
            return User.Id;
        }
    }
}
