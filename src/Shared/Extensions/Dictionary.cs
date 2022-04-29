using System.Collections.Generic;

namespace Sabine.Shared.Extensions
{
	/// <summary>
	/// Extension methods for generic Dictionaries.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Deconstructs a KeyValuePair into a tuple.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dictonary"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> dictonary, out TKey key, out TValue value)
		{
			key = dictonary.Key;
			value = dictonary.Value;
		}
	}
}
