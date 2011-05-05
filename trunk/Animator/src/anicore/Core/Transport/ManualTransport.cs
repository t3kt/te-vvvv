using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;

namespace Animator.Core.Transport
{

	#region ManualTransport

	[Transport(Key = "manual", Description = "Manual Transport")]
	public sealed class ManualTransport : Transport
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public void SendTick()
		{
			this.OnTick();
		}

		#endregion

	}

	#endregion

}
