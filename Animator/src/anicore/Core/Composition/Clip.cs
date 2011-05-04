using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Model;

namespace Animator.Core.Composition
{

	#region ClipAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ClipAttribute : KeyedExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public string ElementName { get; set; }

		#endregion

		#region Constructors

		public ClipAttribute()
			: base(typeof(Clip)) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IClipMetadata

	public interface IClipMetadata : IKeyedExportMetadata
	{
		string ElementName { get; }
	}

	#endregion

}
