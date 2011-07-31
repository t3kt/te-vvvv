using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.AppCore.Tasks
{

	#region CompositeTask

	[Incomplete]
	internal class CompositeTask : TaskBase
	{

		#region Static / Constant

		private static TaskCapabilities DetermineCapabilities(ICollection<KeyValuePair<TaskBase, object>> tasks)
		{
			Require.ArgNotNull(tasks, "tasks");
			if(tasks.Count == 0)
				return TaskCapabilities.None;
			//var caps = TaskCapabilities.Redoable;
			//foreach(var task in tasks.Keys)
			//{
			//    if(task.Capabilities < caps)
			//        caps = task.Capabilities;
			//}
			return tasks.Min(t => t.Key.Capabilities);
		}

		#endregion

		#region Fields

		private readonly ICollection<KeyValuePair<TaskBase, object>> _Tasks;
		private readonly bool _Parallel;

		#endregion

		#region Properties

		public bool Parallel
		{
			get { return this._Parallel; }
		}

		#endregion

		#region Constructors

		public CompositeTask([NotNull] ICollection<KeyValuePair<TaskBase, object>> tasks, [NotNull] string displayText, bool parallel)
			: base(DetermineCapabilities(tasks), displayText)
		{
			this._Tasks = tasks;
			this._Parallel = parallel;
		}

		#endregion

		#region Methods

		private TaskResult DoSerialPerform()
		{
			throw new NotImplementedException();
		}

		private TaskResult DoSerialUndo()
		{
			throw new NotImplementedException();
		}

		private TaskResult DoSerialRedo()
		{
			throw new NotImplementedException();
		}

		private TaskResult DoParallelPerform()
		{
			throw new NotImplementedException();
		}

		private TaskResult DoParallelUndo()
		{
			throw new NotImplementedException();
		}

		private TaskResult DoParallelRedo()
		{
			throw new NotImplementedException();
		}

		protected override TaskResult DoPerform()
		{
			return this.Parallel ? this.DoParallelPerform() : this.DoSerialPerform();
		}

		protected override TaskResult DoUndo()
		{
			return this.Parallel ? this.DoParallelUndo() : this.DoSerialUndo();
		}

		protected override TaskResult DoRedo()
		{
			return this.Parallel ? this.DoParallelRedo() : this.DoSerialRedo();
		}

		#endregion

	}

	#endregion

}
