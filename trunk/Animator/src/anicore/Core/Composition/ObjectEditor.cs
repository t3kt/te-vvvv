using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Runtime;

namespace Animator.Core.Composition
{

	#region ObjectEditorAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ObjectEditorAttribute : AniExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Type TargetType { get; set; }

		#endregion

		#region Constructors

		public ObjectEditorAttribute()
			: base(typeof(IObjectEditor)) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IObjectEditorMetadata

	public interface IObjectEditorMetadata : IAniExportMetadata
	{

		Type TargetType { get; }

	}

	#endregion


}
