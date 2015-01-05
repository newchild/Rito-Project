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
        private MultiIDclass Users;
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
                   return;
            }
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }
            var test = ids.Split(',');
            foreach (string id in test)
            {
                JSONRAW.Replace(id, "user");
            }
            MultiIDclass Users = JsonConvert.DeserializeObject<MultiIDclass>(JSONRAW);
        }
        public MultiIDclass getUserDTOs()
        {
            return Users;
        }
    }
}
