using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region ITransport

	public interface ITransport
	{

		float BeatsPerMinute { get; set; }

		bool IsPlaying { get; }

		Time Position { get; set; }

		void Play();

		void Stop();

		void Pause();

		void EnqueueAction(Action action, int triggerAlignment);

	}

	#endregion

}
