using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Net;
using System.Media;
using System.Windows.Media.Imaging;

namespace RitoConnector
{
	class CacheManager
	{
		public CacheManager()
		{

		}

		public BitmapImage ProfileIcon(int ProfileIconID)
		{
			BitmapImage logo = new BitmapImage();
			logo.BeginInit();
			if (!Directory.Exists("./resources"))
			{
				Directory.CreateDirectory("./resources");
			}
			if (!Directory.Exists("./resources/ProfileIcons"))
			{
				Directory.CreateDirectory("./resources/ProfileIcons");
			}
			if (!File.Exists(@"./resources/ProfileIcons/" + ProfileIconID + ".png"))
			{
				try
				{
					byte[] data;
					using (WebClient webclient = new WebClient())
					{
						data = webclient.DownloadData("http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + ProfileIconID + ".png");
					}
					File.WriteAllBytes(@"./resources/ProfileIcons/" + ProfileIconID + ".png", data);
				}
				catch (WebException e)
				{
					System.Windows.MessageBox.Show(e.Message);
				}
			}
			logo.StreamSource = new FileStream(@"./resources/ProfileIcons/" + ProfileIconID + ".png", FileMode.Open, FileAccess.Read);
			logo.EndInit();
			return logo;
		}

		public BitmapImage RankedIcon(string Tier, string Division)
		{
            if (Tier == "")
            {
                Tier = "Unranked";
            }
			BitmapImage logo = new BitmapImage();
			logo.BeginInit();
			if (!Directory.Exists("./resources"))
			{
				Directory.CreateDirectory("./resources");
				
			}
			if (!Directory.Exists("./resources/RankedIcons"))
			{
				Directory.CreateDirectory("./resources/RankedIcons");
			}
			if (!File.Exists(@"./resources/RankedIcons/" + Tier + "_" + Division + ".png"))
			{
				try
				{
					byte[] data;
					using (WebClient webclient = new WebClient())
					{
						data = webclient.DownloadData("https://raw.githubusercontent.com/newchild/Rito-Project/master/RitoConnector/Ressources/" + Tier + "_" + Division + ".png");
					}
					File.WriteAllBytes(@"./resources/RankedIcons/" + Tier + "_" + Division + ".png", data);
				}
				catch (WebException e)
				{
					System.Windows.MessageBox.Show(e.Message);
				}
			}
			logo.StreamSource = new FileStream(@"./resources/RankedIcons/" + Tier + "_" + Division + ".png", FileMode.Open, FileAccess.Read);
			logo.EndInit();
			return logo;
		}
	}
}
