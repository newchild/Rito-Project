using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;

namespace RitoConnector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { this.DragMove();};
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.5));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
			bool error = false;

			CacheManager cache = new CacheManager();

			string username = UsernameTextbox.Text;
			string region = RegionBox.SelectedItem.ToString();

			string key;
            if (apiKey.Text == "")
            {
                key = Keyloader.getRealKey();
            }
            else
            {
                key = apiKey.Text;
            }

            if (RegionBox.SelectedItem == null)
            {
				error = true;
                MessageBox.Show("Please select a region");
            }
            
			if (!error)
            {
				SQLManager DB = new SQLManager();
				if (!DB.userInDatabase(username, region))
				{
					Riotconnect Connection = new Riotconnect(username, region, key);
					if (Connection.isValid())
					{
						DB.insertUserinDatabase(Connection.GetUserID(), region, username, Connection.GetUsername(), Connection.GetSummonerLevel(), Connection.GetProfileIcon());	
					}
					else
					{
						error = true;
						MessageBox.Show("Connection to the Riot Server failed. Please try again later");
					}
					if (DB.GetLevel(username, region) == 30)
					{
						//only starts Ranked Call if Summoner is Level 30
						RankedHandler RankedConnection = new RankedHandler(DB.GetUserID(username, region), region, key);
						if (RankedConnection.isValid())
						{
							DB.updateRank(username, region, RankedConnection.getRankedSoloTier(), RankedConnection.getRankedSoloDivision(),RankedConnection.getLeagueName());
							MultipleIDGrabber multi = new MultipleIDGrabber(RankedConnection.getLeagueIDList(RankedConnection.getRankedSoloDivision(), region), region, key);
							foreach(SummonerDTO user in multi.getUserDTOs())
							{
								if (!(user.Id == DB.GetUserID(username, region)))
								{
									DB.insertUserinDatabase(user.Id, region, user.Name, user.Name, user.SummonerLevel, user.ProfileIconId);
									DB.updateRank(user.Name.ToLower(), region, RankedConnection.getRankedSoloTier(), RankedConnection.getRankedSoloDivision(), RankedConnection.getLeagueName());
								}
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
						DB.updateRank(username, region, "Unranked", null,null);
					}
				}
				if (!error)
				{
					//Sets Name
					UsernameLabel.Text = DB.GetName(username, region);
					
					//Sets Profile Icon
					ProfileIcon.Source = cache.ProfileIcon(DB.GetProfileIconID( username, region));

					//Sets Level
					LevelLabel.Text = DB.GetLevel(username, region).ToString();

					//Switches to Profile Tab
					Tabs.SelectedIndex = 1;

					//Sets Ranked
					Rankstatus.Text = DB.GetSoloTier(username, region);
					Divisionstatus.Text = DB.GetSoloDivision(username, region);
					RankedImage.Source = cache.RankedIcon(DB.GetSoloTier(username, region), DB.GetSoloDivision(username, region));
				}
				DB.closeConnection();
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
        }

        private void RankedLeague_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			RankedLeague.UnselectAll();
        }

		private void Reset(object sender, RoutedEventArgs e)
		{
			SQLManager.resetDB();
			CacheManager.resetCache();
		}
    }
}
