using MySql.Data.MySqlClient;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Database.MySQL;
using Sabine.Shared.World;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.Database
{
	/// <summary>
	/// Zone server's database interface.
	/// </summary>
	public class ZoneDb : Db
	{
		/// <summary>
		/// Returns a list with all characters on the given account.
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public PlayerCharacter GetCharacter(Account account, int characterId)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `characters` WHERE `accountId` = @accountId AND `characterId` = @characterId", conn))
			{
				cmd.AddParameter("@accountId", account.Id);
				cmd.AddParameter("@characterId", characterId);

				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
						return null;

					var character = new PlayerCharacter();

					character.Id = reader.GetInt32("characterId");
					character.Name = reader.GetStringSafe("name");
					character.JobId = (JobId)reader.GetInt32("job");
					character.Zeny = reader.GetInt32("zeny");
					character.MapName = reader.GetStringSafe("mapName");
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

					var x = reader.GetInt32("x");
					var y = reader.GetInt32("y");
					character.Position = new Position(x, y);

					return character;
				}
			}
		}

		/// <summary>
		/// Saves character to database.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="character"></param>
		public void SaveCharacter(Account account, PlayerCharacter character)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `accountId` = @accountId AND `characterId` = @characterId", conn))
			{
				cmd.AddParameter("@accountId", account.Id);
				cmd.AddParameter("@characterId", character.Id);

				cmd.Set("name", character.Name);
				cmd.Set("job", character.JobId);
				cmd.Set("zeny", character.Zeny);
				cmd.Set("mapName", character.MapName);
				cmd.Set("x", character.Position.X);
				cmd.Set("y", character.Position.Y);
				cmd.Set("speed", character.Speed);
				cmd.Set("baseLevel", character.BaseLevel);
				cmd.Set("jobLevel", character.JobLevel);
				cmd.Set("baseExp", character.BaseExp);
				cmd.Set("jobExp", character.JobExp);
				cmd.Set("hp", character.Hp);
				cmd.Set("hpMax", character.HpMax);
				cmd.Set("sp", character.Sp);
				cmd.Set("spMax", character.SpMax);
				cmd.Set("str", character.Str);
				cmd.Set("agi", character.Agi);
				cmd.Set("vit", character.Vit);
				cmd.Set("int", character.Int);
				cmd.Set("dex", character.Dex);
				cmd.Set("luk", character.Luk);
				cmd.Set("statPoints", character.StatPoints);
				cmd.Set("hair", character.HairId);
				cmd.Set("weapon", character.WeaponId);

				cmd.Execute();
			}
		}
	}
}
