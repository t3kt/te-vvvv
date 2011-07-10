using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.IO;
using Animator.Core.Model;

namespace Animator.UI.Panes
{
	/// <summary>
	/// Interaction logic for DocItemNavList.xaml
	/// </summary>
	public partial class DocItemNavList
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public DocItemNavList()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void Item_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			var itemControl = e.Source as FrameworkElement;
			if(itemControl != null)
			{
				if(AniItemTypes.ShowEditDetail(itemControl.DataContext))
					e.Handled = true;
			}
		}

		#endregion

	}
}
