using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Runtime;

namespace Animator.Core.Composition
{

	#region PropertyEditorAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class PropertyEditorAttribute : AniExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Type TargetType { get; set; }

		#endregion

		#region Constructors

		public PropertyEditorAttribute()
			: base(typeof(IPropertyEditor)) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IPropertyEditorMetadata

	public interface IPropertyEditorMetadata : IAniExportMetadata
	{

		Type TargetType { get; }

	}

	#endregion


}
