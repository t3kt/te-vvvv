using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Animator.Core.IO;
using Animator.Core.Model;

namespace Animator.UI.Panes
{

	#region DocumentNavigationPane

	/// <summary>
	/// Interaction logic for DocumentNavigationPane.xaml
	/// </summary>
	public partial class DocumentNavigationPane
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public DocumentNavigationPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void OutputList_DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var doc = this.DataContext as Document;
			if(doc != null)
			{
				var output = e.Parameter as Output;
				if(output != null)
				{
					doc.Outputs.Remove(output);
					output.Dispose();
				}
			}
		}

		private void OutputList_ShowEditDetailCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var output = e.Parameter as Output;
			if(output != null)
			{
				AniApplication.ShowNotImplementedWarning();
			}
		}

		#endregion

	}

	#endregion

}
