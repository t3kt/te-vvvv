using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Runtime;

namespace Animator.Core.Composition
{

	#region ClipDataEditor

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ClipDataEditorTypeAttribute : AniExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Type ClipType { get; set; }

		public bool Reusable { get; set; }

		#endregion

		#region Constructors

		public ClipDataEditorTypeAttribute()
			: base(typeof(IClipDataEditor)) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IClipDataEditorMetadata

	public interface IClipDataEditorMetadata : IAniExportMetadata
	{
		Type ClipType { get; }
		bool Reusable { get; }
	}

	#endregion

}
