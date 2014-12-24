using System;
using System.Collections.Generic;
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
            string key;
            if (apiKey.Text == "")
            {
                key = "64becc79-cc38-40e1-afdf-a92b95b4c836";
            }
            else
            {
                key = apiKey.Text;
            }
            Riotconnect Connection = new Riotconnect(UsernameTextbox.Text, RegionBox.SelectedItem.ToString(), key);
            if (Connection.isValid())
            {
                RankedHandler Connection2 = new RankedHandler(Connection.GetUserID(), RegionBox.SelectedItem.ToString(), key);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(Connection.GetProfileIconURL());
                logo.EndInit();
                ProfileIcon.Source = logo;
                LevelLabel.Text = Connection.GetSummonerLevel().ToString();
                UsernameLabel.Text = Connection.getUsername();
                BitmapImage RankedPic = new BitmapImage();
                RankedPic.BeginInit();
                RankedPic.UriSource = new Uri("https://raw.githubusercontent.com/newchild/Rito-Project/master/RitoConnector/Ressources/" + Connection2.getRankedSoloTier().ToLower() + ".png");
                RankedPic.EndInit();
                RankedImage.Source = RankedPic;
                Rankstatus.Text = Connection2.getRankedSoloTier();
                LevelLabel.Visibility = Visibility.Visible;
                UsernameLabel.Visibility = Visibility.Visible;
                Tabs.SelectedIndex = 1;
            }
            else
            {
                MessageBox.Show("An unknown Error has occured. Please try again later");
            }
            
        }
    }
}
