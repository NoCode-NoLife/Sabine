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
			PlayerCharacter character;

			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `characters` WHERE `accountId` = @accountId AND `characterId` = @characterId", conn))
			{
				cmd.AddParameter("@accountId", account.Id);
				cmd.AddParameter("@characterId", characterId);

				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
						return null;

					character = new PlayerCharacter();

					character.Id = reader.GetInt32("characterId");
					character.Name = reader.GetStringSafe("name");
					character.JobId = (JobId)reader.GetInt32("job");
					character.MapId = reader.GetInt32("mapId");
					character.HairId = reader.GetInt32("hair");
					character.WeaponId = reader.GetInt32("weapon");
					character.Parameters.Zeny = reader.GetInt32("zeny");
					character.Parameters.Speed = reader.GetInt32("speed");
					character.Parameters.BaseLevel = reader.GetInt32("baseLevel");
					character.Parameters.JobLevel = reader.GetInt32("jobLevel");
					character.Parameters.BaseExp = reader.GetInt32("baseExp");
					character.Parameters.JobExp = reader.GetInt32("jobExp");
					character.Parameters.Hp = reader.GetInt32("hp");
					character.Parameters.HpMax = reader.GetInt32("hpMax");
					character.Parameters.Sp = reader.GetInt32("sp");
					character.Parameters.SpMax = reader.GetInt32("spMax");
					character.Parameters.Str = reader.GetInt32("str");
					character.Parameters.Agi = reader.GetInt32("agi");
					character.Parameters.Vit = reader.GetInt32("vit");
					character.Parameters.Int = reader.GetInt32("int");
					character.Parameters.Dex = reader.GetInt32("dex");
					character.Parameters.Luk = reader.GetInt32("luk");
					character.Parameters.StatPoints = reader.GetInt32("statPoints");
					character.Parameters.Weight = reader.GetInt32("weight");
					character.Parameters.WeightMax = reader.GetInt32("weightMax");

					var x = reader.GetInt32("x");
					var y = reader.GetInt32("y");
					character.Position = new Position(x, y);
				}
			}

			character.Vars.Perm.Load(this.GetVars("vars_character", character.Id));

			return character;
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
				cmd.Set("mapId", character.MapId);
				cmd.Set("x", character.Position.X);
				cmd.Set("y", character.Position.Y);
				cmd.Set("hair", character.HairId);
				cmd.Set("weapon", character.WeaponId);
				cmd.Set("zeny", character.Parameters.Zeny);
				cmd.Set("speed", character.Parameters.Speed);
				cmd.Set("baseLevel", character.Parameters.BaseLevel);
				cmd.Set("jobLevel", character.Parameters.JobLevel);
				cmd.Set("baseExp", character.Parameters.BaseExp);
				cmd.Set("jobExp", character.Parameters.JobExp);
				cmd.Set("hp", character.Parameters.Hp);
				cmd.Set("hpMax", character.Parameters.HpMax);
				cmd.Set("sp", character.Parameters.Sp);
				cmd.Set("spMax", character.Parameters.SpMax);
				cmd.Set("str", character.Parameters.Str);
				cmd.Set("agi", character.Parameters.Agi);
				cmd.Set("vit", character.Parameters.Vit);
				cmd.Set("int", character.Parameters.Int);
				cmd.Set("dex", character.Parameters.Dex);
				cmd.Set("luk", character.Parameters.Luk);
				cmd.Set("statPoints", character.Parameters.StatPoints);
				cmd.Set("weight", character.Parameters.Weight);
				cmd.Set("weightMax", character.Parameters.WeightMax);

				cmd.Execute();
			}

			this.SaveVars("vars_character", character.Id, character.Vars.Perm.GetList());
		}
	}
}
