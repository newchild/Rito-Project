using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitoConnector
{
	class LeagueIDList
	{
		private int n_League = 0;
		private string IDList;

		public LeagueIDList()
		{}

		public void addListItem(string ID)
		{
			IDList += ID + ",";
		}

		public string returnLeagueIDList()
		{
			IDList = IDList.Remove(IDList.Length - 1);
			return IDList;
		}
	}
}
