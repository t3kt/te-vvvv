using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.UI.Dialogs;

namespace Animator.UI.Panes
{
	/// <summary>
	/// Interaction logic for OutputsPane.xaml
	/// </summary>
	public partial class OutputsPane : UserControl
	{
		public OutputsPane()
		{
			InitializeComponent();
		}

		private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var doc = this.DataContext as Document;
			var output = e.Parameter as Output;
			if(doc != null && output != null && doc.Outputs.Remove(output))
				e.Handled = true;
		}

	}
}
