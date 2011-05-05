using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;

namespace Animator.Tests.Utils
{

	#region CallbackTransmitter

	[OutputTransmitter(Key = "test.callback")]
	internal sealed class CallbackTransmitter : OutputTransmitter
	{

#pragma warning disable 649
		public Func<OutputMessage, bool> PostMessageCallback;
#pragma warning restore 649

		protected override bool PostMessageInternal(OutputMessage message)
		{
			if(this.PostMessageCallback == null)
				return false;
			return this.PostMessageCallback(message);
		}

	}

	#endregion

}
