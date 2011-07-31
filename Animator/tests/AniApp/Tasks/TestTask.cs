using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.AppCore.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.AniApp.Tasks
{

	#region TestTask

	internal class TestTask : TaskBase
	{

		#region Static/Constant

		#endregion

		#region Fields

		public int _DoPerformCallCount;
		public int _DoUndoCallCount;
		public int _DoRedoCallCount;

		public TaskResult _PerformResult;
		public TaskResult _UndoResult;
		public TaskResult _RedoResult;

		public Func<object, TaskResult> _Perform;
		public Func<object, TaskResult> _Undo;
		public Func<object, TaskResult> _Redo;

		public object _Argument;

		public readonly TaskBase_Accessor _Accessor;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public TestTask(TaskCapabilities capabilities = TaskCapabilities.Redoable, string displayText = "test task", object argument = null)
			: base(capabilities, displayText)
		{
			this._Accessor = new TaskBase_Accessor(new PrivateObject(this));
			this._Argument = argument;
		}

		#endregion

		#region Methods

		protected override TaskResult DoPerform()
		{
			this._DoPerformCallCount++;
			if(this._Perform == null)
				return _PerformResult;
			return _Perform(this._Argument);
		}

		protected override TaskResult DoUndo()
		{
			this._DoUndoCallCount++;
			if(this._Undo == null)
				return _UndoResult;
			return _Undo(this._Argument);
		}

		protected override TaskResult DoRedo()
		{
			this._DoRedoCallCount++;
			if(this._Redo == null)
				return _RedoResult;
			return _Redo(this._Argument);
		}

		#endregion

	}

	#endregion

}
