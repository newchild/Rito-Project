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
        public static string getChampName(int ChampID)
        {
            string JSONRAW;
            WebResponse Response;
            string URI = "https://global.api.pvp.net/api/lol/static-data/euw/v1.2/champion/" + ChampID.ToString() + "?api_key=" + Keyloader.getRealKey();
            
            WebRequest ConnectionListener = WebRequest.Create(URI);
            ConnectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                Response = ConnectionListener.GetResponse();
            }
            catch (WebException e)
            {
                System.Windows.MessageBox.Show(e.Message);
                System.Windows.MessageBox.Show(URI);
                Response = null;
            }
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }

            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSONRAW);
            return tempjson["name"].ToString();
        }
    }
}
