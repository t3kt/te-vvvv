using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace OscSeq.UI
{
	#region MainWindow

	public partial class MainWindow : Window
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		#endregion

	}

	#endregion

}
