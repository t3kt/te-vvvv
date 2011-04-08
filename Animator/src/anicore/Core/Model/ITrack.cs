using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Model
{

	#region ITrack

	public interface ITrack : IDocumentItem, IClipContainer
	{

		Guid? OutputId { get; set; }

		IOutput Output { get; set; }

	}

	#endregion

}
