using System.Threading;
using Sabine.Shared.Database;

namespace Sabine.Zone.Network
{
	public class DummyConnection : ZoneConnection
	{
		private static int DummyAccountId = 1_000_001;
		private static int DummySessionId = 5_000_001;

		public DummyConnection()
		{
			this.Account = new Account();
			this.Account.Id = Interlocked.Increment(ref DummyAccountId);
			this.Account.SessionId = Interlocked.Increment(ref DummySessionId);
			this.Account.Username = "Dummy" + this.Account.Id;
		}

		public override void Send(byte[] data)
		{
		}
	}
}
