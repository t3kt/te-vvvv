using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.UI.Dialogs;
using Animator.UI.Editors;

namespace Animator.UI.Panes
{
	/// <summary>
	/// Interaction logic for DocItemNavList.xaml
	/// </summary>
	public partial class DocItemNavList
	{

		#region Static / Constant

		private static bool IsSupportedType(object o)
		{
			return o is Output || o is DocumentSection;
		}

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

		#endregion

	}
}
