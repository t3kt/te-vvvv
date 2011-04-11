using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region IPlayable

	public interface IPlayable
	{

		Time Duration { get; }

		int TriggerAlignment { get; }

		void Start(ITransport transport);

		void Stop(ITransport transport);

	}

	#endregion

}
