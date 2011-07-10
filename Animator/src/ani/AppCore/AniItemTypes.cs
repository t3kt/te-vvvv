using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Animator.Core.Model;
using Animator.Core.Runtime;
using Animator.UI.Dialogs;
using Animator.UI.Editors;
using TESharedAnnotations;

namespace Animator.AppCore
{

	#region AniItemTypes

	internal static class AniItemTypes
	{

		[CanBeNull]
		private static ItemTypeInfo GetObjTypeInfo(object obj)
		{
			var item = obj as IDocumentItem;
			if(item == null) return null;
			Debug.Assert(item.ItemType.Type.IsInstanceOfType(item));
			return item.ItemType;
		}

		public static bool CanShowEditDetail(object obj)
		{
			var info = GetObjTypeInfo(obj);
			return info != null && info.CanEditDetail;
		}

		public static bool CanDelete(Document doc, object obj)
		{
			if(doc == null)
				return false;
			var info = GetObjTypeInfo(obj);
			return info != null && info.CanDelete;
		}

		public static bool CanDeleteNode(DocumentNode node)
		{
			if(node == null || node.Parent == null)
				return false;
			if(node is IDocumentItem)
			{
				var info = GetObjTypeInfo(node);
				return info != null && info.CanDelete;
			}
			return true;
		}

		public static bool ShowEditDetail(object obj)
		{
			if(!CanShowEditDetail(obj))
				return false;
			return ObjectEditorDialog.ShowForEditor(
				new PropertyGridObjectEditor
				{
					Target = obj,
					AutoCommit = true
				});
		}

		public static bool TryDelete(Document doc, object obj)
		{
			if(!CanDelete(doc, obj))
				return false;
			var item = obj as IDocumentItem;
			if(item == null)
				return false;
			return doc.TryDeleteItem(item);
		}

		public static bool TryDeleteNode(DocumentNode node)
		{
			if(!CanDeleteNode(node))
				return false;
			// ReSharper disable PossibleNullReferenceException
			return node.Parent.TryDeleteChild(node);
			// ReSharper restore PossibleNullReferenceException
		}

		internal static void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var itemControl = e.Source as FrameworkElement;
			if(itemControl != null)
			{
				if(ShowEditDetail(itemControl.DataContext))
					e.Handled = true;
			}
		}


	}

	#endregion

}
