using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.AppCore.Tasks
{

	#region TaskState

	public enum TaskState
	{
		NotPerformed,
		Performed,
		Undone
	}

	#endregion

	#region TaskCapabilities

	public enum TaskCapabilities
	{
		None,
		Undoable,
		Redoable
	}

	#endregion

	#region TaskResult

	public enum TaskResult
	{
		Cancelled,
		Completed,
		Failed,
		NoTask
	}

	#endregion

	#region TaskBase

	public abstract class TaskBase
	{

		#region Static / Constant

		public static TaskBase CreateAnonymous<T>([NotNull]string displayText, T argument, [NotNull]Func<T, TaskResult> perform, Func<T, TaskResult> undo, bool canRedo = true)
		{
			return new AnonymousTask<T>(displayText, argument, perform, undo, canRedo);
		}

		public static TaskBase CreateAnonymous<T>([NotNull]string displayText, T argument, [NotNull]Func<T, TaskResult> perform, Func<T, TaskResult> undo, Func<T, TaskResult> redo)
		{
			return new AnonymousTask<T>(displayText, argument, perform, undo, redo);
		}

		#endregion

		#region Fields

		private readonly string _DisplayText;
		private TaskCapabilities _Capabilities;
		private TaskState _State;
		private bool _Terminated;

		#endregion

		#region Properties

		public string DisplayText
		{
			get { return this._DisplayText; }
		}

		public TaskState State
		{
			get { return this._State; }
			private set
			{
				this.AssertNotTerminated("Set State");
				if(value != this._State)
				{
					if(value == TaskState.NotPerformed)
						throw new ArgumentException("State cannot be set to NotPerformed after task has been performed");
					var oldState = this._State;
					this._State = value;
					this.OnChanged(new TaskChangedEventArgs(this, oldState));
				}
			}
		}

		public TaskCapabilities Capabilities
		{
			get { return this._Capabilities; }
			protected set
			{
				this.AssertNotTerminated("Set Capabilities");
				if(value != this._Capabilities)
				{
					var oldCaps = this._Capabilities;
					this._Capabilities = value;
					this.OnChanged(new TaskChangedEventArgs(this, oldCaps));
				}
			}
		}

		public bool CanUndo
		{
			get { return !this._Terminated && this._State == TaskState.Performed && this._Capabilities >= TaskCapabilities.Undoable; }
		}

		public bool CanRedo
		{
			get { return !this._Terminated && this._State == TaskState.Undone && this._Capabilities == TaskCapabilities.Redoable; }
		}

		#endregion

		#region Events

		internal event EventHandler<TaskChangedEventArgs> Changed;

		private void OnChanged(TaskChangedEventArgs e)
		{
			this.AssertNotTerminated("Publish Changed Event");
			var handler = this.Changed;
			if(handler != null)
				handler(this, e);
		}

		#endregion

		#region Constructors

		protected TaskBase(TaskCapabilities capabilities, [NotNull] string displayText)
		{
			Require.ArgNotNullOrEmpty(displayText, "displayText");
			this._Capabilities = capabilities;
			this._DisplayText = displayText;
		}

		#endregion

		#region Methods

		private void AssertNotTerminated(string operation)
		{
			if(this._Terminated)
				throw new InvalidOperationException(String.Format("Operation invalid when task has been terminated: {0}", operation));
		}

		internal void Terminate()
		{
			this.AssertNotTerminated("Terminate");
			this._Terminated = true;
		}

		public void RevokeUndoRedoCapability()
		{
			this.AssertNotTerminated("RevokeUndoRedoCapability");
			this.Capabilities = TaskCapabilities.None;
		}

		public void RevokeRedoCapability()
		{
			this.AssertNotTerminated("RevokeRedoCapability");
			if(this._Capabilities >= TaskCapabilities.Redoable)
				this.Capabilities = TaskCapabilities.Undoable;
		}

		protected abstract TaskResult DoPerform();

		protected abstract TaskResult DoUndo();

		protected virtual TaskResult DoRedo()
		{
			return this.DoPerform();
		}

		public TaskResult Perform()
		{
			this.AssertNotTerminated("Perform");
			if(this._State != TaskState.NotPerformed)
			{
				//throw new InvalidOperationException("Task has already been initially performed");
				return TaskResult.NoTask;
			}
			var result = this.DoPerform();
			if(result == TaskResult.Completed)
				this.State = TaskState.Performed;
			return result;
		}

		public virtual TaskResult Undo()
		{
			this.AssertNotTerminated("Undo");
			if(!this.CanUndo)
				return TaskResult.NoTask;
			var result = this.DoUndo();
			if(result == TaskResult.Completed)
				this.State = TaskState.Undone;
			return result;
		}

		public virtual TaskResult Redo()
		{
			this.AssertNotTerminated("Redo");
			if(!this.CanRedo)
				return TaskResult.NoTask;
			var result = this.DoRedo();
			if(result == TaskResult.Completed)
				this.State = TaskState.Performed;
			return result;
		}

		#endregion

	}

	#endregion

}
