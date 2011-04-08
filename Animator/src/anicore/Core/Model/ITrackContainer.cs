using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Core.Model
{

	#region ITrackContainer

	public interface ITrackContainer : IDocumentItem
	{

		IEnumerable<ITrack> Tracks { get; }

		ITrack GetTrack(Guid id);

		void AddTrack(ITrack track);

		void RemoveTrack(Guid id);

	}

	#endregion

}
