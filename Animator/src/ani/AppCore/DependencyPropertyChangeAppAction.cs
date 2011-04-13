using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Animator.Common.Diagnostics;

namespace Animator.AppCore
{

	#region DependencyPropertyChangeAppAction

	internal sealed class DependencyPropertyChangeAppAction : IAppAction
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly DependencyProperty _Property;
		private readonly bool _SupportsUndo;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public DependencyPropertyChangeAppAction(DependencyProperty property, bool supportsUndo = true)
		{
			Require.ArgNotNull(property, "property");
			this._Property = property;
			this._SupportsUndo = supportsUndo;
		}

		#endregion

		#region Methods

		#endregion

		#region IAppAction Members

		public string Name
		{
			get { return this._Property.Name; }
		}

		public void Perform(object target, object newState, out object oldState, out bool canUndo)
		{
			Require.ArgNotNull(target, "target");
			Require.ArgTypeIs<DependencyObject>(target, "target");
			var dtarget = (DependencyObject)target;
			oldState = this._SupportsUndo ? dtarget.GetValue(this._Property) : null;
			canUndo = this._SupportsUndo;
			dtarget.SetValue(this._Property, newState);
		}

		public void Undo(object target, object oldState)
		{
			if(!this._SupportsUndo)
				throw new NotSupportedException();
			Require.ArgNotNull(target, "target");
			Require.ArgTypeIs<DependencyObject>(target, "target");
			var dtarget = (DependencyObject)target;
			dtarget.SetValue(this._Property, oldState);
		}

		#endregion
	}

	#endregion

}
