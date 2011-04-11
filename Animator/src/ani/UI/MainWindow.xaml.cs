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
using System.Windows.Shapes;
using Animator.Test;

namespace Animator.UI
{

	#region MainWindow

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		#region Static / Constant

		#endregion

		#region Fields

		private TestWindow1 _TestWindow;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion

		private void ShowTestWindowMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if(_TestWindow == null)
				_TestWindow = new TestWindow1 { Owner = this };
			_TestWindow.Show();
		}

		#region Methods

		#endregion

	}

	#endregion

}
