using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace RitoConnector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += delegate { this.DragMove();};
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
            anim.Completed += (s, _) => Close();
            BeginAnimation(OpacityProperty, anim);
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
			var error = false;

			var cache = new CacheManager();

			var username = UsernameTextbox.Text;
			var region = RegionBox.SelectedItem.ToString();

	        var key = ApiKey.Text == "" ? Keyloader.GetRealKey() : ApiKey.Text;

            if (RegionBox.SelectedItem == null)
            {
				error = true;
                MessageBox.Show("Please select a region");
            }

	        if (error) return;
	        var db = new SqlManager();
	        if (!db.UserInDatabase(username, region))
	        {
		        var connection = new Riotconnect(username, region, key);
		        if (connection.IsValid())
		        {
			        db.InsertUserinDatabase(connection.GetUserId(), region, username, connection.GetUsername(), connection.GetSummonerLevel(), connection.GetProfileIcon());	
		        }
		        else
		        {
			        error = true;
			        MessageBox.Show("Connection to the Riot Server failed. Please try again later");
		        }
		        if (db.GetLevel(username, region) == 30)
		        {
			        //only starts Ranked Call if Summoner is Level 30
			        var rankedConnection = new RankedHandler(db.GetUserId(username, region), region, key);
			        if (rankedConnection.IsValid())
			        {
				        db.UpdateRank(username, region, rankedConnection.GetRankedSoloTier(), rankedConnection.GetRankedSoloDivision(),rankedConnection.GetLeagueName());
				        var multi = new MultipleIdGrabber(rankedConnection.GetLeagueIdList(rankedConnection.GetRankedSoloDivision(), region), region, key);
				        foreach (var user in multi.GetUserDtOs().Where(user => user.Id != db.GetUserId(username, region)))
				        {
					        db.InsertUserinDatabase(user.Id, region, user.Name, user.Name, user.SummonerLevel, user.ProfileIconId);
					        db.UpdateRank(user.Name.ToLower(), region, rankedConnection.GetRankedSoloTier(), rankedConnection.GetRankedSoloDivision(), rankedConnection.GetLeagueName());
				        }
			        }
			        else
			        {
				        error = true;
				        MessageBox.Show("Connection to the Riot Server failed. Please try again later");
			        }
		        }
		        else
		        {
			        db.UpdateRank(username, region, "Unranked", null,null);
		        }
	        }
	        if (!error)
	        {
		        //Sets Name
		        UsernameLabel.Text = db.GetName(username, region);
					
		        //Sets Profile Icon
		        ProfileIcon.Source = cache.ProfileIcon(db.GetProfileIconId( username, region));

		        //Sets Level
		        LevelLabel.Text = db.GetLevel(username, region).ToString();

		        //Switches to Profile Tab
		        Tabs.SelectedIndex = 1;

		        //Sets Ranked
		        Rankstatus.Text = db.GetSoloTier(username, region);
		        Divisionstatus.Text = db.GetSoloDivision(username, region);
		        RankedImage.Source = cache.RankedIcon(db.GetSoloTier(username, region), db.GetSoloDivision(username, region));
	        }
	        db.CloseConnection();
	        /*
				Riotconnect Connection = new Riotconnect(UsernameTextbox.Text, RegionBox.SelectedItem.ToString(), key);
                if (Connection.isValid())
                {
                    RankedHandler Connection2 = new RankedHandler(Connection.GetUserID(), RegionBox.SelectedItem.ToString(), key);
                    if (Connection2.isValid())
                    {
                        ObservableCollection<string> NameListLeague = new ObservableCollection<string>();
                        Dictionary<string, int> test = new Dictionary<string, int>();
                        foreach (Entry user in Connection2.getSoloQueueLeague())
                        {
                            if (user.Division == Connection2.getRankedSoloLeague())
                            {
                                test.Add(user.PlayerOrTeamName, user.LeaguePoints);
                            }
						}
                        var sortedDict = from entry in test orderby entry.Value descending select entry;
                        foreach (var user in sortedDict)
                        {
                            if (user.Value == 100)
                            {
                                string HotStreak = "";
                                foreach (Entry user2 in Connection2.getSoloQueueLeague())
                                {
                                    if (user2.PlayerOrTeamName == user.Key)
                                    {
                                        HotStreak = user2.MiniSeries.Progress;
                                    }
                                }
                                NameListLeague.Add(user.Key + " " + user.Value.ToString() + " LP | " + HotStreak.Replace("N","_ ").Replace("L","X").Replace("W","✓"));
                            }
                            else
                            {
                                NameListLeague.Add(user.Key + " " + user.Value.ToString() + " LP");
                            }

                        }
                        RankedLeague.ItemsSource = NameListLeague;
                        UpdateLaper.Content = Connection.GetLastRefresh().ToString();
                    }
                    Matchhistory matches = new Matchhistory(Connection.GetUserID(), RegionBox.SelectedItem.ToString(), key);
                    ObservableCollection<string> Games= new ObservableCollection<string>();
                    if (matches.isValid())
                    {
                        
                        foreach (var Match in matches.getGames())
                        {
                            Games.Add(Match.GameMode + " " + Match.GameType + " " + ChampionTransform.getChampName(Match.ChampionId));

                        }
                        Matchhistorybox.ItemsSource = Games;
                    }
                }
                else
                {
                    MessageBox.Show("An unknown Error has occured. Please try again later");
                }
				*/
        }

        private void RankedLeague_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			RankedLeague.UnselectAll();
        }

		private void Reset(object sender, RoutedEventArgs e)
		{
			SqlManager.ResetDb();
			CacheManager.ResetCache();
		}
    }
}
