using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region ClipState

	[Flags]
	public enum ClipState
	{
		Stopped		= 0x0000,
		Playing		= 0x0001,

		PlayQueued	= 0x0100,
		StopQueued	= 0x0200
	}

	#endregion

}
