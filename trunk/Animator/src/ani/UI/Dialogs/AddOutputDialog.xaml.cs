using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.Core.IO;
using Animator.Core.Model;
using TESharedAnnotations;

namespace Animator.UI.Dialogs
{

	#region AddOutputDialog

	/// <summary>
	/// Interaction logic for AddOutputDialog.xaml
	/// </summary>
	public partial class AddOutputDialog
	{

		#region Static / Constant

		public static readonly DependencyProperty OutputTypeProperty;
		public static readonly DependencyProperty OutputNameProperty;

		static AddOutputDialog()
		{
			OutputTypeProperty = DependencyProperty.Register("OutputType", typeof(string), typeof(AddOutputDialog),
														   new PropertyMetadata(null));
			OutputNameProperty = DependencyProperty.Register("OutputName", typeof(string), typeof(AddOutputDialog),
														   new PropertyMetadata("New Output"));
		}

		public static Output ShowDialogForNewOutput(Window ownerWindow = null)
		{
			var dlg = new AddOutputDialog { Owner = ownerWindow };
			if(dlg.ShowDialog() != true)
				return null;
			return dlg.CreateOutput();
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		// ReSharper disable MemberCanBeMadeStatic.Local
		public IEnumerable<KeyValuePair<string, string>> OutputTypeDescriptions
		{
			get { return OutputTransmitter.GetRegisteredTypeDescriptionsByKey().ToArray(); }
		}
		// ReSharper restore MemberCanBeMadeStatic.Local

		public string OutputType
		{
			get { return (string)this.GetValue(OutputTypeProperty); }
			set { this.SetValue(OutputTypeProperty, value); }
		}

		public string OutputName
		{
			get { return (string)this.GetValue(OutputNameProperty); }
			set { this.SetValue(OutputNameProperty, value); }
		}

		#endregion

		#region Constructors

		public AddOutputDialog()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		public void Reset()
		{
			this.OutputType = null;
			this.OutputName = null;
		}

		[NotNull]
		public Output CreateOutput()
		{
			return new Output
				   {
					   OutputType = this.OutputType,
					   Name = this.OutputName
				   };
		}

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

	#endregion

}
