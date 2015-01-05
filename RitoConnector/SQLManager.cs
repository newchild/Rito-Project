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
										[Name] TINYTEXT NULL,
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
		public void insertUserinDatabase(int ID, string name, int Level, int ProfileIconID)
		{
			dbCommand.CommandText = @"INSERT INTO Summoner
									VALUES ('" + ID + "','" + name.ToLower() + "','" + Level + "','" + ProfileIconID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
			dbCommand.ExecuteNonQuery();
		}

		public void closeConnection()
		{
			dbConnect.Close();
		}
	}
}
