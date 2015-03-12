using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RitoConnector
{
    internal class CacheManager
    {
        public static void PrepareRoaming()
        {
            if (!Directory.Exists(_roamingFolder))
            {
                Directory.CreateDirectory(_roamingFolder);
            }
            if (!Directory.Exists(_resourcesFolder))
            {
                Directory.CreateDirectory(_resourcesFolder);
            }
            if (!Directory.Exists(_resourcesFolder + "/ProfileIcons"))
            {
                Directory.CreateDirectory(_resourcesFolder + "/ProfileIcons");
            }
            if (!Directory.Exists(_resourcesFolder + "/RankedIcons"))
            {
                Directory.CreateDirectory(_resourcesFolder + "/RankedIcons");
            }
            if (!Directory.Exists(_roamingFolder + "/jsonraw"))
            {
                Directory.CreateDirectory(_roamingFolder + "/jsonraw");
            }
        }

        public static void ResetCache()
        {
            if (Directory.Exists(_resourcesFolder))
            {
                Directory.Delete(_resourcesFolder, true);
            }
        }

        public static void SaveJson(string filename, string rawjson)
        {
            // Summoner JSON has Form Summoner-SummonerName
            // Ranked JSON has Form Ranked-SummonerName
            File.WriteAllText(_resourcesFolder + "/" + filename + ".json", rawjson);
        }

        public static string GetJson(string filename)
        {
            return File.ReadAllText(_resourcesFolder + "/" + filename + ".json");
        }

        public static string GetRessources()
        {
            return _resourcesFolder + "/";
        }

        public BitmapImage ProfileIcon(int profileIconId)
        {
            var logo = new BitmapImage();
            logo.BeginInit();
            if (!File.Exists(_resourcesFolder + "/ProfileIcons/" + profileIconId + ".png"))
            {
                try
                {
                    byte[] data;
                    using (var webclient = new WebClient())
                    {
                        data =
                            webclient.DownloadData(
                                "http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" + profileIconId +
                                ".png");
                    }
                    File.WriteAllBytes(_resourcesFolder + "/ProfileIcons/" + profileIconId + ".png", data);
                }
                catch (WebException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            logo.StreamSource = new FileStream(
                _resourcesFolder + "/ProfileIcons/" + profileIconId + ".png", FileMode.Open, FileAccess.Read);
            logo.EndInit();
            return logo;
        }

        public BitmapImage RankedIcon(string tier, string division)
        {
            var logo = new BitmapImage();
            logo.BeginInit();
            if (!File.Exists(_resourcesFolder + "/RankedIcons/" + tier + "_" + division + ".png"))
            {
                try
                {
                    byte[] data;
                    using (var webclient = new WebClient())
                    {
                        data =
                            webclient.DownloadData(
                                "https://raw.githubusercontent.com/newchild/Rito-Project/master/RitoConnector/Ressources/" +
                                tier + "_" + division + ".png");
                    }
                    File.WriteAllBytes(_resourcesFolder + "/RankedIcons/" + tier + "_" + division + ".png", data);
                }
                catch (WebException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            logo.StreamSource = new FileStream(
                _resourcesFolder + "/RankedIcons/" + tier + "_" + division + ".png", FileMode.Open, FileAccess.Read);
            logo.EndInit();
            return logo;
        }

        private static readonly string _roamingFolder =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LoLStats";

        private static readonly string _resourcesFolder = _roamingFolder + @"/resources";
    }
}