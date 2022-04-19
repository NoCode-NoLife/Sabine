using Sabine.Shared.Util;
using Sabine.Zone.Scripting.Dialogues;

namespace Sabine.Zone.Scripting
{
	public static class Shortcuts
	{
		/// <summary>
		/// Returns an option element, to be used with the Menu function.
		/// </summary>
		/// <param name="text">Text to display in the menu.</param>
		/// <param name="key">Key to return if the option was selected.</param>
		/// <returns></returns>
		public static DialogOption Option(string text, string key)
			=> new DialogOption(text, key);

		/// <summary>
		/// Returns a localized version of the given string.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string L(string key)
			=> Localization.Get(key);

		/// <summary>
		/// Returns a localized version of the given string, formatted
		/// with the optional arguments.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string LF(string key, params object[] args)
			=> string.Format(Localization.Get(key), args);

		/// <summary>
		/// Returns a localized plural version of the given string.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keyPlural"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static string LN(string key, string keyPlural, int n)
			=> Localization.GetPlural(key, keyPlural, n);

		/// <summary>
		/// Returns a localized plural version of the given string,
		/// formatted with the optional arguments.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keyPlural"></param>
		/// <param name="n"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string LNF(string key, string keyPlural, int n, params object[] args)
			=> string.Format(Localization.GetPlural(key, keyPlural, n), args);
	}
}
