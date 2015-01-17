using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			var cache = new CacheManager();
			CacheManager.PrepareRoaming();		//Creates Local Files if necessary

			var username = UsernameTextbox.Text;

			if (RegionBox.SelectedItem == null)
			{
				MessageBox.Show("Please select a region");
				return;
			}
			var region = RegionBox.SelectedItem.ToString();

	        var key = ApiKey.Text == "" ? Keyloader.GetRealKey() : ApiKey.Text;

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
				        db.UpdateRank(username, region, rankedConnection.GetRankedSoloTier(), rankedConnection.GetRankedSoloDivision(),rankedConnection.GetLeagueName(), rankedConnection.GetLpByUser(username), rankedConnection.GetMiniSeriesUserId(username));
						string rawIDList = rankedConnection.GetLeagueIdList(rankedConnection.GetRankedSoloDivision(), region);
						string[] IDList = rawIDList.Split(',');
						if (IDList.Length <= 40)
						{
							var multi = new MultipleIdGrabber(rawIDList, region, key);
							foreach (var user in multi.GetUserDtOs().Where(user => user.Id != db.GetUserId(username, region)))
							{
								if (!db.UserInDatabase(user.Name, region))
								{
									db.InsertUserinDatabase(user.Id, region, user.Name, user.Name, user.SummonerLevel, user.ProfileIconId);
									db.UpdateRank(user.Name.ToLower(), region, rankedConnection.GetRankedSoloTier(), rankedConnection.GetRankedSoloDivision(), rankedConnection.GetLeagueName(), rankedConnection.GetLpByUser(user.Name), rankedConnection.GetMiniSeriesUserId(user.Name));
								}
							}
						}
						else
						{
							string splitIDList = "";
							int m = 0;
							int n = 0;
							foreach (string ID in IDList)
							{
								if(n < 40)
								{
									splitIDList += ID + ",";
									m++;
									n++;
								}
								if (n == 40 || m == IDList.Length)
								{
									var multi = new MultipleIdGrabber(splitIDList, region, key);
									foreach (var user in multi.GetUserDtOs().Where(user => user.Id != db.GetUserId(username, region)))
									{
										if (!db.UserInDatabase(user.Name, region))
										{
											db.InsertUserinDatabase(user.Id, region, user.Name, user.Name, user.SummonerLevel, user.ProfileIconId);
											db.UpdateRank(user.Name.ToLower(), region, rankedConnection.GetRankedSoloTier(), rankedConnection.GetRankedSoloDivision(), rankedConnection.GetLeagueName(), rankedConnection.GetLpByUser(user.Name), rankedConnection.GetMiniSeriesUserId(user.Name));
										}
									}
									n = 0;
									splitIDList = "";
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
			        db.UpdateRank(username, region, "Unranked", null,null,null,null);
		        }
	        }
	        //Sets Name
		    UsernameLabel.Text = db.GetName(username, region);
	
		    //Sets Profile Icon
		    ProfileIcon.Source = cache.ProfileIcon(db.GetProfileIconId( username, region));

		    //Sets Level
		    LevelLabel.Text = db.GetLevel(username, region).ToString();

		    //Sets Ranked
		    Rankstatus.Text = db.GetSoloTier(username, region);
		    Divisionstatus.Text = db.GetSoloDivision(username, region);
		    RankedImage.Source = cache.RankedIcon(db.GetSoloTier(username, region), db.GetSoloDivision(username, region));

			//WIP
			ObservableCollection<string> NameListLeague = new ObservableCollection<string>();
            Dictionary<string, int> test = new Dictionary<string, int>();
            foreach (var user in ) // edit
            {
					if (user.Division == Connection2.GetRankedSoloDivision())
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
					foreach (Entry user2 in Connection2.GetSoloQueueLeague("s","s"))
					{
						if (user2.PlayerOrTeamName == user.Key)
							{
								HotStreak = user2.MiniSeries.Progress;
								//NEW: db.GetMiniseries(USERNAME, REGION);
							}
					}
					NameListLeague.Add(user.Key + " " + user.Value.ToString() + " LP | " + HotStreak.Replace("N","_ ").Replace("L","X").Replace("W","✓"));
				}
				else
				{
					NameListLeague.Add(user.Key + " " + user.Value.ToString() + " LP");
					//NEW: db.GetLeaguePoints(USERNAME, REGION);
				}
			}
			RankedLeague.ItemsSource = NameListLeague;
			// END WIP


			//Switches to Profile Tab
		    Tabs.SelectedIndex = 1;
	        db.CloseConnection();
		}
	        
		/*
                    }
                    Matchhistory matches = new Matchhistory(db.GetUserId(username,region), region, key);
                    ObservableCollection<string> Games= new ObservableCollection<string>();
                    if (matches.IsValid())
                    {
                        
                        foreach (var Match in matches.GetGames())
                        {
							Games.Add(ChampionTransform.GetChampName(Match.ChampionId) + " " + matches.GetStats(Match.GameId) + " " + matches.GetGameType(Match.GameId) + " " + matches.GetFarm(Match.GameId));

                        }
                        Matchhistorybox.ItemsSource = Games;
                    }
                
                else
                {
                    MessageBox.Show("An unknown Error has occured. Please try again later");
                }
		*/

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
