using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region ITransport

	/// <summary>
	/// Handles position/state tracking, and fires periodic Tick events
	/// (to trigger updates/outputs, etc).
	/// </summary>
	public interface ITransport
	{

		void SetParameters(IDictionary<string, string> parameters);

		Time Position { get; set; }

		TransportState State { get; }

		void Play();

		void Stop();

		void Pause();

		event EventHandler PositionChanged;

		event EventHandler StateChanged;

		event EventHandler Tick;

	}

	#endregion

	#region IInternalTransport

	internal interface IInternalTransport : ITransport
	{
		float BeatsPerMinute { get; set; }
	}

	#endregion

}
