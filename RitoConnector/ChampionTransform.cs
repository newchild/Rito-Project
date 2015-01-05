using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

namespace RitoConnector
{
    class ChampionTransform
    {
        public static string GetChampName(int champId)
        {
            string jsonraw;
            WebResponse response;
            var uri = "https://global.api.pvp.net/api/lol/static-data/euw/v1.2/champion/" + champId.ToString() + "?api_key=" + Keyloader.GetRealKey();
            
            var connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch (WebException e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show(uri);
                response = null;
            }
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }

            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonraw);
            return tempjson["name"].ToString();
        }
    }
}
