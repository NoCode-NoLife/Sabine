using System;
using Sabine.Shared.Const;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Entities.Components.Characters
{
	/// <summary>
	/// Represents a character's parameters (stats, sub-stats, and
	/// anything related).
	/// </summary>
	public abstract class Parameters
	{
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
		/// Gets or sets how many HP the character currently has,
		/// capped between 0 and the max HP.
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
		/// Gets or sets how many SP the character currently has,
		/// capped between 0 and the max SP.
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
		/// Returns the character's current attack.
		/// </summary>
		public int Attack { get; set; } = 1;

		/// <summary>
		/// Returns the character's current min attack.
		/// </summary>
		public int AttackMin { get; set; } = 1;

		/// <summary>
		/// Returns the character's current max attack.
		/// </summary>
		public int AttackMax { get; set; } = 1;

		/// <summary>
		/// Returns the character's current singular magic attack value,
		/// as used by the alpha client.
		/// </summary>
		public int MagicAttack { get; set; }

		/// <summary>
		/// Returns the character's current min magic attack.
		/// </summary>
		public int MagicAttackMin { get; set; }

		/// <summary>
		/// Returns the character's current max magic attack.
		/// </summary>
		public int MagicAttackMax { get; set; }

		/// <summary>
		/// Returns the character's current defense.
		/// </summary>
		public int Defense { get; set; }

		/// <summary>
		/// Gets or sets the character's base experience points.
		/// </summary>
		public int BaseExp { get; set; }

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
		/// Gets or sets the character's hit value, which determines how
		/// likely they are to hit or miss an enemy.
		/// </summary>
		public int Hit { get; set; } = 2;

		/// <summary>
		/// Gets or sets the character's flee value, which determines how
		/// likely they are to evade an enemy attack.
		/// </summary>
		public int Flee { get; set; } = 2;

		/// <summary>
		/// Returns the delay between attacks for the character.
		/// </summary>
		public int AttackDelay { get; set; } = 800;

		/// <summary>
		/// Returns the duration of the character's attack motion.
		/// </summary>
		public int AttackMotionDelay { get; set; } = 800;

		/// <summary>
		/// Returns the time it takes to play the character's taking
		/// damage motion.
		/// </summary>
		public int DamageMotionDelay { get; set; } = 1000;

		/// <summary>
		/// Gets or sets the character's attack speed value.
		/// </summary>
		public int Aspd { get; set; } = 150;

		/// <summary>
		/// Returns the value for the given parameter.
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
				case ParameterType.AttackMin: return this.AttackMin;
				case ParameterType.AttackMax: return this.AttackMax;
				case ParameterType.Defense: return this.Defense;
				case ParameterType.MagicAttack: return this.MagicAttack;
				case ParameterType.JobLevel: return this.JobLevel;

				default:
					throw new ArgumentException($"Invalid parameter type '{type}'.");
			}
		}

		/// <summary>
		/// Sets the value for the given stat.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int Set(ParameterType type, int value)
		{
			var before = this.Get(type);

			switch (type)
			{
				case ParameterType.Speed: this.Speed = value; break;
				case ParameterType.BaseExp: this.BaseExp = value; break;
				case ParameterType.JobExp: this.JobExp = value; break;
				case ParameterType.Hp: this.Hp = value; break;
				case ParameterType.HpMax: this.HpMax = value; break;
				case ParameterType.Sp: this.Sp = value; break;
				case ParameterType.SpMax: this.SpMax = value; break;
				case ParameterType.StatPoints: this.StatPoints = value; break;
				case ParameterType.BaseLevel: this.BaseLevel = value; break;
				case ParameterType.SkillPoints: this.SkillPoints = value; break;
				case ParameterType.Str: this.Str = value; break;
				case ParameterType.Agi: this.Agi = value; break;
				case ParameterType.Vit: this.Vit = value; break;
				case ParameterType.Int: this.Int = value; break;
				case ParameterType.Dex: this.Dex = value; break;
				case ParameterType.Luk: this.Luk = value; break;
				case ParameterType.Zeny: this.Zeny = value; break;
				case ParameterType.BaseExpNeeded: this.BaseExpNeeded = value; break;
				case ParameterType.JobExpNeeded: this.JobExpNeeded = value; break;
				case ParameterType.Weight: this.Weight = value; break;
				case ParameterType.WeightMax: this.WeightMax = value; break;
				case ParameterType.AttackMin: this.AttackMin = value; break;
				case ParameterType.AttackMax: this.AttackMax = value; break;
				case ParameterType.Defense: this.Defense = value; break;
				case ParameterType.MagicAttack: this.MagicAttack = value; break;
				case ParameterType.JobLevel: this.JobLevel = value; break;

				default:
					throw new ArgumentException($"Invalid parameter type '{type}'.");
			}

			var after = this.Get(type);

			this.OnModified(type, before, after);

			return after;
		}

		/// <summary>
		/// Modifies the given parameter and updates the client. Returns the
		/// parameter's new value after the modification.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="modifier"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int Modify(ParameterType type, int modifier)
		{
			var before = this.Get(type);

			switch (type)
			{
				case ParameterType.Speed: this.Speed += modifier; break;
				case ParameterType.BaseExp: this.BaseExp += modifier; break;
				case ParameterType.JobExp: this.JobExp += modifier; break;
				case ParameterType.Hp: this.Hp += modifier; break;
				case ParameterType.HpMax: this.HpMax += modifier; break;
				case ParameterType.Sp: this.Sp += modifier; break;
				case ParameterType.SpMax: this.SpMax += modifier; break;
				case ParameterType.StatPoints: this.StatPoints += modifier; break;
				case ParameterType.BaseLevel: this.BaseLevel += modifier; break;
				case ParameterType.SkillPoints: this.SkillPoints += modifier; break;
				case ParameterType.Str: this.Str += modifier; break;
				case ParameterType.Agi: this.Agi += modifier; break;
				case ParameterType.Vit: this.Vit += modifier; break;
				case ParameterType.Int: this.Int += modifier; break;
				case ParameterType.Dex: this.Dex += modifier; break;
				case ParameterType.Luk: this.Luk += modifier; break;
				case ParameterType.Zeny: this.Zeny = Math2.AddChecked(this.Zeny, modifier); break;
				case ParameterType.BaseExpNeeded: this.BaseExpNeeded += modifier; break;
				case ParameterType.JobExpNeeded: this.JobExpNeeded += modifier; break;
				case ParameterType.Weight: this.Weight += modifier; break;
				case ParameterType.WeightMax: this.WeightMax += modifier; break;
				case ParameterType.AttackMin: this.AttackMin += modifier; break;
				case ParameterType.AttackMax: this.AttackMax += modifier; break;
				case ParameterType.Defense: this.Defense += modifier; break;
				case ParameterType.MagicAttack: this.MagicAttack += modifier; break;
				case ParameterType.JobLevel: this.JobLevel += modifier; break;

				default:
					throw new ArgumentException($"Unsupported parameter type '{type}'.");
			}

			// Get new value after it was properly assigned and potentially
			// capped in the property.
			var after = this.Get(type);

			this.OnModified(type, before, after);

			return after;
		}

		/// <summary>
		/// Called when a parameter was modified via Modify.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="before"></param>
		/// <param name="after"></param>
		protected virtual void OnModified(ParameterType type, int before, int after)
		{
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
		/// Recalculates all sub-stats.
		/// </summary>
		public virtual void RecalculateAll()
		{
		}
	}

	/// <summary>
	/// Represents a character's parameters (stats, sub-stats, and
	/// anything related).
	/// </summary>
	public class Parameters<TCharacter> : Parameters where TCharacter : Character
	{
		/// <summary>
		/// Returns the character these parameters belong to.
		/// </summary>
		public TCharacter Character { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="character"></param>
		public Parameters(TCharacter character)
		{
			this.Character = character;
		}
	}
}
