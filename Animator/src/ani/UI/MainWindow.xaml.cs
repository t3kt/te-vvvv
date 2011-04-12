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
using Animator.Core.Model;
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

		private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}

		private void ShowTestWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if(_TestWindow == null)
				_TestWindow = new TestWindow1 { Owner = this };
			_TestWindow.Show();
		}

		private void LoadTestDocumentCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var doc = new Document()
					  {
						  Name = "Test Document"
					  };
			var outputA = doc.CreateOutput(Guid.NewGuid());
			outputA.Name = "out A";
			doc.AddOutput(outputA);
			var outputB = doc.CreateOutput(Guid.NewGuid());
			outputB.Name = "out B";
			doc.AddOutput(outputB);
			var trackA = doc.CreateTrack(Guid.NewGuid());
			trackA.Name = "track A";
			doc.AddTrack(trackA);
			this.DataContext = doc;
		}

		#region Methods

		#endregion

	}

	#endregion

}
