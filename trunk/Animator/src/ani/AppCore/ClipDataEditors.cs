using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Animator.Core.Model;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.AppCore
{

	#region ClipDataEditors

	internal static class ClipDataEditors
	{

		#region TypeRec

		private sealed class TypeRec
		{

			#region Static/Constant

			#endregion

			#region Fields

			public readonly Type Type;
			public readonly bool IsReusable;

			private IClipDataEditor _Instance;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public TypeRec(Type type, bool isReusable)
			{
				Debug.Assert(typeof(IClipDataEditor).IsAssignableFrom(type));
				this.Type = type;
				this.IsReusable = isReusable;
			}

			#endregion

			#region Methods

			public IClipDataEditor GetOrCreateInstance()
			{
				if(this.IsReusable && this._Instance != null)
					return this._Instance;
				var instance = (IClipDataEditor)Activator.CreateInstance(this.Type);
				if(this.IsReusable)
					this._Instance = instance;
				return instance;
			}

			#endregion

		}

		#endregion

		private static readonly Dictionary<Type, TypeRec> _EditorTypes = new Dictionary<Type, TypeRec>();

		[CanBeNull]
		private static TypeRec BuildClipDataEditorType([CanBeNull] Type clipType)
		{
			if(clipType == null)
				return null;
			Debug.Assert(typeof(Clip).IsAssignableFrom(clipType));
			var attrs = (ClipDataEditorAttribute[])clipType.GetCustomAttributes(typeof(ClipDataEditorAttribute), false);
			if(attrs.Length == 0)
				return null;
			var editorType = attrs[0].GetEditorType();
			if(editorType == null)
				return null;
			Debug.Assert(typeof(IClipDataEditor).IsAssignableFrom(editorType));
			return new TypeRec(editorType, attrs[0].IsReusable);
		}

		[CanBeNull]
		private static TypeRec GetEditorTypeRec([CanBeNull] Type clipType)
		{
			if(clipType == null)
				return null;
			TypeRec editorTypeRec;
			if(!_EditorTypes.TryGetValue(clipType, out editorTypeRec))
			{
				editorTypeRec = BuildClipDataEditorType(clipType);
				if(editorTypeRec != null)
					_EditorTypes.Add(clipType, editorTypeRec);
			}
			return editorTypeRec;
		}

		[CanBeNull]
		internal static Type GetEditorType([CanBeNull] Type clipType)
		{
			bool isReusable;
			return GetEditorType(clipType, out isReusable);
		}

		[CanBeNull]
		internal static Type GetEditorType([CanBeNull] Type clipType, out bool isReusable)
		{
			var rec = GetEditorTypeRec(clipType);
			if(rec == null)
			{
				isReusable = false;
				return null;
			}
			isReusable = rec.IsReusable;
			return rec.Type;
		}

		[CanBeNull]
		internal static IClipDataEditor GetEditor([CanBeNull] Type clipType)
		{
			var editorTypeRec = GetEditorTypeRec(clipType);
			return editorTypeRec == null ? null : editorTypeRec.GetOrCreateInstance();
		}

	}

	#endregion

}
