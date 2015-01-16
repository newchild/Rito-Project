using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using System;

namespace RitoConnector
{
	class CacheManager
	{
		private static string roamingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LoLStats";
		private static string resourcesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LoLStats\resources";

		public static void PrepareRoaming()
		{
			if (!Directory.Exists(roamingFolder))
			{
				Directory.CreateDirectory(roamingFolder);
			}
			if (!Directory.Exists(resourcesFolder))
			{
				Directory.CreateDirectory(resourcesFolder);
			}
			if (!Directory.Exists(resourcesFolder + "/ProfileIcons"))
			{
				Directory.CreateDirectory(resourcesFolder + "/ProfileIcons");
			}
			if (!Directory.Exists(resourcesFolder + "/RankedIcons"))
			{
				Directory.CreateDirectory(resourcesFolder + "/RankedIcons");
			}
		}

		public static void ResetCache()
		{
			if (Directory.Exists(resourcesFolder))
			{
				Directory.Delete(resourcesFolder, true);
			}
		}

		public BitmapImage ProfileIcon(int profileIconId)
		{
			var logo = new BitmapImage();
			logo.BeginInit();
			if (!File.Exists(resourcesFolder + "/ProfileIcons/" + profileIconId + ".png"))
			{
				try
				{
					byte[] data;
					using (var webclient = new WebClient())
					{
						data = webclient.DownloadData("http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + profileIconId + ".png");
					}
					File.WriteAllBytes(resourcesFolder +"/ProfileIcons/" + profileIconId + ".png", data);
				}
				catch (WebException e)
				{
					MessageBox.Show(e.Message);
				}
			}
			logo.StreamSource = new FileStream(resourcesFolder +"/ProfileIcons/" + profileIconId + ".png", FileMode.Open, FileAccess.Read);
			logo.EndInit();
			return logo;
		}

		public BitmapImage RankedIcon(string tier, string division)
		{
			var logo = new BitmapImage();
			logo.BeginInit();
			if (!File.Exists(resourcesFolder + "/RankedIcons/" + tier + "_" + division + ".png"))
			{
				try
				{
					byte[] data;
					using (var webclient = new WebClient())
					{
						data = webclient.DownloadData("https://raw.githubusercontent.com/newchild/Rito-Project/master/RitoConnector/Ressources/" + tier + "_" + division + ".png");
					}
					File.WriteAllBytes(resourcesFolder + "/RankedIcons/" + tier + "_" + division + ".png", data);
				}
				catch (WebException e)
				{
					MessageBox.Show(e.Message);
				}
			}
			logo.StreamSource = new FileStream(resourcesFolder + "/RankedIcons/" + tier + "_" + division + ".png", FileMode.Open, FileAccess.Read);
			logo.EndInit();
			return logo;
		}
	}
}
