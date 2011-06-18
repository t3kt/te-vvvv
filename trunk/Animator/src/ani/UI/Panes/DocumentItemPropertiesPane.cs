using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.Model;

namespace Animator.UI.Panes
{

	#region DocumentItemPropertiesPane

	public class DocumentItemPropertiesPane : UserControl
	{

		#region Static / Constant

		public static readonly DependencyProperty DetailButtonVisibilityProperty =
			DependencyProperty.Register("DetailButtonVisibility", typeof (Visibility),
				typeof (DocumentItemPropertiesPane), new PropertyMetadata(Visibility.Collapsed));

		static DocumentItemPropertiesPane()
		{
			CommandManager.RegisterClassCommandBinding(typeof(DocumentItemPropertiesPane),
				new CommandBinding(AniAppCommands.ShowEditDetailCommand, ShowEditDetailCommand_Executed, ShowEditDetailCommand_CanExecute));
			IsTabStopProperty.OverrideMetadata(typeof(DocumentItemPropertiesPane), new FrameworkPropertyMetadata(true));
		}

		private static void ShowEditDetailCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			var pane = sender as DocumentItemPropertiesPane;
			e.CanExecute = pane != null && pane.CanShowEditDetail;
		}

		private static void ShowEditDetailCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var pane = sender as DocumentItemPropertiesPane;
			if(pane != null)
				pane.OnShowEditDetailCommand(e);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Visibility)]
		public Visibility DetailButtonVisibility
		{
			get { return (Visibility)this.GetValue(DetailButtonVisibilityProperty); }
			set { this.SetValue(DetailButtonVisibilityProperty, value); }
		}

		protected virtual bool CanShowEditDetail
		{
			get { return this.DataContext is IDocumentItem; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected virtual void OnShowEditDetailCommand(ExecutedRoutedEventArgs e)
		{
		}

		#endregion

	}

	#endregion

}
