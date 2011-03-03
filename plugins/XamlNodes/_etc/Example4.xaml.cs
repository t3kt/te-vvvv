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

namespace XamlNodes._etc
{
	/// <summary>
	/// Interaction logic for Example4.xaml
	/// </summary>
	public partial class Example4
	{
		public Example4()
		{
			InitializeComponent();
		}

		private void ValETextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.ValEOutput.Value = this.ValETextBox.Text;
		}
	}
}
