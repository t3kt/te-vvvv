using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Animator.UI.Dialogs;

namespace Animator.Test
{

	#region TestWindow1

	/// <summary>
	/// Interaction logic for TestWindow1.xaml
	/// </summary>
	public partial class TestWindow1 : Window
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		internal ObservableCollection<string> RecentFiles
		{
			get { return ((AniApplication)Application.Current).RecentFileManager.Files; }
		}

		#endregion

		#region Constructors

		public TestWindow1()
		{
			InitializeComponent();
		}

		#endregion

		private void showAddClipButton_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new AddClipDialog { Owner = this };
			dlg.ShowDialog();
		}

		private void attachDocToPropGridButton_Click(object sender, RoutedEventArgs e)
		{
			this.propgrid.SelectedObject = AniApplication.CurrentActiveDocument;
		}

		#region Methods

		#endregion

	}

	#endregion

}
