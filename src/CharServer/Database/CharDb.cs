using System;
using System.Collections.Generic;
using MySqlConnector;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Database.MySQL;
using Sabine.Shared.World;

namespace Sabine.Char.Database
{
	/// <summary>
	/// Char server's database interface.
	/// </summary>
	public class CharDb : Db
	{
		/// <summary>
		/// Returns a list with all characters on the given account.
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public List<Character> GetCharacters(Account account)
		{
			var result = new List<Character>();

			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `characters` WHERE `accountId` = @accountId", conn))
			{
				cmd.AddParameter("@accountId", account.Id);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var character = new Character();

						character.Id = reader.GetInt32("characterId");
						character.Slot = reader.GetInt32("slot");
						character.Name = reader.GetStringSafe("name");
						character.JobId = (JobId)reader.GetInt32("job");
						character.Zeny = reader.GetInt32("zeny");
						character.Speed = reader.GetInt32("speed");
						character.BaseLevel = reader.GetInt32("baseLevel");
						character.JobLevel = reader.GetInt32("jobLevel");
						character.BaseExp = reader.GetInt32("baseExp");
						character.JobExp = reader.GetInt32("jobExp");
						character.Hp = reader.GetInt32("hp");
						character.HpMax = reader.GetInt32("hpMax");
						character.Sp = reader.GetInt32("sp");
						character.SpMax = reader.GetInt32("spMax");
						character.Str = reader.GetInt32("str");
						character.Agi = reader.GetInt32("agi");
						character.Vit = reader.GetInt32("vit");
						character.Int = reader.GetInt32("int");
						character.Dex = reader.GetInt32("dex");
						character.Luk = reader.GetInt32("luk");
						character.StatPoints = reader.GetInt32("statPoints");
						character.HairId = reader.GetInt32("hair");
						character.WeaponId = reader.GetInt32("weapon");

						var mapId = reader.GetInt32("mapId");
						var x = reader.GetInt32("x");
						var y = reader.GetInt32("y");
						character.Location = new Location(mapId, x, y);

						result.Add(character);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Returns true if a character with the given name exists.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool CharacterNameExists(string name)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `characters` WHERE `name` = @name", conn))
			{
				cmd.AddParameter("@name", name);

				using (var reader = cmd.ExecuteReader())
					return reader.HasRows;
			}
		}

		/// <summary>
		/// Creates character, using the given characters as template
		/// and assigns an id to the object.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="character"></param>
		/// <exception cref="ArgumentException"></exception>
		public void CreateCharacter(Account account, ref Character character)
		{
			if (character.Id != 0)
				throw new ArgumentException("This character appears to have been created already.");

			using (var conn = this.GetConnection())
			using (var cmd = new InsertCommand("INSERT INTO `characters` {0}", conn))
			{
				cmd.Set("accountId", account.Id);
				cmd.Set("slot", character.Slot);
				cmd.Set("name", character.Name);
				cmd.Set("mapId", character.Location.MapId);
				cmd.Set("x", character.Location.Position.X);
				cmd.Set("y", character.Location.Position.Y);
				cmd.Set("hair", character.HairId);
				cmd.Set("str", character.Str);
				cmd.Set("agi", character.Agi);
				cmd.Set("vit", character.Vit);
				cmd.Set("int", character.Int);
				cmd.Set("dex", character.Dex);
				cmd.Set("luk", character.Luk);
				cmd.Set("hp", character.Hp);
				cmd.Set("hpMax", character.Hp);
				cmd.Set("sp", character.Sp);
				cmd.Set("spMax", character.Sp);
				cmd.Set("baseLevel", character.BaseLevel);
				cmd.Set("jobLevel", character.JobLevel);
				cmd.Set("zeny", character.Zeny);

				cmd.Execute();
				character.Id = (int)cmd.LastId;
			}
		}

		/// <summary>
		/// Removes the character with the given id from the database.
		/// Returns false if character id didn't exist and no characters
		/// were deleted.
		/// </summary>
		/// <param name="characterId"></param>
		public bool RemoveCharacter(int characterId)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("DELETE FROM `characters` WHERE `characterId` = @characterId", conn))
			{
				cmd.AddParameter("@characterId", characterId);
				return cmd.ExecuteNonQuery() != 0;
			}
		}

		/// <summary>
		/// Updates character's location in the database.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="mapId"></param>
		/// <param name="pos"></param>
		public void UpdateCharacterLocation(Character character, int mapId, Position pos)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn))
			{
				cmd.AddParameter("@characterId", character.Id);
				cmd.Set("mapId", mapId);
				cmd.Set("x", pos.X);
				cmd.Set("y", pos.Y);

				cmd.Execute();
			}
		}
	}
}
