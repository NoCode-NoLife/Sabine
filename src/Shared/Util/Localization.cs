using nettext;

namespace Sabine.Shared.Util
{
	/// <summary>
	/// Manages localization strings.
	/// </summary>
	public static class Localization
	{
		private static PoFile _catalog = new PoFile();

		/// <summary>
		/// Loads messages from given PO file.
		public static void Load(string path)
		{
			_catalog.LoadFromFile(path);
		}

		/// <summary>
		/// Returns translated string, or id if no translated version
		/// of id exists.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string Get(string id)
		{
			return _catalog.GetString(id);
		}

		/// <summary>
		/// Returns translated string in context, or id if no translated
		/// version of id exists.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetParticular(string context, string id)
		{
			return _catalog.GetParticularString(context, id);
		}

		/// <summary>
		/// Returns translated string as singular or plural, based on n,
		/// or id/id_plural if no translated version of id exists.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="id_plural"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static string GetPlural(string id, string id_plural, int n)
		{
			return _catalog.GetPluralString(id, id_plural, n);
		}

		/// <summary>
		/// Returns translated string in context as singular or plural,
		/// based on n, or id/id_plural if no translated version of id exists.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetParticularPlural(string context, string id, string id_plural, int n)
		{
			return _catalog.GetParticularPluralString(context, id, id_plural, n);
		}
	}
}
