using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.AppCore.Tasks
{

	#region TaskManager

	public sealed class TaskManager
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly LockingTaskStack _UndoableTasks;
		private readonly LockingTaskStack _RedoableTasks;

		#endregion

		#region Properties

		public bool CanUndo
		{
			get { return this._UndoableTasks.Count > 0; }
		}

		public bool CanRedo
		{
			get { return this._RedoableTasks.Count > 0; }
		}

		public int UndoableTaskCount
		{
			get { return this._UndoableTasks.Count; }
		}

		public int RedoableTaskCount
		{
			get { return this._RedoableTasks.Count; }
		}

		public IEnumerable<TaskBase> UndoableTasks
		{
			get { return this._UndoableTasks.GetSnapshot(); }
		}

		public IEnumerable<TaskBase> RedoableTasks
		{
			get { return this._RedoableTasks.GetSnapshot(); }
		}

		#endregion

		#region Events

		public event EventHandler<CancellableTaskEventArgs> TaskPerforming;

		public event EventHandler<TaskEventArgs> TaskPerformed;

		public event EventHandler<CancellableTaskEventArgs> TaskUndoing;

		public event EventHandler<TaskEventArgs> TaskUndone;

		public event EventHandler<CancellableTaskEventArgs> TaskRedoing;

		public event EventHandler<TaskEventArgs> TaskRedone;

		public event EventHandler Cleared;

		public event EventHandler UndoStackChanged;

		public event EventHandler RedoStackChanged;

		private bool OnTaskPerforming(TaskBase task)
		{
			Debug.Assert(task != null);
			//var handler = this.TaskPerforming;
			//if(handler != null)
			//{
			//    var e = new CancellableTaskEventArgs(task);
			//    handler(this, e);
			//    return !e.IsCanceled;
			//}
			//return true;
			return CancellableTaskEventArgs.TryEventHandler(this.TaskPerforming, this, task);
		}

		private void OnTaskPerformed(TaskBase task)
		{
			var handler = this.TaskPerformed;
			if(handler != null)
				handler(this, new TaskEventArgs(task));
		}

		private bool OnTaskUndoing(TaskBase task)
		{
			Debug.Assert(task != null);
			//var handler = this.TaskUndoing;
			//if(handler != null)
			//{
			//    var e = new CancellableTaskEventArgs(task);
			//    handler(this, e);
			//    return !e.IsCanceled;
			//}
			//return true;
			return CancellableTaskEventArgs.TryEventHandler(this.TaskUndoing, this, task);
		}

		private void OnTaskUndone(TaskBase task)
		{
			var handler = this.TaskUndone;
			if(handler != null)
				handler(this, new TaskEventArgs(task));
		}

		private bool OnTaskRedoing(TaskBase task)
		{
			Debug.Assert(task != null);
			//var handler = this.TaskRedoing;
			//if (handler == null)
			//    return true;
			//var e = new CancellableTaskEventArgs(task);
			//handler(this, e);
			//return !e.IsCanceled;
			return CancellableTaskEventArgs.TryEventHandler(this.TaskRedoing, this, task);
		}

		private void OnTaskRedone(TaskBase task)
		{
			var handler = this.TaskRedone;
			if(handler != null)
				handler(this, new TaskEventArgs(task));
		}

		private void OnCleared()
		{
			var handler = this.Cleared;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Constructors

		public TaskManager()
		{
			this._UndoableTasks = new LockingTaskStack(this.Undoable_TaskChanged);
			this._UndoableTasks.Changed += this.Undoable_StackChanged;
			this._RedoableTasks = new LockingTaskStack(this.Redoable_TaskChanged);
			this._RedoableTasks.Changed += this.Redoable_StackChanged;
		}

		#endregion

		#region Methods

		private void Undoable_StackChanged(object sender, EventArgs e)
		{
			var handler = this.UndoStackChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		private void Redoable_StackChanged(object sender, EventArgs e)
		{
			var handler = this.RedoStackChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		private void Undoable_TaskChanged(object sender, TaskChangedEventArgs e)
		{
			if(!e.Task.CanUndo)
				this._UndoableTasks.ClearAt(e.Task);
		}

		private void Redoable_TaskChanged(object sender, TaskChangedEventArgs e)
		{
			if(!e.Task.CanRedo)
				this._RedoableTasks.ClearAt(e.Task);
		}

		public TaskResult Perform([NotNull] TaskBase task)
		{
			Require.ArgNotNull(task, "task");
			if(!this.OnTaskPerforming(task))
				return TaskResult.Cancelled;
			this._RedoableTasks.Clear();
			var result = task.Perform();
			if(result == TaskResult.Completed)
			{
				this.OnTaskPerformed(task);
				if(task.CanUndo)
					this._UndoableTasks.Push(task);
				else
					this._UndoableTasks.Clear();
			}
			return result;
		}

		public TaskResult Undo()
		{
			TaskBase task;
			if(!this._UndoableTasks.TryPop(out task))
				return TaskResult.NoTask;
			Debug.Assert(task != null);
			if(!this.OnTaskUndoing(task))
				return TaskResult.Cancelled;
			Debug.Assert(task.CanUndo);
			var result = task.Undo();
			if(result == TaskResult.Completed)
			{
				this.OnTaskUndone(task);
				if(task.CanRedo)
					this._RedoableTasks.Push(task);
				else
					this._RedoableTasks.Clear();
			}
			return result;
		}

		public TaskResult Redo()
		{
			TaskBase task;
			if(!this._RedoableTasks.TryPop(out task))
				return TaskResult.NoTask;
			Debug.Assert(task != null);
			if(!this.OnTaskRedoing(task))
				return TaskResult.Cancelled;
			Debug.Assert(task.CanRedo);
			var result = task.Redo();
			if(result == TaskResult.Completed)
			{
				this.OnTaskRedone(task);
				if(task.CanUndo)
					this._UndoableTasks.Push(task);
				else
					this._UndoableTasks.Clear();
			}
			return result;
		}

		public void Clear()
		{
			this._RedoableTasks.Clear();
			this._UndoableTasks.Clear();
			this.OnCleared();
		}

		#endregion

	}

	#endregion

}
