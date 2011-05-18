using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.UI.Panes
{

	#region ClipPropertiesPane

	/// <summary>
	/// Interaction logic for ClipPropertiesPane.xaml
	/// </summary>
	public partial class ClipPropertiesPane
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ClipPropertiesPane()
		{
			InitializeComponent();
			this.DataContextChanged += this.ClipPropertiesPane_DataContextChanged;
		}

		void ClipPropertiesPane_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			this.clipPropertyGrid.SelectedObject = e.NewValue;
		}

		#endregion

		#region Methods

		#endregion
	}

	#endregion

}
