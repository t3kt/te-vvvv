using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OscSeq.Core
{

	#region ISeqDocument

	public interface ISeqDocument
	{

		string Host { get; set; }

		int Port { get; set; }

		IList<IChannel> Channels { get; set; }

		void Transmit(double position, Transmitter transmitter);

	}

	#endregion

}
