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
        private SummonerDto _user;
        private string _cleanSummonerJson;
        public Riotconnect(string username, string region,string key)
        {
            string jsonraw;
            WebResponse response;
            string uri = "https://" + region.ToLower() + ".api.pvp.net/api/lol/" + region.ToLower() + "/v1.4/summoner/by-name/" + username + "?api_key=" + key;
            WebRequest connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch(WebException e){
                   System.Windows.MessageBox.Show(e.Message);
                   response = null;
                   _user = null;
                   return;
            }
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }
            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonraw);
            _cleanSummonerJson = tempjson[username.ToLower().Replace(" ",string.Empty)].ToString();
			MessageBox.Show(_cleanSummonerJson);
            _user = JsonConvert.DeserializeObject<SummonerDto>(_cleanSummonerJson);
        }
        public int GetProfileIcon()
        {
            return _user.ProfileIconId;
        }
        public string GetProfileIconUrl()
        {
            string url = "http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + _user.ProfileIconId + ".png"; //needs update at every patch
            return url;
        }
        public string GetUsername()
        {
            return _user.Name;
        }
        public bool IsValid()
        {
            if(_user != null){
                return true;
            }
            else
            {
                return false;
            }
        }
        public DateTime GetLastRefresh()
        {
            long unixDate = _user.RevisionDate;
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date= start.AddMilliseconds(unixDate).ToLocalTime();
            return date;
        }
        public int GetSummonerLevel()
        {
            return _user.SummonerLevel;
        }
        public int GetUserId()
        {
            return _user.Id;
        }
    }
}
