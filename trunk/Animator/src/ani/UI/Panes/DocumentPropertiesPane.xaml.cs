using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.Model;

namespace Animator.UI.Panes
{

	#region DocumentPropertiesPane

	/// <summary>
	/// Interaction logic for DocumentPropertiesPane.xaml
	/// </summary>
	public partial class DocumentPropertiesPane
	{

		#region Static / Constant

		public static readonly DependencyProperty ActiveDocumentProperty = AniUIManager.ActiveDocumentProperty.AddOwner(typeof(DocumentPropertiesPane));

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Document Document
		{
			get { return (Document)this.DataContext; }
			set { this.DataContext = value; }
		}

		public Document ActiveDocument
		{
			get { return (Document)this.GetValue(ActiveDocumentProperty); }
			set { this.SetValue(ActiveDocumentProperty, value); }
		}

		protected override bool CanShowEditDetail
		{
			get { return this.Document != null; }
		}

		#endregion

		#region Constructors

		public DocumentPropertiesPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		protected override void OnShowEditDetailCommand(ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("TODO: document edit detail...");
		}

		#endregion

	}

	#endregion

}
