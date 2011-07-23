using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Animator.Core.Model.Clips;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.AppCore
{

	#region ClipPropertyDataEditors

	internal static class ClipPropertyDataEditors
	{

		#region TypeRec

		private sealed class TypeRec
		{

			#region Static/Constant

			#endregion

			#region Fields

			public readonly Type Type;
			public readonly bool IsReusable;

			private IClipPropertyDataEditor _Instance;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public TypeRec(Type type, bool isReusable)
			{
				Debug.Assert(typeof(IClipPropertyDataEditor).IsAssignableFrom(type));
				this.Type = type;
				this.IsReusable = isReusable;
			}

			#endregion

			#region Methods

			public IClipPropertyDataEditor GetOrCreateInstance()
			{
				if(this.IsReusable && this._Instance != null)
					return this._Instance;
				var instance = (IClipPropertyDataEditor)Activator.CreateInstance(this.Type);
				if(this.IsReusable)
					this._Instance = instance;
				return instance;
			}

			#endregion

		}

		#endregion

		private static readonly Dictionary<Type, TypeRec> _EditorTypes = new Dictionary<Type, TypeRec>();

		[CanBeNull]
		private static TypeRec LoadEditorType([CanBeNull] Type clipDataType)
		{
			if(clipDataType == null)
				return null;
			//Debug.Assert(typeof(ClipPropertyData).IsAssignableFrom(clipDataType));
			var attrs = (ClipPropertyDataEditorAttribute[])clipDataType.GetCustomAttributes(typeof(ClipPropertyDataEditorAttribute), false);
			if(attrs.Length == 0)
				return null;
			var editorType = attrs[0].GetEditorType();
			if(editorType == null)
				return null;
			Debug.Assert(typeof(IClipPropertyDataEditor).IsAssignableFrom(editorType));
			return new TypeRec(editorType, attrs[0].IsReusable);
		}

		[CanBeNull]
		private static TypeRec GetEditorTypeRec([CanBeNull] Type clipDataType)
		{
			if(clipDataType == null)
				return null;
			TypeRec editorTypeRec;
			if(!_EditorTypes.TryGetValue(clipDataType, out editorTypeRec))
			{
				editorTypeRec = LoadEditorType(clipDataType);
				if(editorTypeRec != null)
					_EditorTypes.Add(clipDataType, editorTypeRec);
			}
			return editorTypeRec;
		}

		[CanBeNull]
		internal static Type GetEditorType([CanBeNull] Type clipDataType)
		{
			bool isReusable;
			return GetEditorType(clipDataType, out isReusable);
		}

		[CanBeNull]
		internal static Type GetEditorType([CanBeNull] Type clipDataType, out bool isReusable)
		{
			var rec = GetEditorTypeRec(clipDataType);
			if(rec == null)
			{
				isReusable = false;
				return null;
			}
			isReusable = rec.IsReusable;
			return rec.Type;
		}

		[CanBeNull]
		internal static IClipPropertyDataEditor GetEditor([CanBeNull]Type clipDataType)
		{
			var rec = GetEditorTypeRec(clipDataType);
			return rec == null ? null : rec.GetOrCreateInstance();
		}

	}

	#endregion

}
