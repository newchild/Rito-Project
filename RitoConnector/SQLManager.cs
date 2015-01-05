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
	class SqlManager
	{
		private static string _databasefile = "database.sqlite";

		private static SQLiteConnection _dbConnect = new SQLiteConnection("data source=" + _databasefile);
		private static SQLiteCommand _dbCommand = new SQLiteCommand(_dbConnect);

		public SqlManager()
		{
			//Creates new DatabaseFile is none is present
			if (!File.Exists(_databasefile))
			{
				SQLiteConnection.CreateFile(_databasefile);
			}

			

			//Creates new Table if none is existing
			string createTableQuery = @"CREATE TABLE IF NOT EXISTS [Summoner] (
										[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
										[Region] TINYTEXT NULL,
										[Name] TINYTEXT NULL,
										[RealName] TINYTEXT NULL,
										[Level] TINYINT NULL,
										[ProfileIconID] SMALLINT NULL,
										[Tier] TINYTEXT NULL,
										[Division] TINYTEXT NULL,
										[LeagueName] TINYTEXT NULL,
										[LastUpdate] DATETIME NULL
										)";
			_dbConnect.Open();		//Starts Connection

			_dbCommand.CommandText = createTableQuery;     // Create Tables
			_dbCommand.ExecuteNonQuery();                  // Execute the query
		}

		public static void ResetDb()
		{
			if (File.Exists(_databasefile))
			{
				File.Delete(_databasefile);
			}
		}

		public bool UserInDatabase(string name, string region)
		{
			bool userInDatabase = false;
			_dbCommand.CommandText =		@"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			_dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = _dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				userInDatabase = true;
			}
			dbreader.Close();
			return userInDatabase;
		}

		public void InsertUserinDatabase(int id, string region, string name, string realName, int level, int profileIconId)
		{
			_dbCommand.CommandText = @"INSERT INTO Summoner (ID , Region, Name, RealName, Level, ProfileIconID, LastUpdate)
									VALUES ('" + id + "','" + region + "','" + name.ToLower() + "','" + realName + "','" + level + "','" + profileIconId + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
			_dbCommand.ExecuteNonQuery();
		}

		public void UpdateRank(string name, string region, string tier, string division, string leagueName)
		{
			_dbCommand.CommandText = @"UPDATE Summoner
									SET TIER ='" + tier + "', Division = '" + division + "', LeagueName = '" + leagueName + "' WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			_dbCommand.ExecuteNonQuery();
		}

		public void CloseConnection()
		{
			_dbConnect.Close();
		}

		public int GetUserId(string name, string region)
		{
			int userId = -1;
			_dbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			_dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = _dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				userId = Convert.ToInt32(dbreader["ID"]);
			}
			dbreader.Close();
			return userId;
		}
		
		public string GetName(string name, string region)
		{
			string Name = "";
			_dbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			_dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = _dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				Name = "" + dbreader["realName"];
			}
			dbreader.Close();
			return Name;
		}
		
		public int GetProfileIconId(string name, string region)
		{
			int profileIconId = -1;
			_dbCommand.CommandText =		@"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region +"'";
			_dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = _dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				profileIconId = Convert.ToInt32(dbreader["ProfileIconID"]);
			}
			dbreader.Close();
			return profileIconId;
		}

		public int GetLevel(string name, string region)
		{
			int level = -1;
			_dbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			_dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = _dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				level = Convert.ToInt32(dbreader["Level"]);
			}
			dbreader.Close();
			return level;
		}

		public string GetSoloTier(string name, string region)
		{
			string tier = "";
			_dbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			_dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = _dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				tier = "" + dbreader["Tier"];
			}
			dbreader.Close();
			return tier;
		}

		public string GetSoloDivision(string name, string region)
		{
			string division = "";
			_dbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			_dbCommand.ExecuteNonQuery();
			SQLiteDataReader dbreader = _dbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				division = "" + dbreader["Division"];
			}
			dbreader.Close();
			return division;
		}
	}
}
