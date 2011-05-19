using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
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

		public static readonly DependencyProperty ActiveDocumentProperty;

		public static readonly DependencyProperty ActiveClipProperty;

		private static readonly DependencyPropertyKey RowsPropertyKey;
		public static readonly DependencyProperty RowsProperty;
		private static readonly DependencyPropertyKey ColumnsPropertyKey;
		public static readonly DependencyProperty ColumnsProperty;

		static DocumentClipGridPane()
		{
			ActiveDocumentProperty = AniUIManager.ActiveDocumentProperty.AddOwner(typeof(DocumentClipGridPane),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnActiveDocumentChanged));

			RowsPropertyKey = DependencyProperty.RegisterReadOnly("Rows", typeof(int), typeof(DocumentClipGridPane),
				new PropertyMetadata(0, null, CoerceDimension));
			RowsProperty = RowsPropertyKey.DependencyProperty;

			ColumnsPropertyKey = DependencyProperty.RegisterReadOnly("Columns", typeof(int), typeof(DocumentClipGridPane),
				new PropertyMetadata(0, null, CoerceDimension));
			ColumnsProperty = ColumnsPropertyKey.DependencyProperty;

			ActiveClipProperty = AniUIManager.ActiveClipProperty.AddOwner(typeof(DocumentClipGridPane));
		}

		private static object CoerceDimension(DependencyObject d, object basevalue)
		{
			if(basevalue != null)
			{
				if(basevalue is int)
					return Math.Max(1, (int)basevalue);
				if(basevalue is int?)
					return Math.Max(1, ((int?)basevalue).Value);
			}
			return 1;
		}

		private static void OnActiveDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AniUIManager.OnActiveDocumentChanged(d, e);
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

		public Document ActiveDocument
		{
			get { return (Document)this.GetValue(ActiveDocumentProperty); }
			set { this.SetValue(ActiveDocumentProperty, value); }
		}

		public Clip ActiveClip
		{
			get { return (Clip)this.GetValue(ActiveClipProperty); }
			set { this.SetValue(ActiveClipProperty, value); }
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
