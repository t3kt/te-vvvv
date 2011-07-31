using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Animator.Common;
using TESharedAnnotations;

namespace Animator.AppCore.Tasks
{

	#region LockingTaskStack

	internal sealed class LockingTaskStack
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly ReaderWriterLockSlim _Lock;
		private readonly Stack<TaskBase> _Stack;
		private readonly EventHandler<TaskChangedEventArgs> _OnTaskChanged;

		#endregion

		#region Properties

		public int Count
		{
			get
			{
				this._Lock.EnterReadLock();
				try
				{
					return this._Stack.Count;
				}
				finally
				{
					this._Lock.ExitReadLock();
				}
			}
		}

		#endregion

		#region Events

		public event EventHandler Changed;

		private void OnChanged()
		{
			var handler = this.Changed;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Constructors

		public LockingTaskStack([CanBeNull]EventHandler<TaskChangedEventArgs> onTaskChanged)
		{
			this._Lock = new ReaderWriterLockSlim();
			this._Stack = new Stack<TaskBase>();
			this._OnTaskChanged = onTaskChanged;
		}

		#endregion

		#region Methods

		private void Attach(TaskBase task)
		{
			if(task != null)
				task.Changed += this._OnTaskChanged;
		}

		private void Detach(TaskBase task)
		{
			if(task != null)
			{
				task.Changed -= this._OnTaskChanged;
				task.Terminate();
			}
		}

		public void Clear()
		{
			bool changed;
			this._Lock.EnterWriteLock();
			try
			{
				changed = this._Stack.Count > 0;
				while(this._Stack.Count > 0)
					this.Detach(this._Stack.Pop());
			}
			finally
			{
				this._Lock.ExitWriteLock();
			}
			if(changed)
				this.OnChanged();
		}

		public void Push(TaskBase item)
		{
			this._Lock.EnterWriteLock();
			try
			{
				this.Attach(item);
				this._Stack.Push(item);
			}
			finally
			{
				this._Lock.ExitWriteLock();
			}
			this.OnChanged();
		}

		public bool TryPop(out TaskBase task)
		{
			this._Lock.EnterWriteLock();
			try
			{
				if(!CommonUtil.TryPop(this._Stack, out task))
					return false;
				this.Detach(task);
			}
			finally
			{
				this._Lock.ExitWriteLock();
			}
			this.OnChanged();
			return true;
		}

		public void ClearAt(TaskBase target)
		{
			bool changed;
			this._Lock.EnterWriteLock();
			try
			{
				var removed = CommonUtil.ClearStackAtTarget(this._Stack, target);
				foreach(var task in removed)
					this.Detach(task);
				changed = removed.Count > 0;
			}
			finally
			{
				this._Lock.ExitWriteLock();
			}
			if(changed)
				this.OnChanged();
		}

		public List<TaskBase> GetSnapshot()
		{
			this._Lock.EnterReadLock();
			try
			{
				return this._Stack.ToList();
			}
			finally
			{
				this._Lock.ExitReadLock();
			}
		}

		#endregion

	}

	#endregion

}
