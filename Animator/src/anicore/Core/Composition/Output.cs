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
	public sealed class OutputAttribute : AniExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public OutputAttribute()
			: base(typeof(Output)) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
