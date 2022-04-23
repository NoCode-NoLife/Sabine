using System;
using System.Linq;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;
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
		private readonly PlayerCharacter _playerCharacter;

		/// <summary>
		/// Returns the character these stats belong to.
		/// </summary>
		public Character Character { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="character"></param>
		public Parameters(Character character)
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
				case ParameterType.AttackMin: return this.AttackMin;
				case ParameterType.AttackMax: return this.AttackMax;
				case ParameterType.Defense: return this.Defense;
				case ParameterType.MagicAttack: return this.MagicAttack;

				default:
					throw new ArgumentException($"Invalid stat type '{type}'.");
			}
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
			switch (type)
			{
				case ParameterType.Hp: this.Hp += modifier; break;
				case ParameterType.Sp: this.Sp += modifier; break;
				case ParameterType.Str: this.Str += modifier; break;
				case ParameterType.Agi: this.Agi += modifier; break;
				case ParameterType.Vit: this.Vit += modifier; break;
				case ParameterType.Int: this.Int += modifier; break;
				case ParameterType.Dex: this.Dex += modifier; break;
				case ParameterType.Luk: this.Luk += modifier; break;
				case ParameterType.StatPoints: this.StatPoints += modifier; break;
				case ParameterType.SkillPoints: this.SkillPoints += modifier; break;
				case ParameterType.Zeny: this.Zeny = Math2.AddChecked(this.Zeny, modifier); break;

				default:
					throw new ArgumentException($"Unsupported stat type '{type}'.");
			}

			this.UpdateClient(type);

			// TODO: These player character checks are annoying~ Make a
			//   PlayerCharacterParameters class or something.

			// Send a status update if one of the base stats changed,
			// as the stat points needed for raising it again might've
			// changed as well.			
			if (_playerCharacter != null && type >= ParameterType.Str && type <= ParameterType.Luk)
				Send.ZC_STATUS(_playerCharacter);

			if (type == ParameterType.Str || type == ParameterType.Dex || type == ParameterType.Luk)
				this.RecalculateAttack();

			if (type == ParameterType.Dex)
				this.RecalculateHit();

			if (type == ParameterType.Agi)
				this.RecalculateFlee();

			if (type == ParameterType.Vit)
			{
				this.RecalculateHp();
				this.RecalculateDefense();
			}

			if (type == ParameterType.Int)
				this.RecalculateMagicAttack();

			// Get new value after it was properly assigned and potentially
			// capped in the property.
			return this.Get(type);
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
		public int RecalculateHp()
		{
			if (_playerCharacter == null)
				return this.HpMax;

			var modifiers = _playerCharacter.JobData.Modifiers;

			var baseVal = 35f;
			var statMultiplier = modifiers.HpMultiplier;
			var statFactor = modifiers.HpFactor;
			var statAdditions = 0;
			var itemStatMultipliers = 1;
			var sigmaOfBaseLevel = RoMath.Sigma(this.BaseLevel - 1);

			this.HpMax = (int)Math.Floor((Math.Floor((baseVal + this.BaseLevel * statMultiplier + sigmaOfBaseLevel * statFactor) * (1 + this.Vit / 100f)) + statAdditions) * itemStatMultipliers);

			// I'll admit, I'm getting lazy by this point. Figuring out
			// formulas isn't my strong suit. Half HP and double SP don't
			// match the screen shots, but it's close and simple.
			this.HpMax /= 2;

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
		public int RecalculateSp()
		{
			if (_playerCharacter == null)
				return this.SpMax;

			var modifiers = _playerCharacter.JobData.Modifiers;

			var baseVal = 10f;
			var statFactor = modifiers.SpFactor;
			var statAdditions = 0;
			var itemStatMultiplier = 1;
			var sigmaOfBaseLevel = RoMath.Sigma(this.BaseLevel - 1);

			this.SpMax = (int)Math.Floor((Math.Floor((baseVal + this.BaseLevel * statFactor) * (1 + this.Vit / 100f)) + statAdditions) * itemStatMultiplier);
			this.SpMax *= 2;

			if (this.Sp > this.SpMax)
				this.Sp = this.SpMax;

			this.UpdateClient(ParameterType.Sp, ParameterType.SpMax);

			return this.SpMax;
		}

		/// <summary>
		/// Recalculates character's attack parameters and updates the
		/// property.
		/// </summary>
		/// <returns></returns>
		public void RecalculateAttack()
		{
			if (_playerCharacter == null)
				return;

			var weapon = _playerCharacter.Inventory.RightHand;
			var mainStat = 0;
			var secStat = 0;

			if (weapon?.Type == ItemType.RangedWeapon)
			{
				mainStat = this.Dex;
				secStat = this.Str;
			}
			else
			{
				mainStat = this.Str;
				secStat = this.Dex;
			}

			// Another sub-stat, another formula, another attempt at
			// guessing math based on screen shots. Since the alpha
			// client uses min/max attack stats, maybe it worked like
			// magic did later on, so let's try using that formula,
			// but adjust it slightly for the lower attack values
			// seen in screen shots.

			var fromStats = (int)(mainStat + Math.Pow(mainStat / 10, 2) + secStat / 5 + this.Luk / 5);
			var fromStatsMin = (int)(mainStat + Math.Pow(mainStat / 7, 2)) / 3;
			var fromStatsMax = (int)(mainStat + Math.Pow(mainStat / 5, 2)) / 3;

			var fromWeapon = 0;
			var fromWeaponMin = 0;
			var fromWeaponMax = 0;

			if (weapon != null)
			{
				fromWeapon = weapon.Data.Attack;
				fromWeaponMin = weapon.Data.AttackMin;
				fromWeaponMax = weapon.Data.AttackMax;
			}

			this.Attack = fromStats + fromWeapon;
			this.AttackMin = fromStatsMin + fromWeaponMin;
			this.AttackMax = fromStatsMax + fromWeaponMax;

			Send.ZC_PAR_CHANGE(_playerCharacter, ParameterType.AttackMin);
			Send.ZC_PAR_CHANGE(_playerCharacter, ParameterType.AttackMax);
		}

		/// <summary>
		/// Recalculates character's attack parameters and updates the
		/// property.
		/// </summary>
		/// <returns></returns>
		public void RecalculateMagicAttack()
		{
			if (_playerCharacter == null)
				return;

			this.MagicAttack = (int)(this.Int + Math.Pow(this.Int / 10, 2)) / 2;
			this.MagicAttackMin = (int)(this.Int + Math.Pow(this.Int / 7, 2));
			this.MagicAttackMax = (int)(this.Int + Math.Pow(this.Int / 5, 2));

			Send.ZC_PAR_CHANGE(_playerCharacter, ParameterType.MagicAttack);
		}

		/// <summary>
		/// Recalculates character's attack parameters and updates the
		/// property.
		/// </summary>
		/// <returns></returns>
		public void RecalculateDefense()
		{
			if (_playerCharacter == null)
				return;

			// The alpha client has only one (visible) defense stat,
			// which would presumably display the armor's defense,
			// because it might be confusing if you equipped a 5
			// defense armor, but your defense value didn't change.
			// This armor defense was percentage-based later on,
			// however, and doesn't mix with the point-based VIT
			// defense bonus. Is that bonus maybe not displayed?
			// Did it not exist? Did it play into the percentage?
			this.Defense = _playerCharacter.Inventory.GetEquipDefense();

			// Update: Based on screen shots, we can see that either
			// equip had different defense stats, or that VIT did play
			// into the DEF stat *somehow*, which would make sense.
			// The following formula is made up and *not* accurate,
			// but it gets us pretty close to the screen shots, and
			// with full VIT and the best equipment you would have
			// about 57% damage reduction. It's also possible that
			// these values were combined in some way, but not used
			// together. This will be difficult to proof either way
			// though.
			this.Defense += this.Vit / 3;

			Send.ZC_PAR_CHANGE(_playerCharacter, ParameterType.Defense);
		}

		/// <summary>
		/// Sets the base EXP and job EXP the character currently needs to
		/// reach the next levels.
		/// </summary>
		public void RecalculateExp()
		{
			if (_playerCharacter == null)
				return;

			this.BaseExpNeeded = SabineData.ExpTables.GetExpNeeded(ExpTableType.Base, _playerCharacter.JobId, this.BaseLevel);
			this.JobExpNeeded = SabineData.ExpTables.GetExpNeeded(ExpTableType.Job, _playerCharacter.JobId, this.JobLevel);

			Send.ZC_LONGPAR_CHANGE(_playerCharacter, ParameterType.BaseExpNeeded);
			Send.ZC_LONGPAR_CHANGE(_playerCharacter, ParameterType.JobExpNeeded);
		}

		/// <summary>
		/// Calculates and sets the character's current hit value.
		/// </summary>
		public void RecalculateHit()
		{
			if (_playerCharacter == null)
				return;

			this.Hit = this.BaseLevel + this.Dex;
			//Send.ZC_PAR_CHANGE(_playerCharacter, ParameterType.Hit);
		}

		/// <summary>
		/// Calculates and sets the character's current flee value.
		/// </summary>
		public void RecalculateFlee()
		{
			if (_playerCharacter == null)
				return;

			this.Flee = this.BaseLevel + this.Agi;
			//Send.ZC_PAR_CHANGE(_playerCharacter, ParameterType.Flee);
		}

		/// <summary>
		/// Calculates the character's max weight and updates the client.
		/// </summary>
		private void RecalculateWeight()
		{
			if (_playerCharacter == null)
				return;

			this.Weight = _playerCharacter.Inventory.GetItems().Sum(a => a.Data.Weight);
			this.WeightMax = 2000 + _playerCharacter.JobData.Modifiers.Weight + this.Str * 30;

			// The exact alpha weight formula is currently unknown,
			// but it seems like characters had ~16~25% of the weight
			// they would have in later versions.
			if (!SabineData.Features.IsEnabled("HigherMaxWeight"))
				this.WeightMax /= 5;

			this.UpdateClient(ParameterType.Weight, ParameterType.WeightMax);
		}

		/// <summary>
		/// Recalculates all sub-stats and updates the client.
		/// </summary>
		public void RecalculateAll()
		{
			this.RecalculateHp();
			this.RecalculateSp();
			this.RecalculateAttack();
			this.RecalculateMagicAttack();
			this.RecalculateDefense();
			this.RecalculateHit();
			this.RecalculateFlee();
			this.RecalculateExp();
			this.RecalculateWeight();
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
