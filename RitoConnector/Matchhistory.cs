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
        public string getChampionName(int MatchID)
        {
            Game MatchingGame = null;
            foreach(var match in Matches.Games)
            {
                if (match.GameId == MatchID)
                {
                    MatchingGame = match;
                }
          
            }
            if (MatchingGame != null)
            {
                return ChampionTransform.getChampName(MatchingGame.ChampionId);
            }
            else
            {
                return "INVALID";
            }
        }
        public int[] getMatchhistoryIDs()
        {
            int i = 0;
            int[] test = new int[10];
            foreach (var match in Matches.Games)
            {
                test[i] = match.GameId;
                i++;
            }
            return test;
        }
        public string GetGameMode(int MatchID)
        {
            Game MatchingGame = null;
            foreach (var match in Matches.Games)
            {
                if (match.GameId == MatchID)
                {
                    MatchingGame = match;
                }

            }
            if (MatchingGame != null)
            {
                return getRealMode(MatchingGame.GameMode);
            }
            else
            {
                return "INVALID";
            }

        }
        public string GetStats(int MatchID)
        {
            Game MatchingGame = null;
            foreach (var match in Matches.Games)
            {
                if (match.GameId == MatchID)
                {
                    MatchingGame = match;
                }

            }
            if (MatchingGame != null)
            {
                return MatchingGame.Stats.ChampionsKilled.ToString() + "/" + MatchingGame.Stats.NumDeaths.ToString() + "/" + MatchingGame.Stats.Assists.ToString();
            }
            else
            {
                return "INVALID";
            }

        }
        public int GetFarm(int MatchID)
        {
            Game MatchingGame = null;
            foreach (var match in Matches.Games)
            {
                if (match.GameId == MatchID)
                {
                    MatchingGame = match;
                }

            }
            if (MatchingGame != null)
            {
                return MatchingGame.Stats.MinionsKilled;
            }
            else
            {
                return -1;
            }

        }
        private string getRealMode(string Matchtype)
        {
            switch (Matchtype)
            {
                case "CLASSIC":
                    return "Normal";
                case "ODIN":
                    return "Dominion";
                case "ARAM":
                    return "Aram";
                case "TUTORIAL":
                    return "Tutorialgame";
                case "ONEFORALL":
                    return "One for All";
                case "ASCENSION":
                    return "Ascension";
                case "FIRSTBLOOD":
                    return "Snowdown Showdown";
                case "KINGPORO":
                    return "Poroking";
          
                    
            }
            return "INVALID";
        }
        
    }
}
