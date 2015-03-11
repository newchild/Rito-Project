using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace RitoConnector
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ObservableCollection<int> _games = new ObservableCollection<int>();
        private readonly Dictionary<int, string> _matchInfo = new Dictionary<int, string>();

        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += delegate { this.DragMove(); };
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e) {}
        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e) {}

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
            anim.Completed += (s, _) => Close();
            BeginAnimation(OpacityProperty, anim);
        }

        /*
         * 
         * Lacking await operators
         * 
         */

        private async void Connect(object sender, RoutedEventArgs e)
        {
            var playerdivion = "";
            var cache = new CacheManager();
            CacheManager.PrepareRoaming(); //Creates Local Files if necessary

            var username = _usernameTextbox.Text;

            if (_regionBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a region");
                return;
            }
            var region = _regionBox.SelectedItem.ToString();

            var key = _apiKey.Text == "" ? Keyloader.GetRealKey() : _apiKey.Text;

            var db = new SqlManager();
            if (!db.UserInDatabase(username, region))
            {
                var connection = new Riotconnect(username, region, key);
                if (connection.IsValid())
                {
                    db.InsertUserinDatabase(
                        connection.GetUserId(), region, username, connection.GetUsername(),
                        connection.GetSummonerLevel(), connection.GetProfileIcon());
                }
                else
                {
                    MessageBox.Show("Connection to the Riot Server failed. Please try again later");
                    db.CloseConnection();
                    return;
                }
                if (db.GetLevel(username, region) == 30)
                {
                    //only starts Ranked Call if Summoner is Level 30
                    var rankedConnection = new RankedHandler(db.GetUserId(username, region), region, key);
                    if (rankedConnection.IsValid())
                    {
                        db.UpdateRank(
                            username, region, rankedConnection.GetRankedSoloTier(),
                            rankedConnection.GetRankedSoloDivision(), rankedConnection.GetLeagueName(),
                            rankedConnection.GetLpByUser(username), rankedConnection.GetMiniSeriesUserId(username));
                        var rawIdList = rankedConnection.GetLeagueIdList(
                            rankedConnection.GetRankedSoloDivision(), region);
                        var idList = rawIdList.Split(',');
                        if (idList.Length <= 40)
                        {
                            var multi = new MultipleIdGrabber(rawIdList, region, key);
                            foreach (
                                var user in multi.GetUserDtOs().Where(user => user.Id != db.GetUserId(username, region))
                                )
                            {
                                if (!db.UserInDatabase(user.Name, region))
                                {
                                    db.InsertUserinDatabase(
                                        user.Id, region, user.Name, user.Name, user.SummonerLevel, user.ProfileIconId);
                                    db.UpdateRank(
                                        user.Name.ToLower(), region, rankedConnection.GetRankedSoloTier(),
                                        rankedConnection.GetRankedSoloDivision(), rankedConnection.GetLeagueName(),
                                        rankedConnection.GetLpByUser(user.Name),
                                        rankedConnection.GetMiniSeriesUserId(user.Name));
                                }
                            }
                        }
                        else
                        {
                            var splitIdList = "";
                            var m = 0;
                            var n = 0;
                            foreach (var id in idList)
                            {
                                if (n < 40)
                                {
                                    splitIdList += id + ",";
                                    m++;
                                    n++;
                                }
                                if (n == 40 || m == idList.Length)
                                {
                                    var multi = new MultipleIdGrabber(splitIdList, region, key);
                                    foreach (
                                        var user in
                                            multi.GetUserDtOs().Where(user => user.Id != db.GetUserId(username, region))
                                        )
                                    {
                                        if (!db.UserInDatabase(user.Name, region))
                                        {
                                            db.InsertUserinDatabase(
                                                user.Id, region, user.Name, user.Name, user.SummonerLevel,
                                                user.ProfileIconId);
                                            db.UpdateRank(
                                                user.Name.ToLower(), region, rankedConnection.GetRankedSoloTier(),
                                                rankedConnection.GetRankedSoloDivision(),
                                                rankedConnection.GetLeagueName(),
                                                rankedConnection.GetLpByUser(user.Name),
                                                rankedConnection.GetMiniSeriesUserId(user.Name));
                                        }
                                    }
                                    n = 0;
                                    splitIdList = "";
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Connection to the Riot Server failed. Please try again later");
                        db.CloseConnection();
                        return;
                    }
                }
                else
                {
                    db.UpdateRank(username, region, "Unranked", null, null, null, null);
                }
            }
            //Sets Name
            _usernameLabel.Text = db.GetName(username, region);

            //Sets Profile Icon
            _profileIcon.Source = cache.ProfileIcon(db.GetProfileIconId(username, region));

            //Sets Level
            _levelLabel.Text = db.GetLevel(username, region).ToString();

            //Sets Ranked
            _rankstatus.Text = db.GetSoloTier(username, region);
            _divisionstatus.Text = db.GetSoloDivision(username, region);
            _rankedImage.Source = cache.RankedIcon(
                db.GetSoloTier(username, region), db.GetSoloDivision(username, region));

            //WIP
            var rankjson = "";
            if (File.Exists(CacheManager.GetRessources() + db.GetUserId(username, region) + ".json"))
            {
                rankjson = CacheManager.GetJson(db.GetUserId(username, region).ToString());
            }
            else
            {
                MessageBox.Show("Added new user. Please restart to reload");
                Close();
                return;
            }
            var rankedStatus = JsonConvert.DeserializeObject<RankedDto>(rankjson);
            var nameListLeague = new ObservableCollection<string>();

            foreach (var rank in rankedStatus.RankedId)
            {
                if (rank.Queue == "RANKED_SOLO_5x5")
                {
                    foreach (var person in rank.Entries)
                    {
                        if (username == person.PlayerOrTeamName)
                        {
                            playerdivion = person.Division;
                        }
                    }
                }
            }
            var test = (from rankedId in rankedStatus.RankedId where rankedId.Queue == "RANKED_SOLO_5x5" from user in rankedId.Entries where user.Division == playerdivion select user).ToDictionary(user => user.PlayerOrTeamName, user => user.LeaguePoints);
            var sortedDict = from entry in test orderby entry.Value descending select entry;
            foreach (var user in sortedDict)
            {
                if (user.Value == 100)
                {
                    var hotStreak = "";

                    foreach (var rankedId in rankedStatus.RankedId) // edit
                    {
                        if (rankedId.Queue == "RANKED_SOLO_5x5")
                        {
                            foreach (var user2 in rankedId.Entries)
                            {
                                if (user2.PlayerOrTeamName == user.Key)
                                {
                                    hotStreak = user2.MiniSeries.Progress;
                                }
                            }
                        }
                    }
                    nameListLeague.Add(
                        user.Key + " " + user.Value + " LP | " +
                        hotStreak.Replace("N", "_ ").Replace("L", "X").Replace("W", "✓"));
                }
                else
                {
                    nameListLeague.Add(user.Key + " " + user.Value + " LP");
                    //NEW: db.GetLeaguePoints(USERNAME, REGION);
                }
            }
            _rankedLeague.ItemsSource = nameListLeague;
            // END WIP


            //Switches to Profile Tab
            _tabs.SelectedIndex = 1;


            var matches = new Matchhistory(db.GetUserId(username, region), region, key);

            if (matches.IsValid())
            {
                foreach (var match in matches.GetGames())
                {
                    _games.Add(match.GameId);
                    var items = new int[7];

                    items[0] = match.Stats.Item0;

                    items[1] = match.Stats.Item1;

                    items[2] = match.Stats.Item2;

                    items[3] = match.Stats.Item3;

                    items[4] = match.Stats.Item4;
                    if (match.Stats.Item5 != null)
                    {
                        items[5] = match.Stats.Item5.Value;
                    }
                    else
                    {
                        items[5] = 0;
                    }

                    items[6] = match.Stats.Item6;


                    var itemBuild = items.Aggregate("", (current, element) => current + (" " + ItemConverter.GetItemName(element.ToString(), region, Keyloader.GetRealKey()) + " |"));
                    itemBuild = itemBuild.Remove(0, 1);
                    _matchInfo.Add(
                        match.GameId,
                        "Champion: " + ChampionTransform.GetChampName(match.ChampionId) + "\n" + "Gamemode: " +
                        matches.GetGameType(match.GameId) + "\n" + "IP Earned: " + match.IpEarned + "\n" + itemBuild);
                }
                _matchhistorybox.ItemsSource = _games;
            }

            else
            {
                MessageBox.Show("An unknown Error has occured. Please try again later");
            }
            db.CloseConnection();
        }

        private void RankedLeagueSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _rankedLeague.UnselectAll();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            SqlManager.ResetDb();
            CacheManager.ResetCache();
        }

        private DateTime ToDateTime(int synonDate)
        {
            var day = synonDate % 100;
            var dateWithoutDay = synonDate / 100;
            var month = dateWithoutDay % 100;
            var dateWithoutDayAndMonth = dateWithoutDay / 100;
            var year = dateWithoutDayAndMonth % 100;
            var century = dateWithoutDayAndMonth / 100;

            if (0 == day || 0 == month)
            {
                return new DateTime();
            }
            return new DateTime((19 + century) * 100 + year, month, day);
        }

        private void MatchHistorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gamesBuffer = new ObservableCollection<string>();
            if (_matchhistorybox.SelectedValue != null)
            {
                gamesBuffer.Add(_matchInfo[(int) _matchhistorybox.SelectedValue]);
                _matchhistoryInfo.ItemsSource = gamesBuffer;
            }
            else
            {
                _matchhistoryInfo.ItemsSource = "Meh";
            }
        }
    }
}