using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TESharedAnnotations;

namespace Animator.Core.Runtime.ObjectStates
{

	#region StatePropertyDescriptor

	internal sealed class StatePropertyDescriptor : PropertyDescriptor
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly PropertyDescriptor _OriginalProperty;

		#endregion

		#region Properties

		internal PropertyDescriptor OriginalProperty
		{
			get { return this._OriginalProperty; }
		}

		public override Type ComponentType
		{
			get { return typeof(ObjectState); }
		}

		public override bool IsReadOnly
		{
			get { return this._OriginalProperty.IsReadOnly; }
		}

		public override Type PropertyType
		{
			get { return this._OriginalProperty.PropertyType; }
		}

		#endregion

		#region Constructors

		public StatePropertyDescriptor([NotNull] PropertyDescriptor originalProperty)
			: base(originalProperty)
		{
			this._OriginalProperty = originalProperty;
		}

		#endregion

		#region Methods

		public void CopyValueToState(object target, ObjectState state)
		{
			if(target == null || state == null)
				return;
			var value = this._OriginalProperty.GetValue(target);
			state.SetProperty(this.Name, value);
		}

		public void CopyValueFromState(ObjectState state, object target)
		{
			if(target == null || state == null || this.IsReadOnly)
				return;
			var value = state.GetProperty(this.Name);
			this._OriginalProperty.SetValue(target, value);
		}

		public override bool CanResetValue(object component)
		{
			return true;
		}

		public override object GetValue(object component)
		{
			var state = (ObjectState)component;
			if(state == null)
				return null;
			return state.GetProperty(this.Name);
		}

		public override void ResetValue(object component)
		{
			var state = (ObjectState)component;
			if(state == null)
				return;
			this.CopyValueToState(state.Target, state);
		}

		public override void SetValue(object component, object value)
		{
			var state = (ObjectState)component;
			if(state == null)
				return;
			state.SetProperty(this.Name, value);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}

		public override int GetHashCode()
		{
			return this._OriginalProperty.GetHashCode();
		}

		#endregion

	}

	#endregion

}
