using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendReceiveNodes.Core
{

	#region IBusCore

	internal interface IBusCore
	{

		event EventHandler<PortEventArgs> PortAdded;
		event EventHandler<PortEventArgs> PortRemoved;

		IPort RequestPort(string key);
		void ReleasePort(string key);

		IEnumerable<string> PortKeys { get; }

	}

	#endregion

	#region BusCore<T>

	internal class BusCore<T> : IBusCore
	{

		#region Static/Constant

		internal static readonly BusCore<T> Instance = new BusCore<T>();

		#endregion

		#region Fields

		private readonly Dictionary<string, Port<T>> _Ports;

		#endregion

		#region Properties

		public IEnumerable<string> PortKeys
		{
			get { return this._Ports.Keys; }
		}

		#endregion

		#region Events

		public event EventHandler<PortEventArgs> PortAdded;

		public event EventHandler<PortEventArgs> PortRemoved;

		#endregion

		#region Constructors

		public BusCore()
		{
			this._Ports = new Dictionary<string, Port<T>>(StringComparer.OrdinalIgnoreCase);
		}

		#endregion

		#region Methods

		public Port<T> RequestPort(string key)
		{
			Port<T> port;
			if(!this._Ports.TryGetValue(key, out port))
			{
				port = new Port<T>(key);
				this._Ports.Add(key, port);
				var handler = this.PortAdded;
				if(handler != null)
					handler(this, new PortEventArgs(port));
			}
			return port;
		}

		public void ReleasePort(string key)
		{
			Port<T> port;
			if(this._Ports.TryGetValue(key, out port) && port.RemoveReference())
			{
				this._Ports.Remove(key);
				var handler = this.PortRemoved;
				if(handler != null)
					handler(this, new PortEventArgs(port));
			}
		}

		#endregion

		#region IBusCore Members

		IPort IBusCore.RequestPort(string key)
		{
			return this.RequestPort(key);
		}

		#endregion

	}

	#endregion

}
