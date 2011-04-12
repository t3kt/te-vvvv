using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Animator.AppCore
{

	#region AppActionHistoryManager

	internal sealed class AppActionHistoryManager : DispatcherObject
	{

		#region Record

		private struct Record
		{

			#region Static / Constant

			#endregion

			#region Fields

			public readonly IAppAction Action;
			public readonly object Target;
			public readonly object State;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public Record(IAppAction action, object target, object state)
			{
				this.Action = action;
				this.Target = target;
				this.State = state;
			}

			#endregion

			#region Methods

			#endregion

		}

		#endregion

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Stack<Record> _UndoRecords;

		#endregion

		#region Events

		public event EventHandler CanUndoChanged;

		private void OnCanUndoChanged()
		{
			var handler = this.CanUndoChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Properties

		public bool CanUndo
		{
			get
			{
				return this._UndoRecords.Count != 0 &&
					   this._UndoRecords.Peek().Action.SupportsUndo;
			}
		}

		public IAppAction TopAction
		{
			get
			{
				if(this._UndoRecords.Count == 0)
					return null;
				return this._UndoRecords.Peek().Action;
			}
		}

		#endregion

		#region Constructors

		public AppActionHistoryManager()
		{
			this._UndoRecords = new Stack<Record>();
		}

		#endregion

		#region Methods

		public void Clear()
		{
			var canUndo = this.CanUndo;
			this._UndoRecords.Clear();
			if(canUndo)
				this.OnCanUndoChanged();
		}

		public void PerformAndRecord(IAppAction action, object target, object newState)
		{
			if(action == null)
				return;
			var oldState = action.Perform(target, newState);
			PushUndoRecord(action, target, oldState);
		}

		public void PushUndoRecord(IAppAction action, object target, object oldState)
		{
			if(action == null)
				return;
			var canUndo = this.CanUndo;
			var record = new Record(action, target, oldState);
			this._UndoRecords.Push(record);
			if(canUndo != this.CanUndo)
				this.OnCanUndoChanged();
		}

		private bool TryPopUndoRecord(out Record record)
		{
			if(this._UndoRecords.Count > 0 && this._UndoRecords.Peek().Action.SupportsUndo)
			{
				record = this._UndoRecords.Pop();
				return true;
			}
			record = default(Record);
			return false;
		}

		public bool TryUndo()
		{
			Record record;
			var canUndo = this.CanUndo;
			if(!this.TryPopUndoRecord(out record))
				return false;
			record.Action.Undo(record.Target, record.State);
			if(canUndo != this.CanUndo)
				this.OnCanUndoChanged();
			return true;
		}

		#endregion

	}

	#endregion

}
