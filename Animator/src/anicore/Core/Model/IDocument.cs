using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region IDocument

	public interface IDocument : IDocumentItem, IDocumentItemContainer
	{

		Time Duration { get; set; }

		float BeatsPerMinute { get; set; }

		int TriggerAlignment { get; set; }

		ICollection<ITrack> Tracks { get; set; }

		ITrack GetTrack(Guid id);

		ICollection<IOutput> Outputs { get; set; }

		IOutput GetOutput(Guid id);

	}

	#endregion

}
