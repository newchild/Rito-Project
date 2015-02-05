using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

namespace RitoConnector
{
    class Riotconnect
    {
        private readonly SummonerDto _user;

	    public Riotconnect(string username, string region,string key)
        {
	        string jsonraw;
            WebResponse response;
            var uri = "https://" + region.ToLower() + ".api.pvp.net/api/lol/" + region.ToLower() + "/v1.4/summoner/by-name/" + username + "?api_key=" + key;
            var connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch(WebException e){
                   MessageBox.Show(e.Message);
                   response = null;
                   _user = null;
                   return;
            }
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }
            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonraw);
            var cleanSummonerJson = tempjson[username.ToLower().Replace(" ",string.Empty)].ToString();
			
            _user = JsonConvert.DeserializeObject<SummonerDto>(cleanSummonerJson);
        }
        public int GetProfileIcon()
        {
            return _user.ProfileIconId;
        }
        public string GetProfileIconUrl()
        {
            var url = "http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + _user.ProfileIconId + ".png"; //needs update at every patch
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
            var unixDate = _user.RevisionDate;
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date= start.AddMilliseconds(unixDate).ToLocalTime();
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
