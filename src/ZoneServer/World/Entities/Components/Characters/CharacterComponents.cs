using System;
using System.Collections.Generic;
using System.Linq;

namespace Sabine.Zone.World.Entities.Components.Characters
{
	/// <summary>
	/// Describes a character component.
	/// </summary>
	public interface ICharacterComponent : IUpdateable
	{
		/// <summary>
		/// Returns the character this component belongs to.
		/// </summary>
		Character Character { get; }
	}

	/// <summary>
	/// Manages a character's components.
	/// </summary>
	public class CharacterComponents : IUpdateable
	{
		private readonly object _syncLock = new object();

		private readonly Dictionary<Type, ICharacterComponent> _components = new Dictionary<Type, ICharacterComponent>();
		private ICharacterComponent[] _componentsArr;

		/// <summary>
		/// Adds component to character.
		/// </summary>
		/// <typeparam name="TComponent"></typeparam>
		public void Add<TComponent>(TComponent component) where TComponent : ICharacterComponent
		{
			lock (_syncLock)
			{
				_components[typeof(TComponent)] = component;

				// We will be be iterating over the components much more
				// than we're going to add or remove them, so caching
				// them in an array will save some performance.
				_componentsArr = _components.Values.ToArray();
			}
		}

		/// <summary>
		/// Removes component from character.
		/// </summary>
		/// <typeparam name="TComponent"></typeparam>
		public void Remove<TComponent>() where TComponent : ICharacterComponent
		{
			lock (_syncLock)
			{
				_components.Remove(typeof(TComponent));
				_componentsArr = _components.Values.ToArray();
			}
		}

		/// <summary>
		/// Returns component from character if it exists. It it doesn't
		/// the method returns null.
		/// </summary>
		/// <typeparam name="TComponent"></typeparam>
		/// <returns></returns>
		public TComponent Get<TComponent>() where TComponent : ICharacterComponent
		{
			lock (_syncLock)
			{
				if (!_components.TryGetValue(typeof(TComponent), out var component))
					return default;

				return (TComponent)component;
			}
		}

		/// <summary>
		/// Returns component from character via out if it exists. It it
		/// doesn't the method returns false.
		/// </summary>
		/// <typeparam name="TComponent"></typeparam>
		/// <param name="component"></param>
		/// <returns></returns>
		public bool TryGet<TComponent>(out TComponent component) where TComponent : ICharacterComponent
		{
			component = default;

			lock (_syncLock)
			{
				if (!_components.TryGetValue(typeof(TComponent), out var characterComponent))
					return false;

				component = (TComponent)characterComponent;
				return true;
			}
		}

		/// <summary>
		/// Updates all components.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			lock (_syncLock)
			{
				//if (_componentsArr == null)
				//	return;

				foreach (var component in _components.Values)
					component.Update(elapsed);
			}
		}
	}
}
