using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Model
{

	#region ITrack

	public interface ITrack : IDocumentItem, IDocumentItemContainer
	{

		ICollection<IClip> Clips { get; set; }

		IClip GetClip(Guid id);

		IOutput Output { get; set; }

	}

	#endregion

}
