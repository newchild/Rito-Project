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
        private MatchhistoryDto _matches;
        public Matchhistory(int userid, string region, string key)
        {
            string jsonraw;
            WebResponse response;
            string uri = "https://" + region.ToLower() + ".api.pvp.net/api/lol/" + region.ToLower() + "/v1.3/game/by-summoner/" + userid.ToString() +"/recent" + "?api_key=" + key;
            WebRequest connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch(WebException e){
                   System.Windows.MessageBox.Show(e.Message);
                   response = null;
                   _matches = null;
                   return;
            }
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }
            _matches = JsonConvert.DeserializeObject<MatchhistoryDto>(jsonraw);
            
            
        }
        
        public bool IsValid()
        {
            if(_matches != null){
                return true;
            }
            else
            {
                return false;
            }
        }
        public Game[] GetGames()
        {
            return _matches.Games;
        }
        public string GetChampionName(int matchId)
        {
            Game matchingGame = null;
            foreach(var match in _matches.Games)
            {
                if (match.GameId == matchId)
                {
                    matchingGame = match;
                }
          
            }
            if (matchingGame != null)
            {
                return ChampionTransform.GetChampName(matchingGame.ChampionId);
            }
            else
            {
                return "INVALID";
            }
        }
        public int[] GetMatchhistoryIDs()
        {
            int i = 0;
            int[] test = new int[10];
            foreach (var match in _matches.Games)
            {
                test[i] = match.GameId;
                i++;
            }
            return test;
        }
        public string GetGameType(int matchId)
        {
            Game matchingGame = null;
            foreach (var match in _matches.Games)
            {
                if (match.GameId == matchId)
                {
                    matchingGame = match;
                }

            }
            if (matchingGame != null)
            {
                return GetRealMode(matchingGame.GameType);
            }
            else
            {
                return "INVALID";
            }

        }
        public string GetGameMap(int matchId)
        {
            Game matchingGame = null;
            foreach (var match in _matches.Games)
            {
                if (match.GameId == matchId)
                {
                    matchingGame = match;
                }

            }
            if (matchingGame != null)
            {
                return GetRealType(matchingGame.GameMode);
            }
            else
            {
                return "INVALID";
            }

        }
        public string GetStats(int matchId)
        {
            Game matchingGame = null;
            foreach (var match in _matches.Games)
            {
                if (match.GameId == matchId)
                {
                    matchingGame = match;
                }

            }
            if (matchingGame != null)
            {
                return matchingGame.Stats.ChampionsKilled.ToString() + "/" + matchingGame.Stats.NumDeaths.ToString() + "/" + matchingGame.Stats.Assists.ToString();
            }
            else
            {
                return "INVALID";
            }

        }
        public int GetFarm(int matchId)
        {
            Game matchingGame = null;
            foreach (var match in _matches.Games)
            {
                if (match.GameId == matchId)
                {
                    matchingGame = match;
                }

            }
            if (matchingGame != null)
            {
                return matchingGame.Stats.MinionsKilled;
            }
            else
            {
                return -1;
            }

        }
        private string GetRealType(string matchtype)
        {
            MessageBox.Show(matchtype);
            switch (matchtype)
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
            return "Unknown";
        }
        private string GetRealMode(string matchmode)
        {
            MessageBox.Show(matchmode);
            switch (matchmode)
            {
                case "CUSTOM_GAME":
                    return "Custom Game";
                case "MATCHED_GAME":
                    return "Normal Game";
                case "TUTORIAL_GAME":
                    return "Tutorial";
            }
            return "Unknown";
        }

        
    }
}
