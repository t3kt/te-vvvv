using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region IBusPort<T>

	public interface IBusPort<T> : IDisposable
	{
		string Key { get; }
		IDiffSpread<T> DefaultValues { get; }
		IDiffSpread<T> Values { get; }

		int ClientCount { get; }
		int SenderCount { get; }

		IBusSender<T> CurrentSender { get; }

		event BusSenderEventHandler<T> SenderAdded;
		event BusSenderEventHandler<T> SenderRemoved;

		event BusClientEventHandler<T> ClientAdded;
		event BusClientEventHandler<T> ClientRemoved;

		event BusSenderEventHandler<T> ValueUpdated;

		event BusPortEventHandler<T> Closing;

		void AddSender(IBusSender<T> sender);
		void RemoveSender(IBusSender<T> sender);

		void AddClient(IBusClient<T> client);
		void RemoveClient(IBusClient<T> client);

	}

	#endregion

}
