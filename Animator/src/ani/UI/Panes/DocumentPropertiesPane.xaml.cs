using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Animator.UI.Panes
{

	#region DocumentPropertiesPane

	/// <summary>
	/// Interaction logic for DocumentPropertiesPane.xaml
	/// </summary>
	public partial class DocumentPropertiesPane : AniPane
	{

		#region Static / Constant

		public static readonly DependencyProperty ShowCountsProperty;

		static DocumentPropertiesPane()
		{
			ShowCountsProperty = DependencyProperty.Register("ShowCounts", typeof(bool), typeof(DocumentPropertiesPane), new PropertyMetadata(false));
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public bool ShowCounts
		{
			get { return (bool)this.GetValue(ShowCountsProperty); }
			set { this.SetValue(ShowCountsProperty, value); }
		}

		#endregion

		#region Constructors

		public DocumentPropertiesPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
