using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.Utils.OSC;

namespace OscSeq.Core
{

	#region IChannel

	public interface IChannel
	{
		string Address { get; }
		object Value { get; }
		OSCPacket BuildPacket();
	}

	#endregion

}
