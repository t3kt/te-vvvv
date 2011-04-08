using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Core.Transport
{

	#region ITransportContext

	public interface ITransportContext
	{

		TransportState State { get; }

		TransportTime Position { get; }

		int BarLength { get; }

		int BeatsPerMinute { get; }

		void Stop();

		void Play();

		void Pause();

		void GoTo(float bars);

	}

	#endregion

}
