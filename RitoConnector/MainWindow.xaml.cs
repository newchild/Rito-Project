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
				if (!DB.userInDatabase(username))
				{
					Riotconnect Connection = new Riotconnect(UsernameTextbox.Text, RegionBox.SelectedItem.ToString(), key);
					if (Connection.isValid())
					{
						DB.insertUserinDatabase(Connection.GetUserID(), RegionBox.SelectedItem.ToString(), UsernameTextbox.Text, Connection.GetUsername(), Connection.GetSummonerLevel(), Connection.GetProfileIcon());
					}
					else
					{
						error = true;
						MessageBox.Show("Connection to the Riot Server failed. Please try again later");
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
				}
				DB.closeConnection();
                /*
				Riotconnect Connection = new Riotconnect(UsernameTextbox.Text, RegionBox.SelectedItem.ToString(), key);
                if (Connection.isValid())
                {
                    RankedHandler Connection2 = new RankedHandler(Connection.GetUserID(), RegionBox.SelectedItem.ToString(), key);
                    if (Connection2.isValid())
                    {
                        BitmapImage logo = new BitmapImage();
                        logo.BeginInit();
						if (!Directory.Exists("./resources"))
						{
							Directory.CreateDirectory("./resources");
						}
						if (!File.Exists(@"./resources/" + Connection.GetProfileIcon() + ".png"))
						{
							try
							{	byte[] data;
								using (WebClient webclient = new WebClient())
								{
									data = webclient.DownloadData(Connection.GetProfileIconURL());
								}
								File.WriteAllBytes(@"./resources/" + Connection.GetProfileIcon() + ".png" , data);
							}
							catch (WebException e1)
							{
								System.Windows.MessageBox.Show(e1.Message);
							}
						}
						logo.StreamSource = new FileStream(@"./resources/" + Connection.GetProfileIcon() + ".png", FileMode.Open, FileAccess.Read);
                        logo.EndInit();
                        ProfileIcon.Source = logo;

                        LevelLabel.Text = Connection.GetSummonerLevel().ToString();

                        UsernameLabel.Text = Connection.getUsername();

                        BitmapImage RankedPic = new BitmapImage();
                        RankedPic.BeginInit();
                        RankedPic.UriSource = new Uri("https://raw.githubusercontent.com/newchild/Rito-Project/master/RitoConnector/Ressources/" + Connection2.getRankedSoloTier().ToLower() + ".png");
                        RankedPic.EndInit();
                        RankedImage.Source = RankedPic;

                        Divisionstatus.Text = Connection2.getRankedSoloLeague();

                        Rankstatus.Text = Connection2.getRankedSoloTier();

                        LevelLabel.Visibility = Visibility.Visible;
                        UsernameLabel.Visibility = Visibility.Visible;

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
                        Tabs.SelectedIndex = 1;
                        UpdateLaper.Content = Connection.GetLastRefresh().ToString();
                    }
                    else
                    {
                        BitmapImage logo = new BitmapImage();
                        logo.BeginInit();
                        logo.UriSource = new Uri(Connection.GetProfileIconURL());
                        logo.EndInit();
                        ProfileIcon.Source = logo;
                        LevelLabel.Text = Connection.GetSummonerLevel().ToString();
                        UsernameLabel.Text = Connection.getUsername();
                        BitmapImage RankedPic = new BitmapImage();
                        RankedPic.BeginInit();
                        RankedPic.UriSource = new Uri("https://raw.githubusercontent.com/newchild/Rito-Project/master/RitoConnector/Ressources/unranked.png");
                        RankedPic.EndInit();
                        RankedImage.Source = RankedPic;
                        Divisionstatus.Text = "Unranked";
                        Rankstatus.Text = "Unranked";
                        LevelLabel.Visibility = Visibility.Visible;
                        UsernameLabel.Visibility = Visibility.Visible;
                        Tabs.SelectedIndex = 1;
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
    }
}
