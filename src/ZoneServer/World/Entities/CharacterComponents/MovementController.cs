using System;
using System.Collections.Generic;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Yggdrasil.Logging;

namespace Sabine.Zone.World.Entities.CharacterComponents
{
	/// <summary>
	/// Controls a character's movement.
	/// </summary>
	public class MovementController : IUpdateable
	{
		private readonly PlayerCharacter _playerCharacter;

		private readonly Queue<Position> _pathQueue = new Queue<Position>();
		private Position _finalDestination;
		private Position _nextDestination;
		private TimeSpan _currentMoveTime;
		private bool _moving;

		/// <summary>
		/// Returns the character this controller belongs to.
		/// </summary>
		public Character Character { get; }

		/// <summary>
		/// Returns true if the character is currently moving.
		/// </summary>
		public bool IsMoving => _moving;

		/// <summary>
		/// Returns the final destination of the character's current
		/// movement path.
		/// </summary>
		public Position Destination => _finalDestination;

		/// <summary>
		/// Creates new controller.
		/// </summary>
		/// <param name="character"></param>
		public MovementController(Character character)
		{
			this.Character = character;
			_playerCharacter = character as PlayerCharacter;
		}

		/// <summary>
		/// Makes character move to the given position.
		/// </summary>
		/// <param name="toPos"></param>
		public void MoveTo(Position toPos)
		{
			var character = this.Character;
			var fromPos = character.Position;
			var map = character.Map;

			// If we're currently moving, the start position needs to be
			// the next tile we move to, or the client will rubber-band
			// back to the tile it was just on.
			if (_moving)
				fromPos = _nextDestination;

			// Clear the current movement queue and fill it with the
			// new path.
			var path = map.PathFinder.FindPath(fromPos, toPos);
			if (path.Length == 0)
			{
				Log.Debug("Controller.MoveTo: No move path found between {0} and {1} on {2}.", fromPos, toPos, map.StringId);
				return;
			}

			_pathQueue.Clear();
			foreach (var pos in path)
				_pathQueue.Enqueue(pos);

			//Log.Debug("Moving from {0} to {1}.", fromPos, toPos);
			//for (var i = 0; i < path.Length; i++)
			//{
			//	var pos = path[i];
			//	Log.Debug("  {0}: {1}", i + 1, pos);
			//}

			// Start the move to the next tile if we aren't moving already.
			// If we are moving, we queued up the new path that starts at
			// the current destination, where we were currently headed.
			if (!_moving)
				this.NextMovement(TimeSpan.Zero);

			_finalDestination = toPos;

			Send.ZC_NOTIFY_MOVE(character, fromPos, toPos);

			// Not exactly happy about needing to treat player characters
			// differently in here, but since the client thinks it's a
			// great idea to have specific packets for player characters,
			// this is the simplest solution. Otherwise we'd need dedicated
			// controller classes for different character types, or every
			// character would need a connection.

			if (_playerCharacter != null)
				Send.ZC_NOTIFY_PLAYERMOVE(_playerCharacter, fromPos, toPos);
		}

		/// <summary>
		/// Stops character's current movement.
		/// </summary>
		public Position StopMove()
		{
			var character = this.Character;

			if (!_moving)
				return character.Position;

			var stopPos = _nextDestination;
			_pathQueue.Clear();
			_moving = false;

			//Log.Debug("Stopping at {0}", stopPos);

			// The client doesn't react to ZC_STOPMOVE for its controlled
			// character, so we need to send a move to make it stop.
			// Unless that's not desired? Maybe there should a be
			// forceStop parameter or something? TBD.

			Send.ZC_STOPMOVE(character, stopPos);

			if (_playerCharacter != null)
				Send.ZC_NOTIFY_PLAYERMOVE(_playerCharacter, character.Position, stopPos);

			return stopPos;
		}

		/// <summary>
		/// Updates the character's position based on its current movement.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			// Do nothing if we're not currently moving to a new tile
			if (!_moving)
				return;

			var character = this.Character;

			// Wait until enough time has passed to reach the next tile
			_currentMoveTime -= elapsed;
			if (_currentMoveTime > TimeSpan.Zero)
				return;

			// Update current position
			character.Position = _nextDestination;
			//Log.Debug("  now at {0}", this.Position);

			Npc npc = null;
			System.Threading.Tasks.Task.Run(() => (npc = new Npc(66)).Warp(character.Map.Id, character.Position));
			System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ => npc.Map.RemoveNpc(npc));

			this.OnReachedTile(character.Position);

			// Start next move if there's still something left in the queue
			if (_pathQueue.Count != 0)
			{
				this.NextMovement(TimeSpan.Zero - _currentMoveTime);
				return;
			}

			_moving = false;
		}

		/// <summary>
		/// Starts server-side move to the next tile in the character's
		/// movement queue.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		private void NextMovement(TimeSpan remainder)
		{
			if (_pathQueue.Count == 0)
				throw new InvalidOperationException("Path queue is empty.");

			var character = this.Character;
			_nextDestination = _pathQueue.Dequeue();

			var movingStright = character.Position.X == _nextDestination.X || character.Position.Y == _nextDestination.Y;
			var speed = (float)character.Parameters.Speed;

			// Speed appears to match diagonal movement, but it needs
			// to be adjusted for straight moves, or the character will
			// move slower on the server than on the client.
			// Athena uses 'speed * 14 / 10' to slow down diagonal movement
			// instead, so are characters possibly moving faster on the
			// alpha client? That would explain why Athena's default walk
			// speed value of 150 seems way too fast for the alpha client.
			// Meanwhile, the client seems to use '10 * speed / 13'...? But
			// our 'speed / 1.81f' seems to work well for the moment and gives
			// us ~111 for 200 speed, while the client's formula gives us
			// ~153, which is way off. This needs more research.
			// 
			// Update: Changed the divisor to 1.81, because 1.8 tended
			// to lack behind the client, causing a light rubberbanding.
			// This seems to be kind of impossible to get right. There
			// will always be a small difference between client and
			// server, causing the character to run fast or slower
			// at times. Ideally we would want to not send a start
			// position, so the client can figure that part out
			// itself, making it at least look smooth...
			// 
			// Update: After realizing that I'm stupid, and that I can't
			// start the next move with the full speed on every update,
			// because the update time is not consistent, I'm now
			// including the "remainder" in the next move time, to
			// compensate for the time that has already passed since
			// the move was actually finished (usually a few ms).
			// With this change, the new divisor is 1.307, or rather,
			// it matches the client's '10 * speed / 13', which seems
			// to work for the most part now, though it's still not
			// absolutely perfect. Diagonal movement meanwhile seems
			// to still work very well somehow.
			if (movingStright)
				speed = speed * 10f / 13f;

			_currentMoveTime = TimeSpan.FromMilliseconds(speed) - remainder;
			_moving = true;
		}

		/// <summary>
		/// Called when the character reached the given position while
		/// moving.
		/// </summary>
		/// <param name="position"></param>
		private void OnReachedTile(Position position)
		{
			// TODO: Add auto trigger system that we can check for things
			//   to do when stepping onto tiles.

			var character = this.Character;

			var warps = character.Map.GetAllNpcs(a => a.ClassId == 32 && a.Position.InSquareRange(position, 2));
			if (warps.Length > 0)
			{
				var warp = warps[0];
				character.Warp(warp.WarpDestination);
			}
		}
	}
}
