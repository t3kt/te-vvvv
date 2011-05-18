using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Animator.Core.Model;

namespace Animator.AppCore
{

	#region AniUIManager

	internal sealed class AniUIManager : DependencyObject
	{

		internal static readonly DependencyProperty ActiveDocumentProperty;
		private static readonly DependencyPropertyKey HasActiveDocumentPropertyKey;
		public static readonly DependencyProperty HasActiveDocumentProperty;
		public static readonly DependencyProperty ActiveDocumentDirtyProperty;

		static AniUIManager()
		{
			ActiveDocumentProperty = DependencyProperty.Register("ActiveDocument", typeof(Document), typeof(AniUIManager),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnActiveDocumentChanged));
			HasActiveDocumentPropertyKey = DependencyProperty.RegisterReadOnly("HasActiveDocument", typeof(bool), typeof(AniUIManager),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
			HasActiveDocumentProperty = HasActiveDocumentPropertyKey.DependencyProperty;
			ActiveDocumentDirtyProperty = DependencyProperty.Register("ActiveDocumentDirty", typeof(bool), typeof(AniUIManager),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
		}

		internal static void OnActiveDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.SetValue(HasActiveDocumentPropertyKey, e.NewValue != null);
		}

	}

	#endregion

}
