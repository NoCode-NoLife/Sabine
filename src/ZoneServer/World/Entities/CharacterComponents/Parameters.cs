using System;
using Sabine.Shared.Const;
using Sabine.Shared.Util;
using Sabine.Zone.Network;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Entities.CharacterComponents
{
	/// <summary>
	/// Represents a character's parameters (stats, sub-stats, and
	/// anything related).
	/// </summary>
	public class Parameters
	{
		private PlayerCharacter _playerCharacter;

		/// <summary>
		/// Returns the character these stats belong to.
		/// </summary>
		public ICharacter Character { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="character"></param>
		public Parameters(ICharacter character)
		{
			this.Character = character;
			_playerCharacter = character as PlayerCharacter;
		}

		/// <summary>
		/// Gets or sets the character's STR stat.
		/// </summary>
		public int Str
		{
			get => _str;
			set { _str = Math.Max(1, value); }
		}
		private int _str = 1;

		/// <summary>
		/// Gets or sets the character's AGI stat.
		/// </summary>
		public int Agi
		{
			get => _agi;
			set { _agi = Math.Max(1, value); }
		}
		private int _agi = 1;

		/// <summary>
		/// Gets or sets the character's VIT stat.
		/// </summary>
		public int Vit
		{
			get => _vit;
			set { _vit = Math.Max(1, value); }
		}
		private int _vit = 1;

		/// <summary>
		/// Gets or sets the character's INT stat.
		/// </summary>
		public int Int
		{
			get => _int;
			set { _int = Math.Max(1, value); }
		}
		private int _int = 1;

		/// <summary>
		/// Gets or sets the character's DEX stat.
		/// </summary>
		public int Dex
		{
			get => _dex;
			set { _dex = Math.Max(1, value); }
		}
		private int _dex = 1;

		/// <summary>
		/// Gets or sets the character's LUK stat.
		/// </summary>
		public int Luk
		{
			get => _luk;
			set { _luk = Math.Max(1, value); }
		}
		private int _luk = 1;

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the STR stat by one.
		/// </summary>
		public int StrNeeded => (1 + (this.Str + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the AGI stat by one.
		/// </summary>
		public int AgiNeeded => (1 + (this.Agi + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the VIT stat by one.
		/// </summary>
		public int VitNeeded => (1 + (this.Vit + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the INT stat by one.
		/// </summary>
		public int IntNeeded => (1 + (this.Int + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the DEX stat by one.
		/// </summary>
		public int DexNeeded => (1 + (this.Dex + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the LUK stat by one.
		/// </summary>
		public int LukNeeded => (1 + (this.Luk + 9) / 10);

		/// <summary>
		/// Gets or sets the character's stat points.
		/// </summary>
		public int StatPoints
		{
			get => _statPoints;
			set { _statPoints = Math.Max(0, value); }
		}
		private int _statPoints = 0;

		/// <summary>
		/// Returns the number of skill points the character has to assign.
		/// </summary>
		public int SkillPoints
		{
			get => _skillPoints;
			set { _skillPoints = Math.Max(0, value); }
		}
		private int _skillPoints = 0;

		/// <summary>
		/// Returns the character's current weight.
		/// </summary>
		public int Weight { get; set; } = 10;

		/// <summary>
		/// Returns the character's current max weight.
		/// </summary>
		public int WeightMax { get; set; } = 20000;

		/// <summary>
		/// Gets or sets the character's speed.
		/// </summary>
		public int Speed { get; set; } = 200;

		/// <summary>
		/// Gets or sets how many HP the character currently has.
		/// </summary>
		public int Hp
		{
			get => _hp;
			set { _hp = Math2.Clamp(0, this.HpMax, value); }
		}
		private int _hp = 40;

		/// <summary>
		/// Gets or sets the character's maximum amount of HP.
		/// </summary>
		public int HpMax
		{
			get => _hpMax;
			set { _hpMax = Math.Max(1, value); }
		}
		private int _hpMax = 40;

		/// <summary>
		/// Gets or sets how many SP the character currently has.
		/// </summary>
		public int Sp
		{
			get => _sp;
			set { _sp = Math2.Clamp(0, this.SpMax, value); }
		}
		private int _sp = 10;

		/// <summary>
		/// Gets or sets the character's maximum amount of SP.
		/// </summary>
		public int SpMax
		{
			get => _spMax;
			set { _spMax = Math.Max(1, value); }
		}
		private int _spMax = 10;

		/// <summary>
		/// Returns the character's current min attack.
		/// </summary>
		public int AtkMin { get; set; } = 10;

		/// <summary>
		/// Returns the character's current max attack.
		/// </summary>
		public int AtkMax { get; set; } = 15;

		/// <summary>
		/// Returns the character's current magic attack.
		/// </summary>
		public int MAtk { get; set; } = 1;

		/// <summary>
		/// Returns the character's current defense.
		/// </summary>
		public int Defense { get; set; } = 7;

		/// <summary>
		/// Gets or sets the character's base experience points.
		/// </summary>
		public int BaseExp { get; set; } = 10;

		/// <summary>
		/// Gets or sets the character's job experience points.
		/// </summary>
		public int JobExp { get; set; }

		/// <summary>
		/// Returns the amount of experience points necessary to reach
		/// the next base level.
		/// </summary>
		public int BaseExpNeeded { get; set; } = 9;

		/// <summary>
		/// Returns the amount of experience points necessary to reach
		/// the next job level.
		/// </summary>
		public int JobExpNeeded { get; set; }

		/// <summary>
		/// Gets or sets the character's current base level.
		/// </summary>
		public int BaseLevel { get; set; } = 1;

		/// <summary>
		/// Gets or sets the character's current base level.
		/// </summary>
		public int JobLevel { get; set; } = 1;

		/// <summary>
		/// Gets or sets how many Zeny the character has.
		/// </summary>
		public int Zeny
		{
			get => _zeny;
			set { _zeny = Math.Max(0, value); }
		}
		private int _zeny = 0;

		/// <summary>
		/// Returns the value for the given stat.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int Get(ParameterType type)
		{
			switch (type)
			{
				case ParameterType.Speed: return this.Speed;
				case ParameterType.BaseExp: return this.BaseExp;
				case ParameterType.JobExp: return this.JobExp;
				case ParameterType.Hp: return this.Hp;
				case ParameterType.HpMax: return this.HpMax;
				case ParameterType.Sp: return this.Sp;
				case ParameterType.SpMax: return this.SpMax;
				case ParameterType.StatPoints: return this.StatPoints;
				case ParameterType.BaseLevel: return this.BaseLevel;
				case ParameterType.SkillPoints: return this.SkillPoints;
				case ParameterType.Str: return this.Str;
				case ParameterType.Agi: return this.Agi;
				case ParameterType.Vit: return this.Vit;
				case ParameterType.Int: return this.Int;
				case ParameterType.Dex: return this.Dex;
				case ParameterType.Luk: return this.Luk;
				case ParameterType.Zeny: return this.Zeny;
				case ParameterType.BaseExpNeeded: return this.BaseExpNeeded;
				case ParameterType.JobExpNeeded: return this.JobExpNeeded;
				case ParameterType.Weight: return this.Weight;
				case ParameterType.WeightMax: return this.WeightMax;
				case ParameterType.AtkMin: return this.AtkMin;
				case ParameterType.AtkMax: return this.AtkMax;
				case ParameterType.Defense: return this.Defense;
				case ParameterType.MAtk: return this.MAtk;

				default:
					throw new ArgumentException($"Invalid stat type '{type}'.");
			}
		}

		/// <summary>
		/// Modifies the given stat and updates the client.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="modifier"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int Modify(ParameterType type, int modifier)
		{
			var newValue = 0;
			switch (type)
			{
				case ParameterType.Str: newValue = this.Str += modifier; break;
				case ParameterType.Agi: newValue = this.Agi += modifier; break;
				case ParameterType.Vit: newValue = this.Vit += modifier; break;
				case ParameterType.Int: newValue = this.Int += modifier; break;
				case ParameterType.Dex: newValue = this.Dex += modifier; break;
				case ParameterType.Luk: newValue = this.Luk += modifier; break;
				case ParameterType.StatPoints: newValue = this.StatPoints += modifier; break;
				case ParameterType.SkillPoints: newValue = this.SkillPoints += modifier; break;
				case ParameterType.Zeny: newValue = this.Zeny = Math2.AddChecked(this.Zeny, modifier); break;

				default:
					throw new ArgumentException($"Unsupported stat type '{type}'.");
			}

			this.UpdateClient(type);

			return newValue;
		}

		/// <summary>
		/// Returns the amount of stat points needed to increase the
		/// given stat by one.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int GetStatPointsNeeded(ParameterType type)
		{
			switch (type)
			{
				case ParameterType.Str: return this.StrNeeded;
				case ParameterType.Agi: return this.AgiNeeded;
				case ParameterType.Vit: return this.VitNeeded;
				case ParameterType.Int: return this.IntNeeded;
				case ParameterType.Dex: return this.DexNeeded;
				case ParameterType.Luk: return this.LukNeeded;

				default:
					throw new ArgumentException($"Invalid stat type '{type}'.");
			}
		}

		/// <summary>
		/// Recalculates character's HP, updates the property, and returns
		/// the new max value.
		/// </summary>
		/// <returns></returns>
		private int RecalculateHp()
		{
			var statMultiplier = 1;
			var statFactor = 0;
			var statAdditions = 0;
			var itemStatMultipliers = 1;
			var sigmaOfBaseLevel = RoMath.Sigma(this.BaseLevel - 1);

			this.HpMax = (int)Math.Floor((Math.Floor((35f + this.BaseLevel * statMultiplier + sigmaOfBaseLevel * statFactor) * (1 + this.Vit / 100f)) + statAdditions) * itemStatMultipliers);

			if (this.Hp > this.HpMax)
				this.Hp = this.HpMax;

			this.UpdateClient(ParameterType.Hp, ParameterType.HpMax);

			return this.HpMax;
		}

		/// <summary>
		/// Recalculates character's SP, updates the property, and returns
		/// the new max value.
		/// </summary>
		/// <returns></returns>
		private int RecalculateSp()
		{
			var statMultiplier = 1;
			var statFactor = 0;
			var statAdditions = 0;
			var itemStatMultiplier = 1;
			var sigmaOfBaseLevel = RoMath.Sigma(this.BaseLevel - 1);

			this.SpMax = (int)Math.Floor((Math.Floor((10f + this.BaseLevel * statMultiplier + sigmaOfBaseLevel * statFactor) * (1 + this.Vit / 100f)) + statAdditions) * itemStatMultiplier);

			if (this.Sp > this.SpMax)
				this.Sp = this.SpMax;

			this.UpdateClient(ParameterType.Sp, ParameterType.SpMax);

			return this.SpMax;
		}

		/// <summary>
		/// Updates the given parameters on the character's client.
		/// </summary>
		/// <param name="types"></param>
		public void UpdateClient(params ParameterType[] types)
		{
			if (_playerCharacter == null)
				return;

			if (types == null || types.Length == 0)
				return;

			foreach (var type in types)
			{
				if (!type.IsLong())
					Send.ZC_PAR_CHANGE(_playerCharacter, type);
				else
					Send.ZC_LONGPAR_CHANGE(_playerCharacter, type);
			}
		}
	}
}
