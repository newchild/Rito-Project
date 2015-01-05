using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RitoConnector
{
    class MultipleIDGrabber
    {
        private SummonerDTO[] Users;
        public MultipleIDGrabber(string ids,string Region, string key)
        {
            string JSONRAW;
            WebResponse Response;
            string URI = "https://" + Region.ToLower() + ".api.pvp.net/api/lol/" + Region.ToLower() + "/v1.4/summoner/" + ids + "?api_key=" + key;
            WebRequest ConnectionListener = WebRequest.Create(URI);
            ConnectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                Response = ConnectionListener.GetResponse();
            }
            catch(WebException e){
                   System.Windows.MessageBox.Show(e.Message);
                   Response = null;
                   Users = null;
                   return;
            }
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }
            var userids = ids.Split(',');
            int usercounter = 0;
            foreach (string user in userids)
            {
                usercounter++;
            }
            Users = new SummonerDTO[usercounter];
            usercounter = 0;
            foreach (string user in userids)
            {
				MessageBox.Show(user);
                var tempjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(JSONRAW);
                var CleanSummonerJSON = tempjson[user].ToString();
                var User = JsonConvert.DeserializeObject<SummonerDTO>(CleanSummonerJSON);
                Users[usercounter] = User;
                usercounter++;
            }
            
        }
        public SummonerDTO[] getUserDTOs()
        {
            return Users;
        }
    }
}
