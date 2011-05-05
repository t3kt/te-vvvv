using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.Core.Model;
using TESharedAnnotations;

namespace Animator.UI.Dialogs
{

	#region AddClipDialog

	/// <summary>
	/// Interaction logic for AddClipDialog.xaml
	/// </summary>
	public partial class AddClipDialog
	{

		#region Static / Constant

		public static readonly DependencyProperty ClipTypeProperty;
		public static readonly DependencyProperty ClipNameProperty;

		static AddClipDialog()
		{
			ClipTypeProperty = DependencyProperty.Register("ClipType", typeof(string), typeof(AddClipDialog),
														   new PropertyMetadata(null));
			ClipNameProperty = DependencyProperty.Register("ClipName", typeof(string), typeof(AddClipDialog),
														   new PropertyMetadata("New Clip"));
		}

		public static Clip ShowDialogForNewClip(Window ownerWindow = null)
		{
			var dlg = new AddClipDialog { Owner = ownerWindow };
			if(dlg.ShowDialog() != true)
				return null;
			return dlg.CreateClip();
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		// ReSharper disable MemberCanBeMadeStatic.Local
		public IEnumerable<KeyValuePair<string, string>> ClipTypeDescriptions
		{
			get
			{
				return AniApplication.CurrentHost.GetClipTypeDescriptionsByKey();
			}
		}
		// ReSharper restore MemberCanBeMadeStatic.Local

		public string ClipType
		{
			get { return (string)this.GetValue(ClipTypeProperty); }
			set { this.SetValue(ClipTypeProperty, value); }
		}

		public string ClipName
		{
			get { return (string)this.GetValue(ClipNameProperty); }
			set { this.SetValue(ClipNameProperty, value); }
		}

		#endregion

		#region Constructors

		public AddClipDialog()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Methods

		public void Reset()
		{
			this.ClipType = null;
			this.ClipName = null;
		}

		[NotNull]
		public Clip CreateClip()
		{
			var type = this.ClipType;
			var clip = AniApplication.CurrentHost.CreateClipByKey(type);
			Debug.Assert(clip != null);
			clip.Name = this.ClipName;
			return clip;
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
