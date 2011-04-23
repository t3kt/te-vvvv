using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;

namespace Animator.Core.Runtime
{

	#region ClipDataEditorAttribute

	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ClipDataEditorAttribute : Attribute
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly Type _EditorType;
		private readonly string _EditorTypeName;

		#endregion

		#region Properties

		public Type EditorType
		{
			get { return this._EditorType; }
		}

		public string EditorTypeName
		{
			get { return this._EditorTypeName; }
		}

		#endregion

		#region Constructors

		public ClipDataEditorAttribute(Type editorType)
		{
			this._EditorType = editorType;
		}

		public ClipDataEditorAttribute(string editorTypeName)
		{
			this._EditorTypeName = editorTypeName;
		}

		#endregion

		#region Methods

		public Type GetEditorType()
		{
			if(this._EditorType != null)
				return this._EditorType;
			return Type.GetType(this._EditorTypeName, false, false);
		}

		#endregion

	}

	#endregion

	#region IClipDataEditor

	public interface IClipDataEditor
	{

		Clip Clip { get; set; }

	}

	#endregion

}
