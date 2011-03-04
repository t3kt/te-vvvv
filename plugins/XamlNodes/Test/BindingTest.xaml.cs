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

namespace XamlNodes.Test
{

	#region BindingTest

	/// <summary>
	/// Interaction logic for BindingTest.xaml
	/// </summary>
	public partial class BindingTest
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		private TestDataObject TestData
		{
			get { return (TestDataObject)this.FindResource("TestData"); }
		}

		#endregion

		#region Constructors

		public BindingTest()
		{
			InitializeComponent();
			((TestDataObject)this.FindResource("TestData")).Node = this;
		}

		#endregion

		private void ManualUpdateButton_Click(object sender, RoutedEventArgs e)
		{
			this.TestData.Str = this.ManualSourceTextBox.Text;
		}

		#region Methods

		#endregion

	}

	#endregion

}
