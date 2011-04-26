using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region ITransport

	public interface ITransport
	{

		bool IsPlaying { get; }

		Time Position { get; }

		void Play();

		void Stop();

		void Pause();

	}

	#endregion

}