using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.AppCore;
using Animator.Core.Model;

namespace Animator.UI.Session
{

	#region DocumentClipGridPane

	/// <summary>
	/// Interaction logic for DocumentClipGridPane.xaml
	/// </summary>
	public partial class DocumentClipGridPane
	{

		#region Static / Constant

		public static readonly DependencyProperty DocumentProperty;

		private static readonly DependencyPropertyKey RowsPropertyKey;
		public static readonly DependencyProperty RowsProperty;
		private static readonly DependencyPropertyKey ColumnsPropertyKey;
		public static readonly DependencyProperty ColumnsProperty;

		static DocumentClipGridPane()
		{
			DocumentProperty = DependencyProperty.Register("Document", typeof(Document), typeof(DocumentClipGridPane),
				new PropertyMetadata(null, OnDocumentChanged));

			RowsPropertyKey = DependencyProperty.RegisterReadOnly("Rows", typeof(int), typeof(DocumentClipGridPane),
				new PropertyMetadata(0), ValidateDimension);
			RowsProperty = RowsPropertyKey.DependencyProperty;

			ColumnsPropertyKey = DependencyProperty.RegisterReadOnly("Columns", typeof(int), typeof(DocumentClipGridPane),
				new PropertyMetadata(0), ValidateDimension);
			ColumnsProperty = ColumnsPropertyKey.DependencyProperty;
		}

		private static bool ValidateDimension(object value)
		{
			return value is int && (int)value >= 0;
		}

		private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var pane = (DocumentClipGridPane)d;
			var doc = (Document)e.NewValue;
			if(e.OldValue != null)
				((Document)e.OldValue).PropertyChanged -= pane.Document_PropertyChanged;
			if(doc != null)
				doc.PropertyChanged += pane.Document_PropertyChanged;
			pane.UpdateDimensions(doc);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Document Document
		{
			get { return (Document)this.GetValue(DocumentProperty); }
			set { this.SetValue(DocumentProperty, value); }
		}

		public int Rows
		{
			get { return (int)this.GetValue(RowsProperty); }
			private set { this.SetValue(RowsPropertyKey, value); }
		}

		public int Columns
		{
			get { return (int)this.GetValue(ColumnsProperty); }
			private set { this.SetValue(ColumnsPropertyKey, value); }
		}

		#endregion

		#region Constructors

		public DocumentClipGridPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void UpdateDimensions(Document document)
		{
			this.Rows = AniAppModelUtil.CalculateRows(document);
			this.Columns = AniAppModelUtil.CalculateColumns(document);
		}

		private void Document_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.UpdateDimensions(sender as Document);// ?? this.Document);
		}

		#endregion

	}

	#endregion

}
