using System;
using Yggdrasil.Events;
using Yggdrasil.Scripting;

namespace Sabine.Zone.Scripting
{
	/// <summary>
	/// General purpose script class.
	/// </summary>
	public class GeneralScript : IScript, IDisposable
	{
		/// <summary>
		/// Initializes script.
		/// </summary>
		/// <returns></returns>
		public bool Init()
		{
			this.Load();

			OnAttribute.Load(this, ZoneServer.Instance.ServerEvents);

			return true;
		}

		/// <summary>
		/// Called when the script is being removed before a reload.
		/// </summary>
		public void Dispose()
		{
			OnAttribute.Unload(this, ZoneServer.Instance.ServerEvents);
		}

		/// <summary>
		/// Called when the script is being initialized.
		/// </summary>
		public virtual void Load()
		{
		}
	}
}
