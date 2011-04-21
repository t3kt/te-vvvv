using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Runtime
{

	#region IRuntimeContext

	public interface IRuntimeContext
	{

		[NotNull]
		Document Document { get; }

		[CanBeNull]
		Clip GetClip(Guid id);

		[CanBeNull]
		Track GetTrack(Guid id);

		[CanBeNull]
		Output GetOutput(Guid id);

		[CanBeNull]
		IOutputTransmitter GetTransmitter(Guid id);

		[NotNull]
		ITransport Transport { get; }

		IEnumerable<ClipState> ActiveClips { get; }

		[CanBeNull]
		ClipState GetClipState(Guid id);

		void PostActiveClipOutputs();

	}

	#endregion

}
