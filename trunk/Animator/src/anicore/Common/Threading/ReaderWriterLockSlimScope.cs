using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Animator.Common.Diagnostics;
using Animator.Resources;
using TESharedAnnotations;

namespace Animator.Common.Threading
{

	#region ReaderWriterLockSlimScope

	/// <summary>
	/// Base class for scopes that hold various types of
	/// locks on a <see cref="ReaderWriterLockSlim"/>.
	/// </summary>
	internal abstract class ReaderWriterLockSlimScope : IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ReaderWriterLockSlim _Lock;
		private bool _Entered;
		private bool _Exited;

		#endregion

		#region Properties

		/// <summary>
		/// The <see cref="ReaderWriterLockSlim"/> on which the scope is based.
		/// </summary>
		public ReaderWriterLockSlim Lock { get { return this._Lock; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Create a new <see cref="ReaderWriterLockSlimScope"/> based on <paramref name="lok"/>,
		/// and <see cref="Enter"/> the scope.
		/// </summary>
		/// <param name="lok"></param>
		protected ReaderWriterLockSlimScope([NotNull] ReaderWriterLockSlim lok)
		{
			Require.ArgNotNull(lok, "lok");
			this._Lock = lok;
			this.Enter();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Enter the lock on the <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		protected abstract void EnterLock();

		/// <summary>
		/// Exit the lock on the <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		protected abstract void ExitLock();

		/// <summary>
		/// Enter the scope, claiming the lock, if the scope has not
		/// yet been entered.
		/// </summary>
		public void Enter()
		{
			if(!this._Entered && !this._Exited)
			{
				this.EnterLock();
				this._Entered = true;
			}
		}

		/// <summary>
		/// Exists the scope, releasing the lock, if the scope has been
		/// entered but not exited.
		/// </summary>
		public void Exit()
		{
			if(this._Entered && !this._Exited)
			{
				this.ExitLock();
				this._Exited = true;
			}
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			this.Exit();
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

	#region ReaderWriterLockSlimReadScope

	/// <summary>
	/// A scope in which a read lock is held on a <see cref="ReaderWriterLockSlim"/>.
	/// </summary>
	internal sealed class ReaderWriterLockSlimReadScope : ReaderWriterLockSlimScope
	{

		#region Static / Constant

		private static ReaderWriterLockSlim ValidateLock(ReaderWriterLockSlim lok)
		{
			Require.ArgNotNull(lok, "lok");
			if(lok.RecursionPolicy != LockRecursionPolicy.SupportsRecursion &&
				(lok.IsReadLockHeld || lok.IsUpgradeableReadLockHeld))
				throw new ArgumentException(CoreStrings.LockReadScope_NonRecursiveWithLockNotAllowed, "lok");
			//Require.ArgValid(lok, "lok",
			//                 l => l.RecursionPolicy == LockRecursionPolicy.SupportsRecursion ||
			//                      (!l.IsReadLockHeld && !l.IsUpgradeableReadLockHeld),
			//                 CommonErrorStrings.Utilities.NonRecursiveLockHoldsWritableLock());
			return lok;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new <see cref="ReaderWriterLockSlimReadScope"/> based on <paramref name="lok"/>,
		/// and enters the scope, claiming a read lock.
		/// </summary>
		/// <param name="lok"></param>
		public ReaderWriterLockSlimReadScope([NotNull]ReaderWriterLockSlim lok)
			: base(ValidateLock(lok)) { }

		#endregion

		#region Methods

		/// <summary>
		/// Enter the read lock on the <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		/// <seealso cref="ReaderWriterLockSlim.EnterReadLock"/>
		protected override void EnterLock()
		{
			this.Lock.EnterReadLock();
		}

		/// <summary>
		/// Exit the write lock on the <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		/// <seealso cref="ReaderWriterLockSlim.ExitReadLock"/>
		protected override void ExitLock()
		{
			this.Lock.ExitReadLock();
		}

		#endregion

	}

	#endregion

	#region ReaderWriterLockSlimWriteScope

	/// <summary>
	/// A scope in which a write lock is held on a <see cref="ReaderWriterLockSlim"/>.
	/// </summary>
	internal sealed class ReaderWriterLockSlimWriteScope : ReaderWriterLockSlimScope
	{

		#region Static / Constant

		private static ReaderWriterLockSlim ValidateLock(ReaderWriterLockSlim lok)
		{
			Require.ArgNotNull(lok, "lok");
			if(lok.RecursionPolicy != LockRecursionPolicy.SupportsRecursion &&
				(lok.IsWriteLockHeld || lok.IsUpgradeableReadLockHeld))
				throw new ArgumentException(CoreStrings.LockWriteScope_NonRecursiveWithWriteLockNotAllowed, "lok");
			//Require.ArgValid(lok, "lok",
			//    l => l.RecursionPolicy == LockRecursionPolicy.SupportsRecursion ||
			//        (!l.IsWriteLockHeld && !l.IsUpgradeableReadLockHeld),
			//    CommonErrorStrings.Utilities.NonRecursiveLockHoldsWritableLock());
			return lok;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new <see cref="ReaderWriterLockSlimWriteScope"/> based on <paramref name="lok"/>,
		/// and enters the scope, claiming a write lock.
		/// </summary>
		/// <param name="lok"></param>
		public ReaderWriterLockSlimWriteScope([NotNull] ReaderWriterLockSlim lok)
			: base(ValidateLock(lok)) { }

		#endregion

		#region Methods

		/// <summary>
		/// Enter the read lock on the <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		/// <seealso cref="ReaderWriterLockSlim.EnterWriteLock"/>
		protected override void EnterLock()
		{
			this.Lock.EnterWriteLock();
		}

		/// <summary>
		/// Exit the write lock on the <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		/// <seealso cref="ReaderWriterLockSlim.ExitWriteLock"/>
		protected override void ExitLock()
		{
			this.Lock.ExitWriteLock();
		}

		#endregion

	}

	#endregion

	#region ReaderWriterLockSlimUpgradeableReadScope

	/// <summary>
	/// A scope in which an upgradeable read lock is held on a <see cref="ReaderWriterLockSlim"/>.
	/// </summary>
	internal sealed class ReaderWriterLockSlimUpgradeableReadScope : ReaderWriterLockSlimScope
	{

		#region Static / Constant

		private static ReaderWriterLockSlim ValidateLock(ReaderWriterLockSlim lok)
		{
			Require.ArgNotNull(lok, "lok");
			if(lok.RecursionPolicy != LockRecursionPolicy.SupportsRecursion &&
				(lok.IsWriteLockHeld || lok.IsUpgradeableReadLockHeld || lok.IsReadLockHeld))
				throw new ArgumentException(CoreStrings.LockUpgradeableReadScope_NonRecursiveWithLockNotAllowed, "lok");
			//Require.ArgValid(lok, "lok",
			//                 l => l.RecursionPolicy == LockRecursionPolicy.SupportsRecursion ||
			//                      (!l.IsWriteLockHeld && !l.IsUpgradeableReadLockHeld && !l.IsReadLockHeld),
			//                 CommonErrorStrings.Utilities.NonRecursiveLockHoldsWritableLock());
			return lok;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new <see cref="ReaderWriterLockSlimUpgradeableReadScope"/> based on <paramref name="lok"/>,
		/// and enters the scope, claiming an upgradeable read lock.
		/// </summary>
		/// <param name="lok"></param>
		public ReaderWriterLockSlimUpgradeableReadScope([NotNull] ReaderWriterLockSlim lok)
			: base(ValidateLock(lok)) { }

		#endregion

		#region Methods

		/// <summary>
		/// Enter the upgradeable read lock on the <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		/// <seealso cref="ReaderWriterLockSlim.EnterUpgradeableReadLock"/>
		protected override void EnterLock()
		{
			this.Lock.EnterUpgradeableReadLock();
		}

		/// <summary>
		/// Upgrades the lock on the <see cref="ReaderWriterLockSlim"/> to a write lock.
		/// </summary>
		public void UpgradeToWriteLock()
		{
			this.Lock.EnterWriteLock();
		}

		/// <summary>
		/// Exit the write lock on <see cref="ReaderWriterLockSlimScope.Lock"/>,
		/// if <see cref="ReaderWriterLockSlim.IsWriteLockHeld"/> is <see langword="null"/> (meaning
		/// the lock has been upgraded), then exits the read lock.
		/// </summary>
		/// <seealso cref="ReaderWriterLockSlim.ExitWriteLock"/>
		/// <seealso cref="ReaderWriterLockSlim.ExitUpgradeableReadLock"/>
		protected override void ExitLock()
		{
			if(this.Lock.IsWriteLockHeld)
				this.Lock.ExitWriteLock();
			this.Lock.ExitUpgradeableReadLock();
		}

		#endregion

	}

	#endregion

}
