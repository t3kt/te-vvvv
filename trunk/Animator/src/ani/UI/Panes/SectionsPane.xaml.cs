using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.Model;
using Animator.UI.Dialogs;
using Animator.UI.Editors;

namespace Animator.UI.Panes
{
	/// <summary>
	/// Interaction logic for SectionsPane.xaml
	/// </summary>
	public partial class SectionsPane
	{

		public static readonly DependencyProperty SelectedSectionProperty;

		static SectionsPane()
		{
			SelectedSectionProperty = AniUI.SelectedSectionProperty.AddOwner(typeof(SectionsPane));
		}

		public DocumentSection SelectedSection
		{
			get { return (DocumentSection)this.GetValue(SelectedSectionProperty); }
			set { this.SetValue(SelectedSectionProperty, value); }
		}

		public SectionsPane()
		{
			InitializeComponent();
		}

		private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var doc = this.DataContext as Document;
			var section = e.Parameter as DocumentSection;
			if(doc != null && section != null && doc.Sections.Remove(section))
				e.Handled = true;
		}

	}
}
