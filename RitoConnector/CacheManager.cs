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

		public static void ResetCache()
		{
			if (Directory.Exists("./resources"))
			{
				Directory.Delete("./resources", true);
			}
		}

		public BitmapImage ProfileIcon(int profileIconId)
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
			if (!File.Exists(@"./resources/ProfileIcons/" + profileIconId + ".png"))
			{
				try
				{
					byte[] data;
					using (WebClient webclient = new WebClient())
					{
						data = webclient.DownloadData("http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + profileIconId + ".png");
					}
					File.WriteAllBytes(@"./resources/ProfileIcons/" + profileIconId + ".png", data);
				}
				catch (WebException e)
				{
					System.Windows.MessageBox.Show(e.Message);
				}
			}
			logo.StreamSource = new FileStream(@"./resources/ProfileIcons/" + profileIconId + ".png", FileMode.Open, FileAccess.Read);
			logo.EndInit();
			return logo;
		}

		public BitmapImage RankedIcon(string tier, string division)
		{
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
			if (!File.Exists(@"./resources/RankedIcons/" + tier + "_" + division + ".png"))
			{
				try
				{
					byte[] data;
					using (WebClient webclient = new WebClient())
					{
						data = webclient.DownloadData("https://raw.githubusercontent.com/newchild/Rito-Project/master/RitoConnector/Ressources/" + tier + "_" + division + ".png");
					}
					File.WriteAllBytes(@"./resources/RankedIcons/" + tier + "_" + division + ".png", data);
				}
				catch (WebException e)
				{
					System.Windows.MessageBox.Show(e.Message);
				}
			}
			logo.StreamSource = new FileStream(@"./resources/RankedIcons/" + tier + "_" + division + ".png", FileMode.Open, FileAccess.Read);
			logo.EndInit();
			return logo;
		}
	}
}
