using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Shops;
using Yggdrasil.Logging;

namespace Sabine.Zone.Scripting.Dialogues
{
	/// <summary>
	/// Manages a dialog between a player and and NPC and allows sending
	/// of messages to the player.
	/// </summary>
	public class Dialog
	{
		private string _response;
		private readonly SemaphoreSlim _resumeSignal = new SemaphoreSlim(0);
		private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

		private DialogActionType _lastAction;

		/// <summary>
		/// Returns a reference to the character that initiated the dialog.
		/// </summary>
		public PlayerCharacter Player { get; }

		/// <summary>
		/// Returns a reference to the NPC the player is talking to.
		/// </summary>
		public Npc Npc { get; }

		/// <summary>
		/// Gets or sets the dialog's current state.
		/// </summary>
		public DialogState State { get; set; }

		/// <summary>
		/// Creates and prepares a new dialog between the player and
		/// the NPC.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="npc"></param>
		public Dialog(PlayerCharacter player, Npc npc)
		{
			this.Player = player;
			this.Npc = npc;
		}

		/// <summary>
		/// Starts dialog by calling the NPC's dialog function.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		public void Start()
		{
			if (this.Npc.DialogFunc == null)
				throw new InvalidOperationException($"NPC '{this.Npc.Name}' doesn't have a dialog function assigned to it.");

			this.Start(this.Npc.DialogFunc);
		}

		/// <summary>
		/// Starts dialog by calling the given function.
		/// </summary>
		/// <param name="dialogFunc"></param>
		public async void Start(DialogFunc dialogFunc)
		{
			if (this.State != DialogState.NotStarted)
				throw new InvalidOperationException("Dialog was already started.");

			this.State = DialogState.Active;

			try
			{
				await dialogFunc(this);

				// Try to choose the appropriate action to take after
				// the dialog function ended. A common issue one might
				// run into is that Close wasn't called before the script
				// ended, which could result in the player getting stuck.
				// We could simply call Close, but at least in the alpha
				// client this closes the dialog window immediately.
				// We want to wait for the player to click the last
				// button though, so we need a Next first. However,
				// if the user put a Next at the end of the script
				// for whatever reason, we would be calling it twice,
				// forcing the player to click twice before the dialog
				// closes, so we'll check what happened last during the
				// dialog, to see what we can do to clean everything up
				// neatly.

				if (_lastAction == DialogActionType.Message /*&& isAlpha*/)
				{
					await this.Next();
					this.Close();
				}
				else if (_lastAction == DialogActionType.Input)
				{
					this.Close();
				}
			}
			catch (OperationCanceledException)
			{
				// Thrown to get out of the async chain.
			}
			catch (Exception ex)
			{
				Log.Error("Dialog: During dialog between player '{0}' and NPC '{1}': {2}", this.Player.Name, this.Npc.Name, ex);
			}

			this.State = DialogState.Ended;
		}

		/// <summary>
		/// Sets response and resumes dialog after a Select.
		/// </summary>
		/// <param name="response"></param>
		public void Resume(string response)
		{
			_response = response;
			_resumeSignal.Release();
		}

		/// <summary>
		/// Returns delegates to common dialog functions for people
		/// who absolutely hate everything that is C# scripting and
		/// just want their good old eAthena functions.
		/// Wait, no, don't use it! This was just a joke! Damn it...
		/// </summary>
		/// <param name="mes"></param>
		/// <param name="next"></param>
		/// <param name="select"></param>
		public void AthenaFTW(out Action<string> mes, out Func<Task> next, out DialogSelectFunc select)
		{
			mes = this.Msg;
			next = this.Next;
			select = this.Select;
		}

		/// <summary>
		/// Returns delegates that translate strings to the language
		/// selected by the player.
		/// </summary>
		/// <param name="L"></param>
		/// <param name="LN"></param>
		public void PlayerLocalization(out Func<string, string> L, out Func<string, string, int, string> LN)
		{
			L = this.Player.Localizer.Get;
			LN = this.Player.Localizer.GetPlural;
		}

		/// <summary>
		/// Sends message to player, adding to any messages already
		/// in the dialog box..
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Msg(string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			this.Msg(format);
		}

		/// <summary>
		/// Sends a message to the player with the NPC's name,
		/// as typically placed before every message block.
		/// </summary>
		public void MsgNpcName()
		{
			//this.Msg("[^0000FF{0}^000000]", this.Npc.Name);
			this.Msg("[{0}]", this.Npc.Name);
		}

		/// <summary>
		/// Experimental version of Msg that supports paging "&lt;p/&gt;",
		/// line-breaks "&lt;br/&gt;", and automatically adds the name
		/// of the NPC as a title.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task Talk(string message)
		{
			var pages = message.Split(new string[] { "<p/>" }, StringSplitOptions.None);

			for (var i = 0; i < pages.Length; i++)
			{
				var page = pages[i];
				var paragraphs = page.Split(new string[] { "<br/>" }, StringSplitOptions.None);

				this.MsgNpcName();
				foreach (var paragraph in paragraphs)
					this.Msg(paragraph);

				//if (i < pages.Length - 1 || pages.Length == 1)
				await this.Next();
			}
		}

		/// <summary>
		/// Sends message to player, adding to any messages already
		/// in the dialog box..
		/// </summary>
		/// <param name="message"></param>
		public void Msg(string message)
		{
			_lastAction = DialogActionType.Message;
			Send.ZC_SAY_DIALOG(this.Player, this.Npc.Handle, message);
		}

		/// <summary>
		/// Display a continuation button on the dialog window and
		/// waits for the player to click it before resuming the
		/// script.
		/// </summary>
		/// <returns></returns>
		public async Task Next()
		{
			_lastAction = DialogActionType.Input;

			Send.ZC_WAIT_DIALOG(this.Player, this.Npc.Handle);

			this.State = DialogState.Waiting;
			await _resumeSignal.WaitAsync(_cancellation.Token);
			this.State = DialogState.Active;
		}

		/// <summary>
		/// Shows a menu with options to select from, returns the key
		/// of the option selected.
		/// </summary>
		/// <param name="options">List of options to select from.</param>
		/// <returns></returns>
		public async Task<string> Select(params DialogOption[] options)
		{
			// Go through SelectSimple to get the integer response
			// and then look up the key in the options to return it.

			var optionsString = string.Join(":", options.Select(a => a.Text));
			var intResponse = await this.SelectSimple(optionsString);
			string response;

			if (intResponse > options.Length)
			{
				Log.Warning("Dialog.Select: Unexpected out-of-range response '{0}/{1}'.", intResponse, options.Length);
				response = "__error_invalid_range__";
			}
			else
			{
				response = options[intResponse - 1].Key;
			}

			return response;
		}

		/// <summary>
		/// Shows a menu with options to select from. Returns the index
		/// of the selected option, starting at 1. Returns 0 in case
		/// of errors.
		/// </summary>
		/// <param name="options">List of options to select from. Every string is one option.</param>
		/// <returns></returns>
		public async Task<int> Select(params string[] options)
			=> await this.SelectSimple(string.Join(":", options));

		/// <summary>
		/// Shows a menu with options to select from and returns the
		/// of the selected option, starting at 1. Returns 0 in case
		/// of errors.
		/// </summary>
		/// <param name="optionsString"></param>
		/// <returns></returns>
		public async Task<int> SelectSimple(string optionsString)
		{
			_lastAction = DialogActionType.Input;

			Send.ZC_MENU_LIST(this.Player, this.Npc.Handle, optionsString);

			// Wait for response
			this.State = DialogState.Waiting;
			await _resumeSignal.WaitAsync(_cancellation.Token);
			this.State = DialogState.Active;

			// We'll presumably use the resume and response system for
			// all responses down the line, such as player input, which
			// will be sent as text, so we might as well use strings
			// right away. For the basic Select, however, we want an
			// integer response, as the index.

			if (!int.TryParse(_response, out var intResponse))
			{
				Log.Warning("Dialog.Menu: Unexpected non-integer response '{0}'.", _response);
				intResponse = 0;
			}

			return intResponse;
		}

		/// <summary>
		/// Closes the dialog.
		/// </summary>
		/// <exception cref="OperationCanceledException"></exception>
		public void Close()
		{
			_lastAction = DialogActionType.Close;

			Send.ZC_CLOSE_DIALOG(this.Player, this.Npc.Handle);
			throw new OperationCanceledException("Dialog closed by script.");
		}

		/// <summary>
		/// Opens a shop for the player, where the given items are
		/// for sale.
		/// </summary>
		/// <param name="shopName">Name of the shop to open.</param>
		/// <param name="type">Whether to show the buy/sell selection or go straight to one of them.</param>
		/// <exception cref="ArgumentException"></exception>
		public void OpenShop(string shopName, ShopOpenType type = ShopOpenType.BuyAndSell)
		{
			if (!ZoneServer.Instance.World.NpcShops.TryGet(shopName, out var shop))
				throw new ArgumentException($"Shop '{shopName}' not found.");

			this.OpenShop(shop, type);
		}

		/// <summary>
		/// Opens the shop for the player.
		/// </summary>
		/// <param name="shopName">The shop to open.</param>
		/// <param name="type">Whether to show the buy/sell selection or go straight to one of them.</param>
		public void OpenShop(NpcShop shop, ShopOpenType type = ShopOpenType.BuyAndSell)
		{
			switch (type)
			{
				case ShopOpenType.BuyAndSell:
				{
					Send.ZC_SELECT_DEALTYPE(this.Player, this.Npc.Handle);
					break;
				}
				case ShopOpenType.BuyOnly:
				{
					var items = shop.GetItems();
					Send.ZC_PC_PURCHASE_ITEMLIST(this.Player, items);
					break;
				}
				case ShopOpenType.SellOnly:
				{
					// TODO: Check for sellability?
					var items = this.Player.Inventory.GetItems();
					Send.ZC_PC_SELL_ITEMLIST(this.Player, items);
					break;
				}
			}

			this.Player.Vars.Temp.Set("Sabine.CurrentShop", shop);

			this.Close();
		}
	}

	/// <summary>
	/// A function that can be called to handle a dialog.
	/// </summary>
	/// <param name="dialog"></param>
	/// <returns></returns>
	public delegate Task DialogFunc(Dialog dialog);

	/// <summary>
	/// A function that returns a variable number of options and returns
	/// which one was selected.
	/// </summary>
	/// Necessary because you can't use params in Func/Action.
	/// <param name="options"></param>
	/// <returns></returns>
	public delegate Task<int> DialogSelectFunc(params string[] options);
}
