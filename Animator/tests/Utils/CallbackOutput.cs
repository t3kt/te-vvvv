using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;

namespace Animator.Tests.Utils
{

	#region CallbackOutput

	[Output(Key = "test.callback")]
	internal sealed class CallbackOutput : Output
	{

		#region Static / Constant

		#endregion

		#region Fields

#pragma warning disable 649
		public Func<OutputMessage, bool> PostMessageCallback;
#pragma warning restore 649

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override bool PostMessageInternal(OutputMessage message)
		{
			if(this.PostMessageCallback == null)
				return false;
			return this.PostMessageCallback(message);
		}

		#endregion

	}

	#endregion

}
