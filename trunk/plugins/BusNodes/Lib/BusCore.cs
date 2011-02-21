using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VVVV.Lib
{

	#region BusCore<T>

	public class BusCore<T> : IBusCore<T>
	{

		#region Static / Constant

		private static StringComparer KeyComparer
		{
			get { return StringComparer.OrdinalIgnoreCase; }
		}

		#endregion

		#region Fields

		private readonly Dictionary<string, IBusPort<T>> _Ports = new Dictionary<string, IBusPort<T>>(KeyComparer);
		private readonly Func<string, IBusPort<T>> _PortCreator;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public BusCore(Func<string, IBusPort<T>> portCreator)
		{
			this._PortCreator = portCreator;
		}

		#endregion

		#region Methods

		private IBusPort<T> OpenPort(string key)
		{
			IBusPort<T> port;
			if(_PortCreator != null)
			{
				port = _PortCreator(key);
				Debug.Assert(port != null);
			}
			port = new BusPort<T>(key);
			var handler = this.PortOpened;
			if(handler != null)
				handler(port);
			return port;
		}

		private void ClosePort(IBusPort<T> port)
		{
			Debug.Assert(port != null);
			Debug.Assert(_Ports.ContainsKey(port.Key));
			var handler = this.PortClosing;
			if(handler != null)
				handler(port);
			_Ports.Remove(port.Key);
			port.Dispose();
		}

		#endregion

		#region IBusCore<T> Members

		public event BusPortEventHandler<T> PortOpened;

		public event BusPortEventHandler<T> PortClosing;

		public IBusPort<T> RequestPort(string key)
		{
			IBusPort<T> port;
			if(!_Ports.TryGetValue(key, out port))
			{
				port = this.OpenPort(key);
				_Ports.Add(key, port);
			}
			return port;
		}

		public void ReleasePort(string key)
		{
			IBusPort<T> port;
			if(_Ports.TryGetValue(key, out port))
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
