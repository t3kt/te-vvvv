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
		public static readonly DependencyProperty BeatsPerMinuteProperty;
		public static readonly DependencyProperty DurationProperty;

		static TransportPropertiesDialog()
		{
			TransportTypeProperty = DependencyProperty.Register("TransportType", typeof(string), typeof(TransportPropertiesDialog));
			BeatsPerMinuteProperty = DependencyProperty.Register("BeatsPerMinute", typeof(float), typeof(TransportPropertiesDialog),
				new PropertyMetadata(Document.DefaultBeatsPerMinute), ValidateBeatsPerMinute);
			DurationProperty = DependencyProperty.Register("Duration", typeof(Time), typeof(TransportPropertiesDialog),
				new PropertyMetadata(Time.Infinite), ValidateDuration);
		}

		private static bool ValidateDuration(object value)
		{
			return !(0.0f > (Time)value);
		}

		private static bool ValidateBeatsPerMinute(object value)
		{
			return (float)value > 0;
		}

		public static void ShowDialogForDocument(Document document, Window ownerWindow = null)
		{
			Require.ArgNotNull(document, "document");
			var dlg = new TransportPropertiesDialog
					  {
						  Owner = ownerWindow,
						  TransportType = document.TransportType,
						  BeatsPerMinute = document.BeatsPerMinute,
						  Duration = document.Duration
					  };
			if(dlg.ShowDialog() == true)
			{
				document.TransportType = dlg.TransportType;
				document.BeatsPerMinute = dlg.BeatsPerMinute;
				document.Duration = dlg.Duration;
			}
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		// ReSharper disable MemberCanBeMadeStatic.Local
		public IEnumerable<KeyValuePair<string, string>> TransportTypeDescriptions
		{
			get { return Transport.TypeRegistry.RegisteredTypeDescriptionsByKey; }
		}
		// ReSharper restore MemberCanBeMadeStatic.Local

		public string TransportType
		{
			get { return (string)this.GetValue(TransportTypeProperty); }
			set { this.SetValue(TransportTypeProperty, value); }
		}

		public float BeatsPerMinute
		{
			get { return (float)this.GetValue(BeatsPerMinuteProperty); }
			set { this.SetValue(BeatsPerMinuteProperty, value); }
		}

		public Time Duration
		{
			get { return (Time)this.GetValue(DurationProperty); }
			set { this.SetValue(DurationProperty, value); }
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
