using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;

namespace Animator.Tests.Utils
{

	#region TargetObjectComparer

	internal sealed class TargetObjectComparer : DocumentItemComparer<TargetObject>
	{

		#region Static / Constant

		internal new static readonly TargetObjectComparer Default = new TargetObjectComparer();

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override bool EqualsInternal(TargetObject x, TargetObject y)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
