using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.AppCore.Tasks
{

	#region ChangeObjectPropertyTask

	[Incomplete]
	public class ChangeObjectPropertyTask : TaskBase
	{

		#region Static / Constant

		private static string PrepareDefaultDisplayText(PropertyInfo propertyInfo)
		{
			return String.Format("Change {0}", propertyInfo != null ? propertyInfo.Name : "Property");
		}

		private static PropertyInfo GetPropertyInfo(object target, string propertyName)
		{
			Require.ArgNotNull(target, "target");
			Require.ArgNotNullOrEmpty(propertyName, "propertyName");
			var type = target.GetType();
			return type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		private static bool TryGetPropertyValue(object target, PropertyInfo propertyInfo, out object value)
		{
			if(propertyInfo == null || !propertyInfo.CanRead)
			{
				value = null;
				return false;
			}
			try
			{
				value = propertyInfo.GetValue(target, null);
				return true;
			}
			catch
			{
				value = null;
				return false;
			}
		}

		private static bool TrySetPropertyValue(object target, PropertyInfo propertyInfo, object value)
		{
			if(propertyInfo == null || !propertyInfo.CanWrite)
				return false;
			try
			{
				propertyInfo.SetValue(target, value, null);
				return true;
			}
			catch
			{
				return false;
			}
		}

		#endregion

		#region Fields

		private readonly object _Target;
		private readonly PropertyInfo _PropertyInfo;
		private object _PreviousValue;
		private object _NewValue;
		private bool _Failed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ChangeObjectPropertyTask(TaskCapabilities capabilities, [NotNull] object target, [NotNull]string propertyName, object newValue, string displayText = null)
			: this(capabilities, target, GetPropertyInfo(target, propertyName), newValue, displayText)
		{
			//Require.ArgNotNull(target, "target");
			//Require.ArgNotNullOrEmpty(propertyName, "propertyName");
		}

		public ChangeObjectPropertyTask(TaskCapabilities capabilities, [NotNull] object target, [NotNull]PropertyInfo propertyInfo, object newValue, string displayText = null)
			: base(capabilities, displayText ?? PrepareDefaultDisplayText(propertyInfo))
		{
			Require.ArgNotNull(target, "target");
			Require.ArgNotNull(propertyInfo, "propertyInfo");
			this._Target = target;
			this._PropertyInfo = propertyInfo;
			this._NewValue = newValue;
		}

		#endregion

		#region Methods

		protected override TaskResult DoPerform()
		{
			if(this._Failed)
				return TaskResult.Failed;
			if(this.Capabilities > TaskCapabilities.None)
			{

			}

			if(!TrySetPropertyValue(this._Target, this._PropertyInfo, this._NewValue))
			{
				this._Failed = true;
				return TaskResult.Failed;
			}
			throw new NotImplementedException();
		}

		protected override TaskResult DoUndo()
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
