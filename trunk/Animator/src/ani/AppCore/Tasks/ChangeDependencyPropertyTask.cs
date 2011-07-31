using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.AppCore.Tasks
{

	#region ChangeDependencyPropertyTask

	public sealed class ChangeDependencyPropertyTask : TaskBase
	{

		#region Static / Constant

		private static string GetDefaultDisplayText(DependencyProperty property)
		{
			return String.Format("Change {0}", property == null ? "Property" : property.Name);
		}

		#endregion

		#region Fields

		private readonly DependencyObject _Target;
		private readonly DependencyProperty _Property;
		private readonly object _NewValue;
		private readonly object _OldValue;
		private bool _Failed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ChangeDependencyPropertyTask([NotNull]DependencyObject target, [NotNull]DependencyProperty property, [CanBeNull]object newValue, TaskCapabilities capabilities = TaskCapabilities.Redoable, [CanBeNull] string displayText = null)
			: base(capabilities, displayText ?? GetDefaultDisplayText(property))
		{
			Require.ArgNotNull(target, "target");
			Require.ArgNotNull(property, "property");
			this._Target = target;
			this._Property = property;
			this._NewValue = newValue;
			this.TryGetTargetValue(out this._OldValue);
		}

		public ChangeDependencyPropertyTask([NotNull]DependencyObject target, [NotNull]DependencyProperty property, [CanBeNull]object newValue, [CanBeNull]object currentValue = null, TaskCapabilities capabilities = TaskCapabilities.Redoable, [CanBeNull] string displayText = null)
			: base(capabilities, displayText ?? GetDefaultDisplayText(property))
		{
			Require.ArgNotNull(target, "target");
			Require.ArgNotNull(property, "property");
			this._Target = target;
			this._Property = property;
			this._NewValue = newValue;
			this._OldValue = currentValue;
		}

		#endregion

		#region Methods

		private bool TryGetTargetValue(out object value)
		{
			if(!this._Failed)
			{
				try
				{
					value = this._Target.GetValue(this._Property);
					return true;
				}
				catch
				{
					this._Failed = true;
				}
			}
			value = null;
			return false;
		}

		private bool TrySetTargetValue(object value)
		{
			if(!this._Failed)
			{
				try
				{
					this._Target.SetValue(this._Property, value);
					return true;
				}
				catch
				{
					this._Failed = true;
				}
			}
			return false;
		}

		protected override TaskResult DoPerform()
		{
			if(this._Failed)
				return TaskResult.NoTask;
			if(this.TrySetTargetValue(this._NewValue))
			{
				return TaskResult.Completed;
			}
			this.RevokeUndoRedoCapability();
			return TaskResult.Failed;
		}

		protected override TaskResult DoUndo()
		{
			if(this._Failed)
				return TaskResult.NoTask;
			if(this.TrySetTargetValue(this._OldValue))
			{
				return TaskResult.Completed;
			}
			this.RevokeRedoCapability();
			return TaskResult.Failed;
		}

		#endregion

	}

	#endregion

}
