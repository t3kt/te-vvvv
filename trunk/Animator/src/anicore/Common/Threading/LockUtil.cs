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

		[NotNull]
		public static IDisposable ReadScope([NotNull] this ReaderWriterLockSlim lok)
		{
			Require.ArgNotNull(lok, "lok");
			lok.EnterReadLock();
			return new ActionScope(lok.ExitReadLock);
		}

		[NotNull]
		public static IDisposable WriteScope([NotNull] this ReaderWriterLockSlim lok)
		{
			Require.ArgNotNull(lok, "lok");
			lok.EnterWriteLock();
			return new ActionScope(lok.ExitWriteLock);
		}

		[NotNull]
		public static IDisposable UpgradeableReadScope([NotNull] this ReaderWriterLockSlim lok)
		{
			Require.ArgNotNull(lok, "lok");
			lok.EnterUpgradeableReadLock();
			return new ActionScope(lok.ExitUpgradeableReadLock);
		}

	}

	#endregion

}
