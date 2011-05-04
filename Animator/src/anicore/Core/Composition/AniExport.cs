using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Animator.Core.Composition
{

	#region AniExportAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class AniExportAttribute : ExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public string Key { get; set; }

		public string Description { get; set; }

		#endregion

		#region Constructors

		public AniExportAttribute(Type contractType)
			: base(contractType) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IAniExportMetadata

	public interface IAniExportMetadata
	{
		string Key { get; set; }
		string Description { get; set; }
	}

	#endregion

}
