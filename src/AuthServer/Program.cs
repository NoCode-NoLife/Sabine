using System;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Sabine.Auth
{
	public class Program
	{
		static void Main(string[] args)
		{
			try
			{
				AuthServer.Instance.Run(args);
			}
			catch (Exception ex)
			{
				Log.Error("While starting server: " + ex);
				ConsoleUtil.Exit(1);
			}
		}
	}
}
