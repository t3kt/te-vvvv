using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Animator.AppCore;
using Animator.Core.Model;

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
	}
}
