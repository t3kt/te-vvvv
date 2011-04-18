using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Animator.UI.Dialogs
{
	/// <summary>
	/// Interaction logic for TrackPropertiesWindow.xaml
	/// </summary>
	public partial class TrackPropertiesWindow
	{
		public TrackPropertiesWindow()
		{
			InitializeComponent();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
