using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.WpfPropertyGrid;
using System.Windows.Input;
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using Animator.UI.Panes;

namespace Animator.AppCore
{

	#region AniUI

	internal static class AniUI
	{

		#region Active Document

		public static readonly DependencyProperty ActiveDocumentProperty =
			DependencyProperty.RegisterAttached("ActiveDocument", typeof(Document), typeof(AniUI),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnActiveDocumentChanged));

		private static readonly DependencyPropertyKey HasActiveDocumentPropertyKey =
			DependencyProperty.RegisterAttachedReadOnly("HasActiveDocument", typeof(bool), typeof(AniUI),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty HasActiveDocumentProperty = HasActiveDocumentPropertyKey.DependencyProperty;

		public static readonly DependencyProperty ActiveDocumentDirtyProperty =
			DependencyProperty.RegisterAttached("ActiveDocumentDirty", typeof(bool), typeof(AniUI),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		public static Document GetActiveDocument(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (Document)d.GetValue(ActiveDocumentProperty);
		}

		public static void SetActiveDocument(DependencyObject d, Document value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(ActiveDocumentProperty, value);
		}

		internal static void OnActiveDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.SetValue(HasActiveDocumentPropertyKey, e.NewValue != null);
		}

		#endregion

		#region PropertyGrid SelectedObject

		public static readonly DependencyProperty GridSelectedObjectProperty =
			DependencyProperty.RegisterAttached("GridSelectedObject", typeof(object), typeof(AniUI),
				new PropertyMetadata(null, OnGridSelectedObjectChanged));

		private static void OnGridSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var grid = d as PropertyGrid;
			if(grid != null)
				grid.SelectedObject = e.NewValue;
		}

		public static object GetGridSelectedObject(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return d.GetValue(GridSelectedObjectProperty);
		}

		public static void SetGridSelectedObject(DependencyObject d, object value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(GridSelectedObjectProperty, value);
		}

		#endregion

		#region Pane Headers

		public static readonly DependencyProperty PaneHeaderProperty =
			DependencyProperty.RegisterAttached("PaneHeader", typeof(object), typeof(AniUI));

		public static readonly DependencyProperty PaneHeaderVisibilityProperty =
			DependencyProperty.RegisterAttached("PaneHeaderVisibility", typeof(Visibility),
				typeof(AniUI), new PropertyMetadata(Visibility.Visible));

		public static object GetPaneHeader(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return d.GetValue(PaneHeaderProperty);
		}

		public static void SetPaneHeader(DependencyObject d, object value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(PaneHeaderProperty, value);
		}

		public static Visibility GetPaneHeadVisibility(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (Visibility)d.GetValue(PaneHeaderVisibilityProperty);
		}

		public static void SetPaneHeaderVisibility(DependencyObject d, Visibility value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(PaneHeaderVisibilityProperty, value);
		}

		#endregion

		#region Commands

		public static readonly RoutedUICommand ShowEditDetailCommand = new RoutedUICommand("Edit", "ShowEditDetail", typeof(DocumentItemPropertiesPane),
				new InputGestureCollection(new[] { new KeyGesture(Key.Enter, ModifierKeys.Alt) }));

		#endregion

	}

	#endregion

}
