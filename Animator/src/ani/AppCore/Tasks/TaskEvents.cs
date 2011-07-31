using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.AppCore.Tasks
{

	#region TaskEventArgs

	public class TaskEventArgs : EventArgs
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly TaskBase _Task;

		#endregion

		#region Properties

		public TaskBase Task
		{
			get { return this._Task; }
		}

		#endregion

		#region Constructors

		public TaskEventArgs([NotNull] TaskBase task)
		{
			Require.ArgNotNull(task, "task");
			this._Task = task;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region TaskChangedEventArgs

	internal sealed class TaskChangedEventArgs : TaskEventArgs
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly TaskState _OldState;
		private readonly TaskCapabilities _OldCapabilities;
		private readonly bool _StateChanged;
		private readonly bool _CapabilitiesChanged;

		#endregion

		#region Properties

		public TaskState OldState
		{
			get { return this._OldState; }
		}

		public TaskCapabilities OldCapabilities
		{
			get { return this._OldCapabilities; }
		}

		public bool StateChanged
		{
			get { return this._StateChanged; }
		}

		public bool CapabilitiesChanged
		{
			get { return this._CapabilitiesChanged; }
		}

		#endregion

		#region Constructors

		public TaskChangedEventArgs([NotNull] TaskBase task, TaskState oldState)
			: base(task)
		{
			this._OldState = oldState;
			this._OldCapabilities = task.Capabilities;
			this._StateChanged = true;
		}

		public TaskChangedEventArgs([NotNull] TaskBase task, TaskCapabilities oldCapabilities)
			: base(task)
		{
			this._OldState = task.State;
			this._OldCapabilities = oldCapabilities;
			this._CapabilitiesChanged = true;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region CancellableTaskEventArgs

	public class CancellableTaskEventArgs : TaskEventArgs
	{

		#region Static / Constant

		internal static bool TryEventHandler(EventHandler<CancellableTaskEventArgs> handler, object sender, [NotNull]TaskBase task)
		{
			if(handler == null)
				return true;
			var e = new CancellableTaskEventArgs(task);
			handler(sender, e);
			return !e.IsCanceled;
		}

		#endregion

		#region Fields

		private bool _IsCanceled;

		#endregion

		#region Properties

		public bool IsCanceled
		{
			get { return this._IsCanceled; }
		}

		#endregion

		#region Constructors

		public CancellableTaskEventArgs([NotNull] TaskBase task)
			: base(task) { }

		#endregion

		#region Methods

		public void Cancel()
		{
			this._IsCanceled = true;
		}

		#endregion

	}

	#endregion

}
