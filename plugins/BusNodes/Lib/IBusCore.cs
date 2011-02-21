using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VVVV.Lib
{

	#region IBusCore<T>

	public interface IBusCore<T> : IDisposable
	{

		event BusPortEventHandler<T> PortOpened;
		event BusPortEventHandler<T> PortClosing;

		IBusPort<T> RequestPort(string key);

		void ReleasePort(string key);

	}

	#endregion

}
