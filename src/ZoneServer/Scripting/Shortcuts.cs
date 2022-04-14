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
	}
}
