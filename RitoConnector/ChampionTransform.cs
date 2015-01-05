using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RitoConnector
{
    class ChampionTransform
    {
        public static string GetChampName(int champId)
        {
            string jsonraw;
            WebResponse response;
            string uri = "https://global.api.pvp.net/api/lol/static-data/euw/v1.2/champion/" + champId.ToString() + "?api_key=" + Keyloader.GetRealKey();
            
            WebRequest connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch (WebException e)
            {
                System.Windows.MessageBox.Show(e.Message);
                System.Windows.MessageBox.Show(uri);
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
