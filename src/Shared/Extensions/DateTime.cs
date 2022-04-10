using System;

namespace Sabine.Shared.Extensions
{
	/// <summary>
	/// Extensions for DateTime.
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Returns the UNIX timestamp for the date, with the seconds
		/// passed since 1970-01-01.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static int GetUnixTimestamp(this DateTime dt)
			=> (int)dt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}
}
