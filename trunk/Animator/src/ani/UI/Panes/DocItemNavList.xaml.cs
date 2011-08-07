using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
				AniAppCommands.EditDetail.Execute(itemControl.DataContext, itemControl);
		}

		private void EditDetailCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var item = e.Parameter as DocumentItem;
			if(item != null)
			{
				//if(ObjectEditorDialog.ShowPropertyGridForTarget(item, Window.GetWindow(this), "Edit " + item.GetType().Name))
				//    e.Handled = true;

				var stateEditor = new PropertyGridObjectStateEditor {Target = item};
				var result = ObjectEditorDialog.ShowForEditor(stateEditor, Window.GetWindow(this));
				e.Handled = result;
			}
		}

		#endregion

	}
}
