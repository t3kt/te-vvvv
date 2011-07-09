using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Animator.AppCore;

namespace Animator.UI.Controls
{

	#region AniPane

	public class AniPane : UserControl
	{

		#region Static / Constant

		public static readonly DependencyProperty PaneHeaderProperty =
			AniUI.PaneHeaderProperty.AddOwner(typeof(AniPane));

		public static readonly DependencyProperty PaneHeaderVisibilityProperty =
			AniUI.PaneHeaderVisibilityProperty.AddOwner(typeof(AniPane));

		#endregion

		#region Fields

		#endregion

		#region Properties

		public object PaneHeader
		{
			get { return this.GetValue(PaneHeaderProperty); }
			set { this.SetValue(PaneHeaderProperty, value); }
		}

		public Visibility PaneHeaderVisibility
		{
			get { return (Visibility)this.GetValue(PaneHeaderVisibilityProperty); }
			set { this.SetValue(PaneHeaderVisibilityProperty, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
