using System;
using System.Linq;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.Util;
using Sabine.Zone.Network;

namespace Sabine.Zone.World.Entities.Components.Characters
{
	/// <summary>
	/// Represents a character's parameters (stats, sub-stats, and
	/// anything related).
	/// </summary>
	public class PlayerCharacterParameters : Parameters<PlayerCharacter>
	{
		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="character"></param>
		public PlayerCharacterParameters(PlayerCharacter character)
			: base(character)
		{
		}

		/// <summary>
		/// Recalculates sub-stats based on modified parameters and
		/// updates the client..
		/// </summary>
		/// <param name="type"></param>
		/// <param name="before"></param>
		/// <param name="after"></param>
		protected override void OnModified(ParameterType type, int before, int after)
		{
			this.UpdateClient(type);

			// Send a status update if one of the base stats changed,
			// as the stat points needed for raising it again might've
			// changed as well.			
			if (type >= ParameterType.Str && type <= ParameterType.Luk)
				Send.ZC_STATUS(this.Character);

			if (type == ParameterType.Str || type == ParameterType.Dex || type == ParameterType.Luk)
				this.RecalculateAttack();

			if (type == ParameterType.Dex)
				this.RecalculateHit();

			if (type == ParameterType.Agi)
				this.RecalculateFlee();

			if (type == ParameterType.Agi || type == ParameterType.Dex)
				this.RecalculateSpeeds();

			if (type == ParameterType.Vit)
			{
				this.RecalculateHp();
				this.RecalculateDefense();
			}

			if (type == ParameterType.Int)
			{
				this.RecalculateSp();
				this.RecalculateMagicAttack();
			}
		}

		/// <summary>
		/// Recalculates character's HP, updates the property, and returns
		/// the new max value.
		/// </summary>
		/// <returns></returns>
		public void RecalculateHp()
		{
			var modifiers = this.Character.JobData.Modifiers;

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
		}

		/// <summary>
		/// Recalculates character's SP, updates the property, and returns
		/// the new max value.
		/// </summary>
		/// <returns></returns>
		public void RecalculateSp()
		{
			var modifiers = this.Character.JobData.Modifiers;

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
		}

		/// <summary>
		/// Recalculates character's attack parameters and updates the
		/// property.
		/// </summary>
		/// <returns></returns>
		public void RecalculateAttack()
		{
			var weapon = this.Character.Inventory.RightHand;
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

			this.UpdateClient(ParameterType.AttackMin, ParameterType.AttackMax);
		}

		/// <summary>
		/// Recalculates character's attack parameters and updates the
		/// property.
		/// </summary>
		/// <returns></returns>
		public void RecalculateMagicAttack()
		{
			this.MagicAttack = (int)(this.Int + Math.Pow(this.Int / 10, 2)) / 2;
			this.MagicAttackMin = (int)(this.Int + Math.Pow(this.Int / 7, 2));
			this.MagicAttackMax = (int)(this.Int + Math.Pow(this.Int / 5, 2));

			this.UpdateClient(ParameterType.MagicAttack);
		}

		/// <summary>
		/// Recalculates character's attack parameters and updates the
		/// property.
		/// </summary>
		/// <returns></returns>
		public void RecalculateDefense()
		{
			// The alpha client has only one (visible) defense stat,
			// which would presumably display the armor's defense,
			// because it might be confusing if you equipped a 5
			// defense armor, but your defense value didn't change.
			// This armor defense was percentage-based later on,
			// however, and doesn't mix with the point-based VIT
			// defense bonus. Is that bonus maybe not displayed?
			// Did it not exist? Did it play into the percentage?
			this.Defense = this.Character.Inventory.GetEquipDefense();

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

			this.UpdateClient(ParameterType.Defense);
		}

		/// <summary>
		/// Sets the base EXP and job EXP the character currently needs to
		/// reach the next levels.
		/// </summary>
		public void RecalculateExp()
		{
			this.BaseExpNeeded = SabineData.ExpTables.GetExpNeeded(ExpTableType.Base, this.Character.JobId, this.BaseLevel);
			this.JobExpNeeded = SabineData.ExpTables.GetExpNeeded(ExpTableType.Job, this.Character.JobId, this.JobLevel);

			this.UpdateClient(ParameterType.BaseExpNeeded, ParameterType.JobExpNeeded);
		}

		/// <summary>
		/// Calculates and sets the character's current hit value.
		/// </summary>
		public void RecalculateHit()
		{
			this.Hit = this.BaseLevel + this.Dex;
			//this.UpdateClient(ParameterType.Hit);
		}

		/// <summary>
		/// Calculates and sets the character's current flee value.
		/// </summary>
		public void RecalculateFlee()
		{
			this.Flee = this.BaseLevel + this.Agi;
			//this.UpdateClient(ParameterType.Flee);
		}

		/// <summary>
		/// Calculates the character's max weight and updates the client.
		/// </summary>
		public void RecalculateWeight()
		{
			this.Weight = this.Character.Inventory.GetItems().Sum(a => a.Data.Weight);
			this.WeightMax = 2000 + this.Character.JobData.Modifiers.Weight + this.Str * 30;

			// The exact alpha weight formula is currently unknown,
			// but it seems like characters had ~16~25% of the weight
			// they would have in later versions.
			if (!SabineData.Features.IsEnabled("HigherMaxWeight"))
				this.WeightMax /= 5;

			this.UpdateClient(ParameterType.Weight, ParameterType.WeightMax);
		}

		/// <summary>
		/// Recalculates attack speed values and motion delays.
		/// </summary>
		public void RecalculateSpeeds()
		{
			// The alpha client doesn't use server-side delays and instead
			// has them hardcoded. It sends the attack action every 800ms,
			// regardless of AGI and DEX, so that's presumably the attack
			// motion delay, and it would be equivilant to an ASPD of 160,
			// which matches the bare hand ASPD with 1 AGI and 1 DEX.
			if (Game.Version < Versions.Beta1)
			{
				this.AttackMotionDelay = 800;
				this.AttackDelay = this.AttackMotionDelay;
				this.Aspd = 160;
				return;
			}

			//var weaponType = this.Character.Inventory.RightHand?.Data.WeaponType;
			var weaponDelay = this.Character.JobData.WeaponDelays.BareHand; // .GetDelay(weaponType);
			var agi = this.Agi;
			var dex = this.Dex;
			var mods = 1f;

			var delay = Math.Ceiling(mods * (weaponDelay - (Math.Ceiling(weaponDelay * agi / 25f) + Math.Ceiling(weaponDelay * dex / 100f)) / 10f));
			var aspd = (int)Math.Min(200, 200 - delay);

			this.AttackMotionDelay = (int)(delay * 20);
			this.AttackDelay = this.AttackMotionDelay;
			this.Aspd = aspd;
		}

		/// <summary>
		/// Recalculates all sub-stats and updates the client.
		/// </summary>
		public override void RecalculateAll()
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
			this.RecalculateSpeeds();
		}

		/// <summary>
		/// Updates the given parameters on the character's client.
		/// </summary>
		/// <param name="types"></param>
		public void UpdateClient(params ParameterType[] types)
		{
			if (types == null || types.Length == 0)
				return;

			foreach (var type in types)
			{
				if (!type.IsLong())
					Send.ZC_PAR_CHANGE(this.Character, type);
				else
					Send.ZC_LONGPAR_CHANGE(this.Character, type);
			}
		}
	}
}
