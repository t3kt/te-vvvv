using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;

namespace Animator.Core.IO
{

	#region IOutputTransmitter

	public interface IOutputTransmitter : IGuidId, IDisposable
	{

		void Initialize(Output output);
		void PostMessage(OutputMessage message);

		event EventHandler<OutputMessageEventArgs> MessageDropped;

	}

	#endregion

}