using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region ITransportController

	public interface ITransportController : IDisposable
	{

		TimeSpan Position { get; set; }

		TransportState State { get; }

		event EventHandler<TransportTickEventArgs> Tick;
		event EventHandler StateChanged;
		event EventHandler PositionChanged;

		void Play();
		void Stop();
		void Pause();

	}

	#endregion

}
