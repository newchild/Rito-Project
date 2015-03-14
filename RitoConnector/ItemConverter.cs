using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace RitoConnector
{
    internal class ItemConverter
    {
        public static string GetItemName(string itemId, string region, string key)
        {
            if (itemId == string.Empty || itemId == "0")
            {
                return "None";
            }

            string jsonraw;
            WebResponse response;
            var uri = "https://global.api.pvp.net/api/lol/static-data/" + region.ToLower() + "/v1.2/item/" + itemId +
                      "?itemData=all&api_key=" + key;
            var connectionListener = WebRequest.Create(uri);
            connectionListener.ContentType = "application/json; charset=utf-8";
            try
            {
                response = connectionListener.GetResponse();
            }
            catch (WebException)
            {
                return "Error";
            }
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonraw = sr.ReadToEnd();
            }

            var tempjson = JsonConvert.DeserializeObject<Items>(jsonraw);
            return tempjson.Name;
        }
    }
}