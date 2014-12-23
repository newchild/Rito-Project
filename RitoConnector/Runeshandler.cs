using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RitoConnector
{
    class Runeshandler
    {
        private RunePagesDto Runes;
        public Runeshandler(int userid, string Region, string key){
            string CleanRunesJSON;
            WebResponse Response;
            string JSONRAW;
            string URI = "https://"+Region+".api.pvp.net/api/lol/ "+ Region + "/v1.4/summoner/" + userid + "/runes?api_key=" + key;
            WebRequest ConnectionListener = WebRequest.Create(URI);
            ConnectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                Response = ConnectionListener.GetResponse();
            }
            catch (WebException e)
            {
                System.Windows.MessageBox.Show(e.Message);
                Response = null;
            }
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }
            var tempjson = JsonConvert.DeserializeObject<Dictionary<int, object>>(JSONRAW);
            CleanRunesJSON = tempjson[userid].ToString();
            Runes = JsonConvert.DeserializeObject<RunePagesDto>(CleanRunesJSON);
        }
        public Page GetRunePageByIndex(int Index)
        {
            return Runes.Pages[Index];
        }
        public int getRunepageCount()
        {
            return Runes.Pages.Count();
        }
        public Page getRunePageByName(string Name){
            foreach(Page runepage in Runes.Pages){
                if(runepage.Name == Name){
                    return runepage;
                }
            }
            return null;
        }
    }
}
