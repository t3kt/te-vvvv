using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Animator.Core.Runtime
{

	#region IGuidId

	public interface IGuidId
	{

		[Browsable(false)]
		Guid Id { get; }

	}

	#endregion

}
