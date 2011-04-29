using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Animator.Tests.Utils
{

	#region EventCounter

	internal sealed class EventCounter
	{

		private int _Count;

		public int Count
		{
			get { return this._Count; }
		}

#pragma warning disable 649
		public Action ExtraAction;
#pragma warning restore 649

		public readonly EventHandler Handler;

		public EventCounter()
		{
			this.Handler = this.HandlerImpl;
		}

		public void Reset()
		{
			this._Count = 0;
		}

		private void HandlerImpl(object sender, EventArgs e)
		{
			this._Count++;
			if(this.ExtraAction != null)
				this.ExtraAction();
		}

	}

	#endregion

	#region LockingEventCounter

	internal sealed class LockingEventCounter
	{

		private readonly ReaderWriterLockSlim _Lock;
		private int _Count;

		public int Count
		{
			get
			{
				try
				{
					this._Lock.EnterReadLock();
					return this._Count;
				}
				finally
				{
					this._Lock.ExitReadLock();
				}
			}
		}

		public Action ExtraAction;

		public readonly EventHandler Handler;

		public LockingEventCounter(ReaderWriterLockSlim lok = null)
		{
			this._Lock = lok ?? new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			this.Handler = this.HandlerImpl;
		}

		public void Reset()
		{
			try
			{
				this._Lock.EnterWriteLock();
				this._Count = 0;
			}
			finally
			{
				this._Lock.ExitWriteLock();
			}
		}

		private void HandlerImpl(object sender, EventArgs e)
		{
			try
			{
				this._Lock.EnterWriteLock();
				this._Count++;
				if(this.ExtraAction != null)
					this.ExtraAction();
			}
			finally
			{
				this._Lock.ExitWriteLock();
			}
		}

	}

	#endregion

}
