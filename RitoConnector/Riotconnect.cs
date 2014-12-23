using System;
using System.Windows;
using System.Windows.Controls;
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
        public Riotconnect(string username, string Region,string key)
        {
            string JSONRAW;
            WebResponse Response;
            string URI = "https://" + Region.ToLower() + ".api.pvp.net/api/lol/" + Region.ToLower() + "/v1.4/summoner/by-name/" + username + "?api_key=" + key;
            WebRequest ConnectionListener = WebRequest.Create(URI);
            ConnectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                Response = ConnectionListener.GetResponse();
            }
            catch(WebException e){
                System.Windows.MessageBox.Show(e.Message);
                   Response = null;
                   return;
            }
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }
            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(JSONRAW);
            CleanSummonerJSON = tempjson[username.ToLower().Replace(" ",string.Empty)].ToString();
            User = JsonConvert.DeserializeObject<SummonerDTO>(CleanSummonerJSON);
            
        }
        public int GetProfileIcon()
        {
            return User.ProfileIconId;
        }
        public string GetProfileIconURL()
        {
            string URL = "http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + User.ProfileIconId + ".png";
            return URL;
        }
        public string getUsername()
        {
            return User.Name;
        }
        public DateTime GetLastRefresh()
        {
            long unixDate = User.RevisionDate;
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date= start.AddMilliseconds(unixDate).ToLocalTime();
            return date;
        }
        public int GetSummonerLevel()
        {
            return User.SummonerLevel;
        }
        public int GetUserID()
        {
            return User.Id;
        }
    }
}
