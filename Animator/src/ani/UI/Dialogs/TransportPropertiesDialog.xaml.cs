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
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using Animator.Core.Transport;

namespace Animator.UI.Dialogs
{
	/// <summary>
	/// Interaction logic for TransportPropertiesDialog.xaml
	/// </summary>
	public partial class TransportPropertiesDialog
	{

		#region Static / Constant

		public static readonly DependencyProperty TransportTypeProperty;

		static TransportPropertiesDialog()
		{
			TransportTypeProperty = DependencyProperty.Register("TransportType", typeof(string), typeof(TransportPropertiesDialog));
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		// ReSharper disable MemberCanBeMadeStatic.Local
		public IEnumerable<KeyValuePair<string, string>> TransportTypeDescriptions
		{
			get { return AniApplication.CurrentHost.GetTransportTypeDescriptionsByKey(); }
		}
		// ReSharper restore MemberCanBeMadeStatic.Local

		public string TransportType
		{
			get { return (string)this.GetValue(TransportTypeProperty); }
			set { this.SetValue(TransportTypeProperty, value); }
		}

		#endregion

		#region Constructors

		public TransportPropertiesDialog()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		#endregion

	}
}
