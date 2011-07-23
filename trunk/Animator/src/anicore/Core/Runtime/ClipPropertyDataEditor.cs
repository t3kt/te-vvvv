using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model.Clips;

namespace Animator.Core.Runtime
{

	#region ClipPropertyDataEditorAttribute

	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ClipPropertyDataEditorAttribute : Attribute
	{

		#region Static/Constant

		#endregion

		#region Fields

		private bool _IsReusable;
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

		public bool IsReusable
		{
			get { return this._IsReusable; }
			set { this._IsReusable = value; }
		}

		#endregion

		#region Constructors

		public ClipPropertyDataEditorAttribute(Type editorType)
		{
			this._EditorType = editorType;
		}

		public ClipPropertyDataEditorAttribute(string editorTypeName)
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

	#region IClipPropertyDataEditor

	public interface IClipPropertyDataEditor
	{

		ClipPropertyData ClipPropertyData { get; set; }

	}

	#endregion

}
