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
	class ItemConverter
	{

		public static string getItemName(string ItemID,string region, string key)
        {
            if (ItemID == string.Empty || ItemID == "0")
                return "None";
	        string jsonraw;
            WebResponse response;
            var uri = "https://global.api.pvp.net/api/lol/static-data/" + region.ToLower() + "/v1.2/item/" + ItemID + "?itemData=all&api_key=" + key;
            var connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch(WebException e){
                   return "Error";
            }
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }
            var tempjson = JsonConvert.DeserializeObject<ItemConverterDTO.Items>(jsonraw);
            return tempjson.Name;
        }
	}
}

