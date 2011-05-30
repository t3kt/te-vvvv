using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;

namespace Animator.Core.Runtime
{

	#region IClipRefContainer

	internal interface IClipRefContainer
	{

		IEnumerable<ClipReference> Clips { get; }

		IEnumerable<ClipReference> GetActiveClips(Transport.Transport transport);

	}

	#endregion

}
