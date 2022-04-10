namespace Sabine.Shared
{
	/// <summary>
	/// Strings used as headers for the applications.
	/// </summary>
	public static class ConsoleHeader
	{
		/// <summary>
		/// Returns title string or ASCII art.
		/// </summary>
		public static readonly string[] Title = new[]
		{
			@" ____   __   ____  __  __ _  ____ ",
			@"/ ___) / _\ (  _ \(  )(  ( \(  __)",
			@"\___ \/    \ ) _ ( )( /    / ) _) ",
			@"(____/\_/\_/(____/(__)\_)__)(____)",
		};

		/// <summary>
		/// Returns the subtitle line.
		/// </summary>
		public static readonly string[] Subtitle = new[]
		{
			"Nostalgia 1, Productivity 0.",
		};

		/// <summary>
		/// Returns the credits line.
		/// </summary>
		public static readonly string[] Credits = new[]
		{
			"Copyright (C) NoCodeNoLife",
		};
	}
}
