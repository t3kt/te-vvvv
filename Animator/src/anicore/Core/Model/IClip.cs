using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region IClip

	public interface IClip : IDocumentItem
	{

		Time Duration { get; set; }

		int TriggerAlignment { get; set; }

		//ClipState State { get; }

		//object GetValue(ITransport transport);

	}

	#endregion

}
