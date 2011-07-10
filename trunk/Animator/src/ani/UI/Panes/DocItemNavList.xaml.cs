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

		private void Item_ShowEditDetailCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			AniItemTypes.ShowEditDetail(e.Parameter);
		}

		private void Item_ShowEditDetailCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AniItemTypes.CanShowEditDetail(e.Parameter);
		}

		private void Item_DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			AniItemTypes.TryDelete(this.DataContext as Document, e.Parameter);
		}

		private void Item_DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AniItemTypes.CanDelete(this.DataContext as Document, e.Parameter);
		}

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
