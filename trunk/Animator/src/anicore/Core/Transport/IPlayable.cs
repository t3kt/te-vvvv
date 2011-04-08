using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region IPlayable

	public interface IPlayable
	{

		Time Duration { get; set; }

		int TriggerAlignment { get; set; }

		void Start(ITransport transport);

		void Stop(ITransport transport);

		void EnqueueStart(ITransport transport);

		void EnqueueStop(ITransport transport);

	}

	#endregion

}
