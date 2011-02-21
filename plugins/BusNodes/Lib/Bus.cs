using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VVVV.Lib
{

	#region Bus<T>

	public class Bus<T> : IBus<T>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Func<IBus<T>, string, IPort<T>> _PortCreator;
		private readonly Dictionary<string, IPort<T>> _Ports;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public Bus(Func<IBus<T>, string, IPort<T>> portCreator)
		{
			this._PortCreator = portCreator;
			this._Ports = new Dictionary<string, IPort<T>>(StringComparer.OrdinalIgnoreCase);
		}

		#endregion

		#region Methods

		private IPort<T> CreatePort(string key)
		{
			if(_PortCreator != null)
				return _PortCreator(this, key);
			return new Port<T>(this, key);
		}

		#endregion

		#region IBus<T> Members

		public bool ContainsPort(string key)
		{
			return _Ports.ContainsKey(key);
		}

		public IPort<T> RequestPort(string key)
		{
			IPort<T> port;
			if(!_Ports.TryGetValue(key, out port))
			{
				port = CreatePort(key);
				Debug.Assert(port != null);
				Debug.Assert(String.Equals(key, port.Key, StringComparison.OrdinalIgnoreCase));
				_Ports.Add(key, port);
			}
			return port;
		}

		public void ReleasePort(string key)
		{
			_Ports.Remove(key);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			foreach(var entry in _Ports)
			{
				if(entry.Value != null)
					entry.Value.Dispose();
				ReleasePort(entry.Key);
			}
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
