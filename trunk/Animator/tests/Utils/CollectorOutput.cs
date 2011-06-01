using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;

namespace Animator.Tests.Utils
{

	#region CollectorOutput

	[Output(Key = "test.collector")]
	internal sealed class CollectorOutput : Output
	{

		#region Static / Constant

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
