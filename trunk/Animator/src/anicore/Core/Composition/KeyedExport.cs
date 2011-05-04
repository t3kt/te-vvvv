using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Animator.Core.Composition
{

	#region KeyedExportAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class KeyedExportAttribute : ExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public string Key { get; set; }

		#endregion

		#region Constructors

		internal KeyedExportAttribute(Type contractType)
			: base(contractType) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IKeyedExportMetadata

	public interface IKeyedExportMetadata
	{
		string Key { get; }
	}

	#endregion

}
