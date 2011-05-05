using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;

namespace Animator.Tests.Utils
{

	#region CollectorTransmitter

	[OutputTransmitter(Key = "test.collector")]
	internal class CollectorTransmitter : OutputTransmitter
	{

		#region Static/Constant

		#endregion

		#region Fields

		public readonly List<OutputMessage> Messages = new List<OutputMessage>();

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override bool PostMessageInternal(OutputMessage message)
		{
			if(message != null)
				this.Messages.Add(message);
			return true;
		}

		#endregion

	}

	#endregion

}
