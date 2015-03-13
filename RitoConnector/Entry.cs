﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using Newtonsoft.Json;

namespace RitoConnector
{

    public class Entry
    {

        [JsonProperty("leaguePoints")]
        public int LeaguePoints { get; set; }

        [JsonProperty("isFreshBlood")]
        public bool IsFreshBlood { get; set; }

        [JsonProperty("isHotStreak")]
        public bool IsHotStreak { get; set; }

        [JsonProperty("division")]
        public string Division { get; set; }

        [JsonProperty("isInactive")]
        public bool IsInactive { get; set; }

        [JsonProperty("isVeteran")]
        public bool IsVeteran { get; set; }

        [JsonProperty("playerOrTeamName")]
        public string PlayerOrTeamName { get; set; }

        [JsonProperty("playerOrTeamId")]
        public string PlayerOrTeamId { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("miniSeries")]
        public MiniSeries MiniSeries { get; set; }
    }

}
