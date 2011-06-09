using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Runtime;

namespace Animator.Core.Composition
{

	#region ClipPropertyDataEditorAttribute

	public sealed class ClipPropertyDataEditorAttribute : AniExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Type ClipDataType { get; set; }

		public bool Reusable { get; set; }

		#endregion

		#region Constructors

		public ClipPropertyDataEditorAttribute()
			: base(typeof(IClipPropertyDataEditor)) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IClipPropertyDataEditorMetadata

	public interface IClipPropertyDataEditorMetadata : IAniExportMetadata
	{
		Type ClipDataType { get; }
		bool Reusable { get; }
	}

	#endregion

}
