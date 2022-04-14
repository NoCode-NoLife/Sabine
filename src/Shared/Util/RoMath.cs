namespace Sabine.Shared.Util
{
	public static class RoMath
	{
		/// <summary>
		/// Calculates sum of all numbers leading up to the given one.
		/// </summary>
		/// <example>
		/// Sigma(5) = 15; // 1 + 2 + 3 + 4 + 5
		/// </example>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int Sigma(int n)
			=> (n * (n + 1)) / 2;
	}
}
