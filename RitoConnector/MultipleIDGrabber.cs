using System;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

namespace RitoConnector
{
    class MultipleIdGrabber
    {
        SummonerDto[] _tests;
        public MultipleIdGrabber(string ids,string region, string key)
        {
            string jsonraw;
            WebResponse response;
            var uri = "https://" + region.ToLower() + ".api.pvp.net/api/lol/" + region.ToLower() + "/v1.4/summoner/" + ids + "?api_key=" + key;
            var connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch(WebException e){
                   MessageBox.Show(e.Message);
                   response = null;
                   return;
            }
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }
            var test = ids.Split(',');
            foreach (var id in test)
            {
                if (id != "")
                {
                    jsonraw = jsonraw.Replace("\"" + id + "\"", "user");
                }
                
            }
            var testcounter = 0;
            foreach (var id in test)
            {
                testcounter++;
            }

            var stringSeparators = new string[] { "user" };
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
                    var users = JsonConvert.DeserializeObject<SummonerDto>(jstringlegit);
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
