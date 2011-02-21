using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VVVV.Lib
{

	#region BusPortEventHandler<T>

	public delegate void BusPortEventHandler<T>(IBusPort<T> port);

	#endregion

	#region BusClientEventHandler<T>

	public delegate void BusClientEventHandler<T>(IBusPort<T> port, IBusClient<T> client);

	#endregion

	#region BusSenderEventHandler<T>

	public delegate void BusSenderEventHandler<T>(IBusPort<T> port, IBusSender<T> sender);

	#endregion

}
