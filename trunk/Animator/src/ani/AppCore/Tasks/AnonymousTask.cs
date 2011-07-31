using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.AppCore.Tasks
{

	#region AnonymousTask<T>

	internal sealed class AnonymousTask<T> : TaskBase
	{

		#region Static / Constant

		private static TaskCapabilities DetermineCapabilities(bool canUndo, bool canRedo)
		{
			return canUndo ? (canRedo ? TaskCapabilities.Redoable : TaskCapabilities.Undoable) : TaskCapabilities.None;
		}

		#endregion

		#region Fields

		private readonly Func<T, TaskResult> _Perform;
		private readonly Func<T, TaskResult> _Undo;
		private readonly Func<T, TaskResult> _Redo;

		private readonly T _Argument;

		#endregion

		#region Properties

		public T Argument
		{
			get { return this._Argument; }
		}

		#endregion

		#region Constructors

		public AnonymousTask([NotNull]string displayText, T argument, [NotNull]Func<T, TaskResult> perform, Func<T, TaskResult> undo, bool canRedo = true)
			: base(DetermineCapabilities(undo != null, canRedo), displayText)
		{
			Require.ArgNotNull(perform, "perform");
			this._Argument = argument;
			this._Perform = perform;
			this._Undo = undo;
		}

		public AnonymousTask([NotNull]string displayText, T argument, [NotNull]Func<T, TaskResult> perform, Func<T, TaskResult> undo, Func<T, TaskResult> redo)
			: base(DetermineCapabilities(undo != null, redo != null), displayText)
		{
			Require.ArgNotNull(perform, "perform");
			this._Argument = argument;
			this._Perform = perform;
			this._Undo = undo;
			this._Redo = redo;
		}

		#endregion

		#region Methods

		protected override TaskResult DoPerform()
		{
			return this._Perform(this.Argument);
		}

		protected override TaskResult DoUndo()
		{
			if(this._Undo == null)
				return TaskResult.NoTask;
			return this._Undo(this.Argument);
		}

		protected override TaskResult DoRedo()
		{
			if(this._Redo != null)
				return this._Redo(this.Argument);
			return this.DoPerform();
		}

		#endregion

	}

	#endregion

}
