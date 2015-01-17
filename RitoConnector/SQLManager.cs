using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace RitoConnector
{
	class SqlManager
	{
		private static readonly string Databasefile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LoLStats\database.sqlite";

		private static readonly SQLiteConnection DbConnect = new SQLiteConnection("data source=" + Databasefile);
		private static readonly SQLiteCommand DbCommand = new SQLiteCommand(DbConnect);

		public SqlManager()
		{
			//Creates new DatabaseFile is none is present
			if (!File.Exists(Databasefile))
			{
				SQLiteConnection.CreateFile(Databasefile);
			}

			//Creates new Table if none is existing
			var createTableQuery = @"CREATE TABLE IF NOT EXISTS [Summoner] (
										[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
										[Region] TINYTEXT NULL,
										[Name] TINYTEXT NULL,
										[RealName] TINYTEXT NULL,
										[Level] TINYINT NULL,
										[ProfileIconID] SMALLINT NULL,
										[Tier] TINYTEXT NULL,
										[Division] TINYTEXT NULL,
										[LeagueName] TINYTEXT NULL,
										[LeaguePoints] TINYINT NULL,
										[Miniseries] TINYTEXT NULL,
										[LastUpdate] DATETIME NULL
										)";
			DbConnect.Open();		//Starts Connection

			DbCommand.CommandText = createTableQuery;     // Create Tables
			DbCommand.ExecuteNonQuery();                  // Execute the query
		}

		public static void ResetDb()
		{
			if (File.Exists(Databasefile))
			{
				File.Delete(Databasefile);
			}
		}

		public bool UserInDatabase(string name, string region)
		{
			var userInDatabase = false;
			DbCommand.CommandText =		@"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				userInDatabase = true;
			}
			
			if (userInDatabase)
			{
				DateTime LastUpdate = Convert.ToDateTime(dbreader["LastUpdate"]);
				if (LastUpdate.AddMinutes(30) < DateTime.Now)
				{
					userInDatabase = false;
					dbreader.Close();
					DbCommand.CommandText = @"DELETE
											FROM Summoner
											WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
					DbCommand.ExecuteNonQuery();
				}
			}
			if (!dbreader.IsClosed)
			{
				dbreader.Close();
			}
			return userInDatabase;
		}

		public void InsertUserinDatabase(int id, string region, string name, string realName, int level, int profileIconId)
		{
			DbCommand.CommandText = @"INSERT INTO Summoner (ID , Region, Name, RealName, Level, ProfileIconID, LastUpdate)
									VALUES ('" + id + "','" + region + "','" + name.ToLower() + "','" + realName + "','" + level + "','" + profileIconId + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
			DbCommand.ExecuteNonQuery();
		}

		public void UpdateRank(string name, string region, string tier, string division, string leagueName, int? leaguepoints, string miniseries)
		{
			leagueName = leagueName.Replace("\'","\'\'");
			DbCommand.CommandText = @"UPDATE Summoner
									SET TIER ='" + tier + "', Division = '" + division + "', LeagueName = '" + leagueName + "', LeaguePoints = '" + leaguepoints + "', Miniseries = '" + miniseries + "' WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
		}

		public void CloseConnection()
		{
			DbConnect.Close();
		}

		public int GetUserId(string name, string region)
		{
			var userId = -1;
			DbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				userId = Convert.ToInt32(dbreader["ID"]);
			}
			dbreader.Close();
			return userId;
		}
		
		public string GetName(string name, string region)
		{
			var Name = "";
			DbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				Name = "" + dbreader["realName"];
			}
			dbreader.Close();
			return Name;
		}
		
		public int GetProfileIconId(string name, string region)
		{
			var profileIconId = -1;
			DbCommand.CommandText =		@"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region +"'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				profileIconId = Convert.ToInt32(dbreader["ProfileIconID"]);
			}
			dbreader.Close();
			return profileIconId;
		}

		public int GetLevel(string name, string region)
		{
			var level = -1;
			DbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				level = Convert.ToInt32(dbreader["Level"]);
			}
			dbreader.Close();
			return level;
		}

		public int GetLeaguePoints(string name, string region)
		{
			var lp = -1;
			DbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				lp = Convert.ToInt32(dbreader["LeaguePoints"]);
			}
			dbreader.Close();
			return lp;
		}

		public string GetSoloTier(string name, string region)
		{
			var tier = "";
			DbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				tier = "" + dbreader["Tier"];
			}
			dbreader.Close();
			return tier;
		}

		public string GetSoloDivision(string name, string region)
		{
			var division = "";
			DbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				division = "" + dbreader["Division"];
			}
			dbreader.Close();
			return division;
		}

		public int GetLeagueMemberCount(string name, string region)
		{
			var division = "";
			var leagueName = "";
			var tier = "";
			DbCommand.CommandText = @"SELECT *
										FROM Summoner
										WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				division = "" + dbreader["Division"];
				tier = "" + dbreader["Tier"];
				leagueName = "" + dbreader["LeagueName"];
				leagueName = leagueName.Replace("\'", "\'\'");
			}
			dbreader.Close();
			DbCommand.CommandText =	@"SELECT COUNT(*)
									FROM Summoner
									WHERE LeagueName = '" + leagueName + "' AND Division = '" + division + "' AND Tier = '" + tier + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader2 = DbCommand.ExecuteScalar();
			string countAsString = dbreader2.ToString();
			int count = Convert.ToInt32(countAsString);
			return count;
		}

		public string GetMiniseries(string name, string region)
		{
			var Miniseries = "";
			DbCommand.CommandText =	@"SELECT *
									FROM Summoner
									WHERE Name = '" + name.ToLower() + "' AND Region = '" + region + "'";
			DbCommand.ExecuteNonQuery();
			var dbreader = DbCommand.ExecuteReader();
			if (dbreader.Read())
			{
				Miniseries = "" + dbreader["Miniseries"];
			}
			dbreader.Close();
			return Miniseries;
		}
	}
}


 