using System.Threading;
using Sabine.Shared.Database;

namespace Sabine.Zone.Network
{
	/// <summary>
	/// Dummy connection used during testing.
	/// </summary>
	public class DummyConnection : ZoneConnection
	{
		private static int DummyAccountId = 1_000_001;
		private static int DummySessionId = 5_000_001;

		/// <summary>
		/// Creates new instance.
		/// </summary>
		public DummyConnection()
		{
			this.Account = new Account();
			this.Account.Id = Interlocked.Increment(ref DummyAccountId);
			this.Account.SessionId = Interlocked.Increment(ref DummySessionId);
			this.Account.Username = "Dummy" + this.Account.Id;
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="data"></param>
		public override void Send(byte[] data)
		{
		}
	}
}
