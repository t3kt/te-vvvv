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
