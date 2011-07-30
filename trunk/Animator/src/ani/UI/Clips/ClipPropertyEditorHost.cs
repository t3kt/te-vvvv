using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Animator.AppCore;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.UI.Clips
{
	public class ClipPropertyEditorHost : ContentControl
	{

		#region Static / Constant

		private static readonly Dictionary<Type, Type> _EditorTypes = new Dictionary<Type, Type>();

		[CanBeNull]
		internal static Type GetEditorType([CanBeNull] Type clipDataType)
		{
			//bool isReusable;
			//return GetEditorType(clipDataType, out isReusable);
			if(clipDataType == null)
				return null;
			Type editorType;
			if(_EditorTypes.TryGetValue(clipDataType, out editorType))
				return editorType;
			var attr = (ClipPropertyDataEditorAttribute)Attribute.GetCustomAttribute(clipDataType, typeof(ClipPropertyDataEditorAttribute));
			if(attr == null)
				return null;
			editorType = attr.GetEditorType();
			_EditorTypes.Add(clipDataType, editorType);
			return editorType;
		}

		[CanBeNull]
		internal static object GetEditor([CanBeNull]Type clipDataType)
		{
			var editorType = GetEditorType(clipDataType);
			return editorType == null ? null : Activator.CreateInstance(editorType);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ClipPropertyEditorHost()
		{
			this.DataContextChanged += this.Host_DataContextChanged;
		}

		#endregion

		#region Methods

		private void Host_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var type = e.NewValue == null ? null : e.NewValue.GetType();
			this.Content = GetEditor(type);
		}

		#endregion

	}
}
