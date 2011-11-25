using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.Utils.OSC;

namespace OscSeq.Core
{

	#region OscUtil

	public static class OscUtil
	{

		public static void AppendAll(this OSCMessage message, params object[] values)
		{
			foreach(var value in values)
				message.Append(value);
		}

		public static void AppendAll(this OSCMessage message, IEnumerable<object> values)
		{
			foreach(var value in values)
				message.Append(value);
		}

		public static OSCBundle ToBundle(this IEnumerable<OSCPacket> packets, long timestamp = 0)
		{
			var bundle = new OSCBundle(timestamp);
			foreach(var packet in packets)
				bundle.Append(packet);
			return bundle;
		}

	}

	#endregion

}
