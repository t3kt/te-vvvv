using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Animator.Common.Diagnostics;
using Animator.Core.Model;

namespace Animator.AppCore
{

	#region AniUIManager

	internal sealed class AniUIManager : DependencyObject
	{

		public static readonly DependencyProperty ActiveDocumentProperty;
		private static readonly DependencyPropertyKey HasActiveDocumentPropertyKey;
		public static readonly DependencyProperty HasActiveDocumentProperty;
		public static readonly DependencyProperty ActiveDocumentDirtyProperty;

		public static readonly DependencyProperty ActiveClipProperty;
		private static readonly DependencyPropertyKey HasActiveClipPropertyKey;
		public static readonly DependencyProperty HasActiveClipProperty;

		static AniUIManager()
		{
			ActiveDocumentProperty = DependencyProperty.RegisterAttached("ActiveDocument", typeof(Document), typeof(AniUIManager),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnActiveDocumentChanged));
			HasActiveDocumentPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HasActiveDocument", typeof(bool), typeof(AniUIManager),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
			HasActiveDocumentProperty = HasActiveDocumentPropertyKey.DependencyProperty;

			ActiveDocumentDirtyProperty = DependencyProperty.RegisterAttached("ActiveDocumentDirty", typeof(bool), typeof(AniUIManager),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

			ActiveClipProperty = DependencyProperty.RegisterAttached("ActiveClip", typeof(Clip), typeof(AniUIManager),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnActiveClipChanged));
			HasActiveClipPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HasActiveClip", typeof(bool), typeof(AniUIManager),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
			HasActiveClipProperty = HasActiveClipPropertyKey.DependencyProperty;
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

		public static Clip GetActiveClip(DependencyObject d)
		{
			Require.ArgNotNull(d, "d");
			return (Clip)d.GetValue(ActiveClipProperty);
		}

		public static void SetActiveClip(DependencyObject d, Clip value)
		{
			Require.ArgNotNull(d, "d");
			d.SetValue(ActiveClipProperty, value);
		}

		internal static void OnActiveDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.SetValue(HasActiveDocumentPropertyKey, e.NewValue != null);
		}

		internal static void OnActiveClipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.SetValue(HasActiveClipPropertyKey, e.NewValue != null);
		}

	}

	#endregion

}