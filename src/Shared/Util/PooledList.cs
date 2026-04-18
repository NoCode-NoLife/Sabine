using System;
using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Sabine.Shared.Util
{
	/// <summary>
	/// A List implementation that takes its lists from a shared pool to
	/// minimize allocations.
	/// </summary>
	/// <remarks>
	/// The class functions the exact same as a normal List, except that
	/// it takes its internal data from a shared pool. After use, the list
	/// needs to be disposed, so it can be returned to the pool. It's
	/// intended for temporary lists that used in hot paths, and should
	/// not be used for long-term storage.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class PooledList<T> : List<T>, IDisposable
	{
		private static readonly ObjectPool<PooledList<T>> Pool = new DefaultObjectPool<PooledList<T>>(new ListPoolPolicy(), 5000);

		private class ListPoolPolicy : IPooledObjectPolicy<PooledList<T>>
		{
			public PooledList<T> Create()
			{
				return new PooledList<T>();
			}

			public bool Return(PooledList<T> list)
			{
				if (list.Capacity >= 1024)
				{
					list.Clear();
					return false;
				}

				list.Clear();
				return true;
			}
		}

		private bool _disposed;

		/// <summary>
		/// Prevent external instantiation.
		/// </summary>
		private PooledList() : base()
		{
		}

		/// <summary>
		/// Returns a list from the pool.
		/// </summary>
		/// <remarks>
		/// It's highly recommended to use this method in a using
		/// statement, so the list will be returned to the pool
		/// semi-automatically.
		/// </remarks>
		/// <returns></returns>
		public static PooledList<T> Rent()
		{
			var list = Pool.Get();
			list._disposed = false;
			return list;
		}

		/// <summary>
		/// Disposes the list and returns it to the pool. After calling
		/// this method, the list should not be used anymore.
		/// </summary>
		public void Dispose()
		{
			if (_disposed)
				return;

			_disposed = true;
			Pool.Return(this);
		}
	}
}
