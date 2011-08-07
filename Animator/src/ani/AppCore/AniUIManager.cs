using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.WpfPropertyGrid;
using System.Windows.Media;
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using Animator.Core.Model.Clips;

namespace Animator.AppCore
{

	#region AniUI

	internal static class AniUI
	{

		#region Active Document / Items

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

		public static bool GetHasActiveDocument(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (bool)d.GetValue(HasActiveDocumentProperty);
		}

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

		public static readonly DependencyProperty SelectedSectionProperty =
			DependencyProperty.RegisterAttached("SelectedSection", typeof(DocumentSection), typeof(AniUI),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static DocumentSection GetSelectedSection(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (DocumentSection)d.GetValue(SelectedSectionProperty);
		}

		public static void SetSelectedSection(DependencyObject d, DocumentSection value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(SelectedSectionProperty, value);
		}

		public static readonly DependencyProperty SelectedClipProperty =
			DependencyProperty.RegisterAttached("SelectedClip", typeof(ClipBase), typeof(AniUI),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static ClipBase GetSelectedClip(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (ClipBase)d.GetValue(SelectedClipProperty);
		}

		public static void SetSelectedClip(DependencyObject d, ClipBase value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(SelectedClipProperty, value);
		}

		public static readonly DependencyProperty SelectedTrackProperty =
			DependencyProperty.RegisterAttached("SelectedTrack", typeof(Track), typeof(AniUI),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static Track GetSelectedTrack(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (Track)d.GetValue(SelectedTrackProperty);
		}

		public static void SetSelectedTrack(DependencyObject d, Track value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(SelectedTrackProperty, value);
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
				typeof(AniUI), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.Inherits));

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

		#region Misc

		public static readonly DependencyProperty ItemIconSourceProperty =
			DependencyProperty.RegisterAttached("ItemIconSource", typeof(ImageSource), typeof(AniUI),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static ImageSource GetItemIconSource(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (ImageSource)d.GetValue(ItemIconSourceProperty);
		}

		public static void SetItemIconSource(DependencyObject d, ImageSource value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(ItemIconSourceProperty, value);
		}

		public static readonly DependencyProperty DirtyProperty =
			DependencyProperty.RegisterAttached("Dirty", typeof(bool), typeof(AniUI), new FrameworkPropertyMetadata(false));

		public static bool GetDirty(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (bool)d.GetValue(DirtyProperty);
		}

		public static void SetDirty(DependencyObject d, bool value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(DirtyProperty, value);
		}

		#endregion

	}

	#endregion

}
