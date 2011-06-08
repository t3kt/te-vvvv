using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Model.Clips;

namespace Animator.Core.Composition
{

	#region ClipPropertyDataAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ClipPropertyDataAttribute : AniExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ClipPropertyDataAttribute()
			: base(typeof(ClipPropertyData)) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
