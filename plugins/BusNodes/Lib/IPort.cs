using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region IPort<T>

	public interface IPort<T> : IDisposable
	{

		string Key { get; }
		IDiffSpread<T> Values { get; }

		bool IsEmpty { get; }

		int SenderCount { get; }
		int ClientCount { get; }

		void AddSender(ISender<T> sender);
		void RemoveSender(ISender<T> sender);

		void AddClient(IClient<T> client);
		void RemoveClient(IClient<T> client);

	}

	#endregion

}
