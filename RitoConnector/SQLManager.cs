using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace RitoConnector
{
	class SQLManager
	{
		private string databasefile = "database.sqlite";
		
		public SQLManager()
		{
			if (!File.Exists(databasefile))
			{
				SQLiteConnection.CreateFile(databasefile);
			}
		}
	}
}
