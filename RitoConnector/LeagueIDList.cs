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
			n_League++;
			IDList += ID + ",";
		}

		public string returnLeagueIDList()
		{
			return IDList;
		}

		public bool isOver20()
		{
			if (n_League > 20)
			{
				return true;
			}
			return false;
		}
	}
}
