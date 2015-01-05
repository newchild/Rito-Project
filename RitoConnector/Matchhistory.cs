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
    class Matchhistory
    {
        private MatchhistoryDTO Matches;
        private string CleanSummonerJSON;
        public Matchhistory(int userid, string Region, string key)
        {
            string JSONRAW;
            WebResponse Response;
            string URI = "https://" + Region.ToLower() + ".api.pvp.net/api/lol/" + Region.ToLower() + "/v1.3/game/by-summoner/" + userid.ToString() +"/recent" + "?api_key=" + key;
            WebRequest ConnectionListener = WebRequest.Create(URI);
            ConnectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                Response = ConnectionListener.GetResponse();
            }
            catch(WebException e){
                   System.Windows.MessageBox.Show(e.Message);
                   Response = null;
                   Matches = null;
                   return;
            }
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }
            Matches = JsonConvert.DeserializeObject<MatchhistoryDTO>(JSONRAW);
            
            
        }
        
        public bool isValid()
        {
            if(Matches != null){
                return true;
            }
            else
            {
                return false;
            }
        }
        public Game[] getGames()
        {
            return Matches.Games;
        }
    }
}
