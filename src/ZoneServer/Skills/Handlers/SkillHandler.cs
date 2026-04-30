using System;
using Sabine.Shared.Const;

namespace Sabine.Zone.Skills.Handlers
{
	/// <summary>
	/// Base interface shared among all skill handlers.
	/// </summary>
	public interface ISkillHandler
	{
	}

	/// <summary>
	/// Describes a skill handler for skills that typically use a target.
	/// </summary>
	public interface ITargetedSkillHandler : ISkillHandler
	{
		public void Handle(UseSkillParams parameters);
	}

	/// <summary>
	/// Describes a skill handler for skills that are typically aimed at
	/// the ground.
	/// </summary>
	public interface IGroundSkillHandler : ISkillHandler
	{
		public void Handle(UseGroundSkillParams parameters);
	}

	/// <summary>
	/// Marks a class as a skill handler for the specified skill ids.
	/// </summary>
	/// <param name="skillIds"></param>
	[AttributeUsage(AttributeTargets.Class)]
	public class SkillHandlerAttribute(params SkillId[] skillIds) : Attribute
	{
		public SkillId[] SkillIds { get; } = skillIds;
	}
}
