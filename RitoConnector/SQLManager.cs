using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Windows;

namespace RitoConnector
{
	class SQLManager
	{
		private static string databasefile = "database.sqlite";

		private static SQLiteConnection dbConnect = new SQLiteConnection("data source=" + databasefile);
		private static SQLiteCommand dbCommand = new SQLiteCommand(dbConnect);

		public SQLManager()
		{
			//Creates new DatabaseFile is none is present
			if (!File.Exists(databasefile))
			{
				SQLiteConnection.CreateFile(databasefile);
			}

			

			//Creates new Table if none is existing
			string createTableQuery = @"CREATE TABLE IF NOT EXISTS [Summoner] (
										[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
										[Region] TINYTEXT NULL,
										[Name] TINYTEXT NULL,
										[RealName] TINYTEXT NULL,
										[Level] TINYINT NULL,
										[ProfileIconID] SMALLINT NULL,
										[LastUpdate] DATETIME NULL
										)";
			dbConnect.Open();		//Starts Connection

			dbCommand.CommandText = createTableQuery;     // Create Tables
			dbCommand.ExecuteNonQuery();                  // Execute the query
		}

		public bool userInDatabase(string name)
		{
			bool userInDatabase;
			dbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "'";
			dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				userInDatabase = true;
			}
			else
			{
				userInDatabase = false;
			}
			dbreader.Close();
			return userInDatabase;
		}
		public void insertUserinDatabase(int ID, string region, string name, string realName, int Level, int ProfileIconID)
		{
			dbCommand.CommandText = @"INSERT INTO Summoner
									VALUES ('" + ID + "','" + region + "','" + name.ToLower() + "','" + realName + "','" + Level + "','" + ProfileIconID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
			dbCommand.ExecuteNonQuery();
		}

		public void closeConnection()
		{
			dbConnect.Close();
		}

		public string GetName(string name, string region)
		{
			string Name = "";
			dbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				Name = "" + dbreader["realName"];
			}
			dbreader.Close();
			return Name;
		}
		
		public int GetProfileIconID(string name, string region)
		{
			int ProfileIconID = -1;
			dbCommand.CommandText =		@"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region +"'";
			dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				ProfileIconID = Convert.ToInt32(dbreader["ProfileIconID"]);
			}
			dbreader.Close();
			return ProfileIconID;
		}

		public int GetLevel(string name, string region)
		{
			int Level = -1;
			dbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				Level = Convert.ToInt32(dbreader["Level"]);
			}
			dbreader.Close();
			return Level;
		}
	}
}
