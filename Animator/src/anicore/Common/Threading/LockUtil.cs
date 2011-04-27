using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Common.Threading
{

	#region LockUtil

	internal static class LockUtil
	{

		//[NotNull]
		//public static IDisposable ReadScope([NotNull] this ReaderWriterLockSlim lok)
		//{
		//    Require.ArgNotNull(lok, "lok");
		//    lok.EnterReadLock();
		//    return new ActionScope(lok.ExitReadLock);
		//}

		//[NotNull]
		//public static IDisposable WriteScope([NotNull] this ReaderWriterLockSlim lok)
		//{
		//    Require.ArgNotNull(lok, "lok");
		//    lok.EnterWriteLock();
		//    return new ActionScope(lok.ExitWriteLock);
		//}

		//[NotNull]
		//public static IDisposable UpgradeableReadScope([NotNull] this ReaderWriterLockSlim lok)
		//{
		//    Require.ArgNotNull(lok, "lok");
		//    lok.EnterUpgradeableReadLock();
		//    return new ActionScope(lok.ExitUpgradeableReadLock);
		//}

		/// <summary>
		/// Creates a <see cref="ReaderWriterLockSlimReadScope"/>, in which <paramref name="lok"/> holds
		/// a read lock.
		/// </summary>
		/// <param name="lok"></param>
		/// <returns></returns>
		public static ReaderWriterLockSlimReadScope ReadScope([NotNull]this ReaderWriterLockSlim lok)
		{
			//Require.ArgNotNull(lok, "lok");
			return new ReaderWriterLockSlimReadScope(lok);
		}

		/// <summary>
		/// Creates a <see cref="ReaderWriterLockSlimWriteScope"/>, in which <paramref name="lok"/> holds
		/// a write lock.
		/// </summary>
		/// <param name="lok"></param>
		/// <returns></returns>
		public static ReaderWriterLockSlimWriteScope WriteScope([NotNull]this ReaderWriterLockSlim lok)
		{
			//Require.ArgNotNull(lok, "lok");
			return new ReaderWriterLockSlimWriteScope(lok);
		}

		/// <summary>
		/// Creates a <see cref="ReaderWriterLockSlimUpgradeableReadScope"/>, in which <paramref name="lok"/> holds
		/// an upgradeable read lock.
		/// </summary>
		/// <param name="lok"></param>
		/// <returns></returns>
		public static ReaderWriterLockSlimUpgradeableReadScope UpgradeableReadScope([NotNull]this ReaderWriterLockSlim lok)
		{
			//Require.ArgNotNull(lok, "lok");
			return new ReaderWriterLockSlimUpgradeableReadScope(lok);
		}

	}

	#endregion

}
