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
        MultiIDclass[] tests;
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
                if (id != "")
                {
                    JSONRAW = JSONRAW.Replace("\"" + id + "\"", "user");
                }
                
            }
            int testcounter = 0;
            foreach (string id in test)
            {
                testcounter++;
            }

            string[] stringSeparators = new string[] { "user" };
            var JSONS = JSONRAW.Split(stringSeparators,StringSplitOptions.None);
            testcounter = 0;
            foreach (var jstring in JSONS)
            {
                
                testcounter++;
            }
            MultiIDclass[] tests = new MultiIDclass[testcounter];
            testcounter = 0;
            foreach (var jstring in JSONS)
            {
                if (jstring == "{" || jstring.Length < 4)
                {

                }
                else
                {
                    var jstringlegit = "{\"user\"" + jstring.Substring(0,jstring.Length - 1) + "}";
                    MessageBox.Show(jstringlegit);
                    MultiIDclass Users = JsonConvert.DeserializeObject<MultiIDclass>(jstringlegit);

                    tests[testcounter] = Users;
                    testcounter++;
                }
                
            }

            

        }
        public MultiIDclass[] getUserDTOs()
        {
            return tests;
        }
    }
}
