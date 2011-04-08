using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Model
{

	#region ITrack

	public interface ITrack : IDocumentItem, IClipContainer
	{

		IOutput Output { get; set; }

	}

	#endregion

}
