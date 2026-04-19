using System;
using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Const;
using Sabine.Zone.Network;
using Sabine.Zone.Skills;

namespace Sabine.Zone.World.Actors.Components.Characters
{
	/// <summary>
	/// A component that manages a character's skills.
	/// </summary>
	/// <param name="character"></param>
	public class SkillComponent(Character character) : ICharacterComponent
	{
		private readonly Dictionary<SkillId, Skill> _skills = new();

		/// <summary>
		/// Returns the character this component belongs to.
		/// </summary>
		public Character Character { get; } = character;

		/// <summary>
		/// Adds the given skill without updating the character's client.
		/// </summary>
		/// <param name="skill"></param>
		public void AddSilent(Skill skill)
		{
			lock (_skills)
				_skills[skill.Id] = skill;
		}

		/// <summary>
		/// Adds the given skill and updates the character's client.
		/// </summary>
		/// <remarks>
		/// If the skill already exists, it gets replaced and updated
		/// instead of added.
		/// </remarks>
		/// <param name="skill"></param>
		public void Add(Skill skill)
		{
			var updated = false;

			lock (_skills)
			{
				if (_skills.ContainsKey(skill.Id))
					updated = true;

				_skills[skill.Id] = skill;
			}

			if (this.Character is PlayerCharacter pc)
			{
				if (!updated)
					Send.ZC_ADD_SKILL(pc, skill);
				else
					Send.ZC_SKILLINFO_UPDATE(pc, skill);
			}
		}

		/// <summary>
		/// Removes the given skill.
		/// </summary>
		/// <param name="skillId"></param>
		public void Remove(SkillId skillId)
		{
			lock (_skills)
				_skills.Remove(skillId);

			//if (this.Character is PlayerCharacter pc)
			//	Send.ZC_REMOVE_SKILL(pc, skillId);
		}

		/// <summary>
		/// Returns the skill with the given id via out. Returns false if
		/// the skill didn't exist.
		/// </summary>
		/// <param name="skillId"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		public bool TryGet(SkillId skillId, out Skill skill)
		{
			lock (_skills)
				return _skills.TryGetValue(skillId, out skill);
		}

		/// <summary>
		/// Returns a list of all skills.
		/// </summary>
		/// <returns></returns>
		public IReadOnlyList<Skill> GetAll()
			=> _skills.Values.ToList();

		/// <summary>
		/// Updates the component.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
		}
	}
}
