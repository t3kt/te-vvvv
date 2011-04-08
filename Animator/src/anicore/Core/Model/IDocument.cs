using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region IDocument

	public interface IDocument : IDocumentItem, IClipContainer, IOutputContainer, ITrackContainer
	{

		Time Duration { get; set; }

		float BeatsPerMinute { get; set; }

		int TriggerAlignment { get; set; }

	}

	#endregion

}
