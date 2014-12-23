﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RitoConnector
{
    class RankedHandler
    {
        private RankedDTO rankedStatus;
        public RankedHandler(int userid, string Region, string key)
        {
            string JSONRAW;
            WebResponse Response;
            string URI = "https://" + Region.ToLower() + ".api.pvp.net/api/lol/" + Region.ToLower() + "/v2.5/league/by-summoner/" + userid + "?api_key=" + key;
            WebRequest ConnectionListener = WebRequest.Create(URI);
            ConnectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                Response = ConnectionListener.GetResponse();
            }
            catch(WebException e){
                   System.Windows.MessageBox.Show(e.Message);
                   Response = null;
                   rankedStatus = null;
                   return;
            }
            using (var sr = new StreamReader(Response.GetResponseStream()))
            {
                JSONRAW = sr.ReadToEnd();
            }
            var tempjson = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(JSONRAW);
            JSONRAW = tempjson[userid.ToString()].ToString();
            rankedStatus = JsonConvert.DeserializeObject<RankedDTO>(JSONRAW);
        }
        public string getRankedTier()
        {
            foreach (RankedID rank in rankedStatus.RankedID)
            {
                MessageBox.Show(rank.Tier);
            }
           return rankedStatus.RankedID.FirstOrDefault().Tier;
        }
    }
}