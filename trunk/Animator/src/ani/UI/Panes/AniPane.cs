using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Animator.UI.Panes
{

	#region AniPane

	public class AniPane : UserControl
	{

		#region Static / Constant

		public static readonly DependencyProperty PaneHeaderProperty;
		public static readonly DependencyProperty PaneHeaderVisibilityProperty;

		static AniPane()
		{
			PaneHeaderProperty = DependencyProperty.Register("PaneHeader", typeof(object), typeof(AniPane),
															 new FrameworkPropertyMetadata(null));
			PaneHeaderVisibilityProperty = DependencyProperty.Register("PaneHeaderVisibility", typeof(Visibility),
				typeof(AniPane), new PropertyMetadata(Visibility.Visible));
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Common)]
		public object PaneHeader
		{
			get { return this.GetValue(PaneHeaderProperty); }
			set { this.SetValue(PaneHeaderProperty, value); }
		}

		[Category(TEShared.Names.Category_Visibility)]
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
