using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.IO;

namespace Animator.Core.Composition
{

	#region OutputTransmitterAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class OutputTransmitterAttribute : KeyedExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public OutputTransmitterAttribute()
			: base(typeof(IOutputTransmitter)) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IOutputTransmitterMetadata

	public interface IOutputTransmitterMetadata : IKeyedExportMetadata
	{
	}

	#endregion

}
