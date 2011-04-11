using System;
using System.Collections;
using System.Collections.Generic;
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

		static AniPane()
		{
			PaneHeaderProperty = DependencyProperty.Register("PaneHeader", typeof(string), typeof(AniPane),
				new FrameworkPropertyMetadata(null));
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public string PaneHeader
		{
			get { return (string)this.GetValue(PaneHeaderProperty); }
			set { this.SetValue(PaneHeaderProperty, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
