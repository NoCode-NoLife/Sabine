using Sabine.Shared.Const;

namespace Sabine.Shared.Database
{
	public class Account
	{
		public int Id { get; set; } = int.MaxValue;
		public int SessionId { get; set; } = -1;
		public string Username { get; set; } = "admin";
		public string Password { get; set; } = "admin";
		public Sex Sex { get; set; } = Sex.Male;
		public int Authority { get; set; } = 99;
	}
}
