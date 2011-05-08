using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.Core.Model;

namespace Animator.UI.Editors
{

	#region DocumentPropertyEditor

	/// <summary>
	/// Interaction logic for DocumentPropertyEditor.xaml
	/// </summary>
	public partial class DocumentPropertyEditor
	{

		#region Static/Constant

		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string),
			typeof(DocumentPropertyEditor), new PropertyMetadata(OnTargetNameChanged));

		public static readonly DependencyProperty TargetUIRowsProperty = DependencyProperty.Register("TargetUIRows", typeof(int?),
			typeof(DocumentPropertyEditor), new PropertyMetadata(null, OnTargetUIRowsChanged));

		public static readonly DependencyProperty TargetUIColumnsProperty = DependencyProperty.Register("TargetUIColumns", typeof(int?),
			typeof(DocumentPropertyEditor), new PropertyMetadata(null, OnTargetUIColumnsChanged));

		private static void OnTargetNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (DocumentPropertyEditor)d;
			var doc = editor.Target as Document;
			if(editor.AutoCommit && doc != null)
				doc.Name = (string)e.NewValue;
		}

		private static void OnTargetUIRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (DocumentPropertyEditor)d;
			var doc = editor.Target as Document;
			if(editor.AutoCommit && doc != null)
				doc.UIRows = (int?)e.NewValue;
		}

		private static void OnTargetUIColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (DocumentPropertyEditor)d;
			var doc = editor.Target as Document;
			if(editor.AutoCommit && doc != null)
				doc.UIColumns = (int?)e.NewValue;
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

		public int? TargetUIRows
		{
			get { return (int?)this.GetValue(TargetUIRowsProperty); }
			set { this.SetValue(TargetUIRowsProperty, value); }
		}

		public int? TargetUIColumns
		{
			get { return (int?)this.GetValue(TargetUIColumnsProperty); }
			set { this.SetValue(TargetUIColumnsProperty, value); }
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
				doc.UIRows = this.TargetUIRows;
				doc.UIColumns = this.TargetUIColumns;
			}
		}

		protected override void PerformReset()
		{
			var doc = this.Target as Document;
			if(doc == null)
			{
				this.TargetName = null;
				this.TargetUIRows = null;
				this.TargetUIColumns = null;
			}
			else
			{
				this.TargetName = doc.Name;
				this.TargetUIRows = doc.UIRows;
				this.TargetUIColumns = doc.UIColumns;
			}
		}

		#endregion

	}

	#endregion

}
