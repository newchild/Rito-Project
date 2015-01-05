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
    class MultipleIdGrabber
    {
        SummonerDto[] _tests;
        public MultipleIdGrabber(string ids,string region, string key)
        {
            string jsonraw;
            WebResponse response;
            string uri = "https://" + region.ToLower() + ".api.pvp.net/api/lol/" + region.ToLower() + "/v1.4/summoner/" + ids + "?api_key=" + key;
            WebRequest connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch(WebException e){
                   System.Windows.MessageBox.Show(e.Message);
                   response = null;
                   return;
            }
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }
            var test = ids.Split(',');
            foreach (string id in test)
            {
                if (id != "")
                {
                    jsonraw = jsonraw.Replace("\"" + id + "\"", "user");
                }
                
            }
            int testcounter = 0;
            foreach (string id in test)
            {
                testcounter++;
            }

            string[] stringSeparators = new string[] { "user" };
            var jsons = jsonraw.Split(stringSeparators,StringSplitOptions.None);
            testcounter = 0;
            foreach (var jstring in jsons)
            {
                
                testcounter++;
            }
            _tests = new SummonerDto[testcounter-1];
            testcounter = 0;
            foreach (var jstring in jsons)
            {
                if (jstring == "{" || jstring.Length < 4)
                {

                }
                else
                {
					var jstringlegit = jstring.Substring(1, jstring.Length - 2);
                    MessageBox.Show(jstringlegit);
                    SummonerDto users = JsonConvert.DeserializeObject<SummonerDto>(jstringlegit);
                    _tests[testcounter] = users;
                    testcounter++;
                }
                
            }
        }
        public SummonerDto[] GetUserDtOs()
        {
            return _tests;
        }
    }
}
