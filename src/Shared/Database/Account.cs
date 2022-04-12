using Sabine.Shared.Const;
using Sabine.Shared.Util;

namespace Sabine.Shared.Database
{
	/// <summary>
	/// Represents an account from the database.
	/// </summary>
	public class Account
	{
		/// <summary>
		/// Gets or sets the account's id.
		/// </summary>
		public int Id { get; set; } = int.MaxValue;

		/// <summary>
		/// Gets or sets the account's temporary session id.
		/// </summary>
		public int SessionId { get; set; } = -1;

		/// <summary>
		/// Gets or sets the account's username.
		/// </summary>
		public string Username { get; set; } = "guest";

		/// <summary>
		/// Gets or sets the account's password.
		/// </summary>
		public string Password { get; set; } = "guest";

		/// <summary>
		/// Gets or sets the account's sex.
		/// </summary>
		public Sex Sex { get; set; } = Sex.Male;

		/// <summary>
		/// Gets or sets the account's authority level.
		/// </summary>
		public int Authority { get; set; }

		/// <summary>
		/// Returns the account's variable container.
		/// </summary>
		public VariableContainer Vars { get; } = new VariableContainer();
	}
}
