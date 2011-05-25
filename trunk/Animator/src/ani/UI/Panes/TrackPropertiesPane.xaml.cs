using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.Core.Model;

namespace Animator.UI.Panes
{

	#region TrackPropertiesPane

	/// <summary>
	/// Interaction logic for TrackPropertiesPane.xaml
	/// </summary>
	public partial class TrackPropertiesPane
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public TrackPropertiesPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void DocumentItemPropertiesPane_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.trackPropertyGrid.SelectedObject = e.NewValue;
		}

		#endregion

	}

	#endregion

}
