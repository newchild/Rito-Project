using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PortableLeagueAPI;
using PortableLeagueApi.Interfaces.Enums;

namespace RitoConnector
{
	class ItemConverter
	{

		public async static Task<string> getItem(int? id, string region, string key)
		{
			var leagueAPI = new LeagueApi(Keyloader.GetRealKey(), RegionEnum.Euw, true);
			int itemID = id.Value;

			var item = await leagueAPI.Static.GetItemsAsync(
				itemID,
				ItemDataEnum.All,
				languageCode: LanguageEnum.EnglishUS);
			return item.Name;
		}
	}
}

