using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Model
{

	#region IClipContainer

	public interface IClipContainer : IDocumentItem
	{

		IEnumerable<Clip> Clips { get; }

		Clip GetClip(Guid id);

		void AddClip(Clip clip);

		void RemoveClip(Guid id);

	}

	#endregion

	#region ITrackContainer

	public interface ITrackContainer : IDocumentItem
	{

		IEnumerable<Track> Tracks { get; }

		Track GetTrack(Guid id);

		void AddTrack(Track track);

		void RemoveTrack(Guid id);

	}

	#endregion

	#region IOutputContainer

	public interface IOutputContainer : IDocumentItem
	{

		IEnumerable<Output> Outputs { get; }

		Output GetOutput(Guid id);

		void AddOutput(Output output);

		void RemoveOutput(Guid id);

	}

	#endregion

}
