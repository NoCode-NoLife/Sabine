﻿using System;
using System.Collections.Generic;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;

namespace Sabine.Zone.World.Entities.Components.Characters
{
	/// <summary>
	/// Controls a character's movement.
	/// </summary>
	public class MovementController : ICharacterComponent
	{
		private readonly PlayerCharacter _playerCharacter;

		private readonly Queue<Position> _pathQueue = new();
		private Position _finalDestination;
		private Position _nextDestination;
		private bool _destinationChanged;
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
		/// Does nothing.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
		}

		/// <summary>
		/// Makes character move to the given position, returns the time
		/// the move will take.
		/// </summary>
		/// <param name="toPos"></param>
		/// <returns></returns>
		public TimeSpan MoveTo(Position toPos)
		{
			var character = this.Character;
			var map = character.Map;

			var fromPos = character.Position;
			if (fromPos == toPos)
				return TimeSpan.Zero;

			_finalDestination = toPos;

			character.StopAttacking();

			// If character is moving already, remember to calculate a new
			// path once they reach the next tile.
			if (_moving)
			{
				_destinationChanged = true;
				return TimeSpan.Zero;
			}

			// Clear the current movement queue and fill it with the
			// new path.
			var path = map.PathFinder.FindPath(fromPos, toPos);
			if (path.Length == 0)
			{
				Log.Debug("Controller.MoveTo: No move path found between {0} and {1} on {2}.", fromPos, toPos, map.StringId);
				return TimeSpan.Zero;
			}

			lock (_pathQueue)
			{
				_pathQueue.Clear();
				_pathQueue.AddRange(path);
			}

			// Start the move to the next tile
			this.StartMove();

			// Not exactly happy about needing to treat player characters
			// differently in here, but since the client thinks it's a
			// great idea to have specific packets for player characters,
			// this is the simplest solution. Otherwise we'd need dedicated
			// controller classes for different character types, or every
			// character would need a connection.

			Send.ZC_NOTIFY_MOVE(character, fromPos, toPos);

			if (_playerCharacter != null)
				Send.ZC_NOTIFY_PLAYERMOVE(_playerCharacter, fromPos, toPos);

			var result = TimeSpan.Zero;
			var prevPos = fromPos;

			foreach (var pos in path)
			{
				var moveDelay = (float)this.Character.Parameters.Speed;
				if (!pos.InStraightLine(prevPos))
					moveDelay *= 1.4f;

				result += TimeSpan.FromMilliseconds(moveDelay);
				prevPos = pos;
			}

			return result;
		}

		/// <summary>
		/// Takes a destination from the path queue and starts moving there.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		private void StartMove()
		{
			var character = this.Character;

			lock (_pathQueue)
			{
				if (_pathQueue.Count == 0)
					throw new InvalidOperationException("Path queue is empty.");

				_nextDestination = _pathQueue.Dequeue();
			}

			var movingStright = character.Position.InStraightLine(_nextDestination);
			var speed = (float)character.Parameters.Speed;

			// If you ever write your own server, and your movement is wonky,
			// check the following:
			// 1) Make sure the movement speed is right.
			// 2) Let your movement update run consistently or factor in
			//    any potential delays.
			// 3) Use valid ticks in your packets.

			if (!movingStright)
				speed *= 1.4f;

			_moving = true;

			// I tried running the controller on the heartbeat, but I wasn't
			// entirely happy with the results. Let's switch to a high-resolution
			// timer for now and see how that goes.
			ZoneServer.Instance.World.Scheduler.Schedule(speed, this.ExecuteMove);
		}

		/// <summary>
		/// Updates the character's position to the current destination
		/// tile and potentially starts the next move.
		/// </summary>
		/// <param name="state"></param>
		private void ExecuteMove(CallbackState state)
		{
			var character = this.Character;
			character.Position = _nextDestination;

			this.OnReachedTile(character.Position);

			if (_destinationChanged)
			{
				_destinationChanged = false;
				_moving = false;

				this.MoveTo(_finalDestination);
				return;
			}

			// Start next move if there's still something left in the queue
			lock (_pathQueue)
			{
				if (_pathQueue.Count != 0)
				{
					this.StartMove();
					return;
				}
			}

			// If this was the last destination in the queue, we're done
			// moving.
			_moving = false;
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

			lock (_pathQueue)
				_pathQueue.Clear();
			_moving = false;

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
		/// Called when the character reached the given position while
		/// moving.
		/// </summary>
		/// <param name="position"></param>
		private void OnReachedTile(Position position)
		{
			// TODO: Add auto trigger system that we can check for things
			//   to do when stepping onto tiles.

			var character = this.Character;

			// TODO: Add option for warping monsters
			if (character is Monster)
				return;

			var warps = character.Map.GetAllNpcs(a => a.ClassId == 45 && a.Position.InSquareRange(position, 2) && a.WarpDestination.MapId != 0);
			if (warps.Length > 0)
			{
				var warp = warps[0];
				character.Warp(warp.WarpDestination);
			}
		}
	}
}
