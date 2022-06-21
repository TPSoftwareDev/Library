using System;
using System.Threading;

namespace Teleperformance.Threading
{
	public static partial class ReaderWriterLockSlimExtensions
	{
		public const int DefaultTryMillisecondsTimeout = 10 * 1000;

		public static void TryExecuteWithReadLock(this ReaderWriterLockSlim readerWriterLockSlim, Action action, TimeSpan timeout)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.TryEnterReadLock(timeout);
				action();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLock();
			}
		}

		public static void TryExecuteWithReadLock(this ReaderWriterLockSlim readerWriterLockSlim, Action action, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.TryEnterReadLock(millisecondsTimeout);
				action();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLock();
			}
		}

		public static TResult TryReturnWithReadLock<TResult>(this ReaderWriterLockSlim readerWriterLockSlim, Func<TResult> func, TimeSpan timeout)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.TryEnterReadLock(timeout);

				return func();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLock();
			}
		}

		public static TResult TryReturnWithReadLock<TResult>(this ReaderWriterLockSlim readerWriterLockSlim, Func<TResult> func, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.TryEnterReadLock(millisecondsTimeout);

				return func();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLock();
			}
		}

		public static void TryExecuteWithReadLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, Action action, TimeSpan timeout)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.TryEnterReadLockIf(condition, timeout);
				action();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLockIf(condition);
			}
		}

		public static void TryExecuteWithReadLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, Action action, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.TryEnterReadLockIf(condition, millisecondsTimeout);
				action();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLockIf(condition);
			}
		}

		public static TResult TryReturnWithReadLockIf<TResult>(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, Func<TResult> func, TimeSpan timeout)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.TryEnterReadLockIf(condition, timeout);

				return func();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLockIf(condition);
			}
		}

		public static TResult TryReturnWithReadLockIf<TResult>(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, Func<TResult> func, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			try
			{
				readerWriterLockSlim.TryEnterReadLockIf(condition, millisecondsTimeout);

				return func();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLockIf(condition);
			}
		}

		public static void ExecuteWithReadLock(this ReaderWriterLockSlim readerWriterLockSlim, Action action)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.EnterReadLock();
				action();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLock();
			}
		}

		public static TResult ReturnWithReadLock<TResult>(this ReaderWriterLockSlim readerWriterLockSlim, Func<TResult> func)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.EnterReadLock();

				return func();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLock();
			}
		}

		public static void TryExecuteWithWriteLock(this ReaderWriterLockSlim WriteerWriterLockSlim, Action action, TimeSpan timeout)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.TryEnterWriteLock(timeout);
				action();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static void TryExecuteWithWriteLock(this ReaderWriterLockSlim WriteerWriterLockSlim, Action action, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.TryEnterWriteLock(millisecondsTimeout);
				action();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static TResult TryReturnWithWriteLock<TResult>(this ReaderWriterLockSlim WriteerWriterLockSlim, Func<TResult> func, TimeSpan timeout)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.TryEnterWriteLock(timeout);

				return func();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static TResult TryReturnWithWriteLock<TResult>(this ReaderWriterLockSlim WriteerWriterLockSlim, Func<TResult> func, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.TryEnterWriteLock(millisecondsTimeout);

				return func();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static void TryExecuteWithWriteLockIf(this ReaderWriterLockSlim WriteerWriterLockSlim, bool condition, Action action, TimeSpan timeout)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.TryEnterWriteLockIf(condition, timeout);
				action();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static void TryExecuteWithWriteLockIf(this ReaderWriterLockSlim WriteerWriterLockSlim, bool condition, Action action, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.TryEnterWriteLockIf(condition, millisecondsTimeout);
				action();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static TResult TryReturnWithWriteLockIf<TResult>(this ReaderWriterLockSlim WriteerWriterLockSlim, bool condition, Func<TResult> func, TimeSpan timeout)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.TryEnterWriteLockIf(condition, timeout);

				return func();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static TResult TryReturnWithWriteLockIf<TResult>(this ReaderWriterLockSlim WriteerWriterLockSlim, bool condition, Func<TResult> func, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			try
			{
				WriteerWriterLockSlim.TryEnterWriteLockIf(condition, millisecondsTimeout);

				return func();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static void ExecuteWithWriteLock(this ReaderWriterLockSlim WriteerWriterLockSlim, Action action)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.EnterWriteLock();
				action();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static TResult ReturnWithWriteLock<TResult>(this ReaderWriterLockSlim WriteerWriterLockSlim, Func<TResult> func)
		{
			if (null == WriteerWriterLockSlim)
			{
				throw new ArgumentNullException("WriteerWriterLockSlim");
			}

			try
			{
				WriteerWriterLockSlim.EnterWriteLock();

				return func();
			}
			finally
			{
				WriteerWriterLockSlim.ExitWriteLock();
			}
		}

		public static void ExecuteWithReadLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, Action action)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.EnterReadLockIf(condition);
				action();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLockIf(condition);
			}
		}

		public static void ExecuteWithWriteLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, Action action)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.EnterWriteLockIf(condition);
				action();
			}
			finally
			{
				readerWriterLockSlim.ExitWriteLockIf(condition);
			}
		}

		public static TResult ReturnWithReadLockIf<TResult>(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, Func<TResult> func)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.EnterReadLockIf(condition);

				return func();
			}
			finally
			{
				readerWriterLockSlim.ExitReadLockIf(condition);
			}
		}

		public static TResult ReturnWithWriteLockIf<TResult>(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, Func<TResult> func)
		{
			if (null == readerWriterLockSlim)
			{
				throw new ArgumentNullException("readerWriterLockSlim");
			}

			try
			{
				readerWriterLockSlim.EnterWriteLockIf(condition);

				return func();
			}
			finally
			{
				readerWriterLockSlim.ExitWriteLockIf(condition);
			}
		}

		public static void EnterReadLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition)
		{
			if (condition)
			{
				if (null == readerWriterLockSlim)
				{
					throw new ArgumentNullException("readerWriterLockSlim");
				}

				readerWriterLockSlim.EnterReadLock();
			}
		}

		public static void ExitReadLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition)
		{
			if (condition)
			{
				if (null == readerWriterLockSlim)
				{
					throw new ArgumentNullException("readerWriterLockSlim");
				}

				readerWriterLockSlim.ExitReadLock();
			}
		}

		public static void EnterWriteLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition)
		{
			if (condition)
			{
				if (null == readerWriterLockSlim)
				{
					throw new ArgumentNullException("readerWriterLockSlim");
				}

				readerWriterLockSlim.EnterWriteLock();
			}
		}

		public static void ExitWriteLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition)
		{
			if (condition)
			{
				if (null == readerWriterLockSlim)
				{
					throw new ArgumentNullException("readerWriterLockSlim");
				}

				readerWriterLockSlim.ExitWriteLock();
			}
		}

		public static void TryEnterReadLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, TimeSpan timeout)
		{
			if (condition)
			{
				if (null == readerWriterLockSlim)
				{
					throw new ArgumentNullException("readerWriterLockSlim");
				}

				readerWriterLockSlim.TryEnterReadLock(timeout);
			}
		}

		public static void TryEnterReadLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			if (condition)
			{
				if (null == readerWriterLockSlim)
				{
					throw new ArgumentNullException("readerWriterLockSlim");
				}

				readerWriterLockSlim.TryEnterReadLock(millisecondsTimeout);
			}
		}

		public static void TryEnterWriteLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, TimeSpan timeout)
		{
			if (condition)
			{
				if (null == readerWriterLockSlim)
				{
					throw new ArgumentNullException("readerWriterLockSlim");
				}

				readerWriterLockSlim.TryEnterWriteLock(timeout);
			}
		}

		public static void TryEnterWriteLockIf(this ReaderWriterLockSlim readerWriterLockSlim, bool condition, int millisecondsTimeout = ReaderWriterLockSlimExtensions.DefaultTryMillisecondsTimeout)
		{
			if (condition)
			{
				if (null == readerWriterLockSlim)
				{
					throw new ArgumentNullException("readerWriterLockSlim");
				}

				readerWriterLockSlim.TryEnterWriteLock(millisecondsTimeout);
			}
		}
	}
}
