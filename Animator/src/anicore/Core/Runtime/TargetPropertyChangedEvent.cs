using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Animator.Core.Runtime
{

	#region TargetPropertyChangedEvent

	public delegate void TargetPropertyChangedEventHandler(object sender, TargetPropertyChangedEventArgs e);

	public class TargetPropertyChangedEventArgs : RoutedEventArgs
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly object _Target;
		private readonly string _PropertyName;

		#endregion

		#region Properties

		public object Target
		{
			get { return this._Target; }
		}

		public string PropertyName
		{
			get { return this._PropertyName; }
		}

		#endregion

		#region Constructors

		public TargetPropertyChangedEventArgs(RoutedEvent routedEvent, object source, object target, string propertyName)
			: base(routedEvent, source)
		{
			this._Target = target;
			this._PropertyName = propertyName;
		}

		#endregion

		#region Methods

		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((TargetPropertyChangedEventHandler)genericHandler).Invoke(genericTarget, this);
		}

		#endregion

	}

	#endregion

}
