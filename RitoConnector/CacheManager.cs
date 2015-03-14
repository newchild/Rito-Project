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
            if (!Directory.Exists(RoamingFolder))
            {
                Directory.CreateDirectory(RoamingFolder);
            }

            if (!Directory.Exists(ResourcesFolder))
            {
                Directory.CreateDirectory(ResourcesFolder);
            }

            if (!Directory.Exists(ResourcesFolder + "/ProfileIcons"))
            {
                Directory.CreateDirectory(ResourcesFolder + "/ProfileIcons");
            }

            if (!Directory.Exists(ResourcesFolder + "/RankedIcons"))
            {
                Directory.CreateDirectory(ResourcesFolder + "/RankedIcons");
            }

            if (!Directory.Exists(RoamingFolder + "/jsonraw"))
            {
                Directory.CreateDirectory(RoamingFolder + "/jsonraw");
            }
        }

        public static void ResetCache()
        {
            if (Directory.Exists(ResourcesFolder))
            {
                Directory.Delete(ResourcesFolder, true);
            }
        }

        public static void SaveJson(string filename, string rawjson)
        {
            // Summoner JSON has Form Summoner-SummonerName
            // Ranked JSON has Form Ranked-SummonerName
            File.WriteAllText(ResourcesFolder + "/" + filename + ".json", rawjson);
        }

        public static string GetJson(string filename)
        {
            return File.ReadAllText(ResourcesFolder + "/" + filename + ".json");
        }

        public static string GetRessources()
        {
            return ResourcesFolder + "/";
        }

        public BitmapImage ProfileIcon(int profileIconId)
        {
            var logo = new BitmapImage();
            logo.BeginInit();
            if (!File.Exists(ResourcesFolder + "/ProfileIcons/" + profileIconId + ".png"))
            {
                try
                {
                    byte[] data;
                    using (var webclient = new WebClient())
                    {
                        data =
                            webclient.DownloadData("http://ddragon.leagueoflegends.com/cdn/4.21.5/img/profileicon/" +
                                                   profileIconId + ".png");
                    }

                    File.WriteAllBytes(ResourcesFolder + "/ProfileIcons/" + profileIconId + ".png", data);
                }
                catch (WebException e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            logo.StreamSource = new FileStream(ResourcesFolder + "/ProfileIcons/" + profileIconId + ".png",
                FileMode.Open, FileAccess.Read);
            logo.EndInit();
            return logo;
        }

        private static readonly string RoamingFolder =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LoLStats";

        private static readonly string ResourcesFolder = RoamingFolder + @"/resources";
    }
}