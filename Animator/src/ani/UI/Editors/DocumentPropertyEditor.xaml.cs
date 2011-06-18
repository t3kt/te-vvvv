using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.Core.Composition;
using Animator.Core.Model;

namespace Animator.UI.Editors
{

	#region DocumentPropertyEditor

	/// <summary>
	/// Interaction logic for DocumentPropertyEditor.xaml
	/// </summary>
	[ObjectEditor(Description = "Document Property Editor", Key = "basic", TargetType = typeof(Document))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class DocumentPropertyEditor
	{

		#region Static/Constant

		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string),
			typeof(DocumentPropertyEditor), new PropertyMetadata(OnTargetNameChanged));

		private static void OnTargetNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (DocumentPropertyEditor)d;
			var doc = editor.Target as Document;
			if(editor.AutoCommit && doc != null)
				doc.Name = (string)e.NewValue;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public string TargetName
		{
			get { return (string)this.GetValue(TargetNameProperty); }
			set { this.SetValue(TargetNameProperty, value); }
		}

		#endregion

		#region Constructors

		public DocumentPropertyEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		protected override void PerformCommit()
		{
			var doc = this.Target as Document;
			if(doc != null)
			{
				doc.Name = this.TargetName;
			}
		}

		protected override void PerformReset()
		{
			var doc = this.Target as Document;
			if(doc == null)
			{
				this.TargetName = null;
			}
			else
			{
				this.TargetName = doc.Name;
			}
		}

		#endregion

	}

	#endregion

}
