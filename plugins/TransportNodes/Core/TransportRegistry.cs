using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TESharedAnnotations;

namespace TransportNodes.Core
{

	#region TransportRegistry

	internal static class TransportRegistry
	{

		#region RegCore

		private sealed class RegCore
		{

			#region Fields

			private readonly Dictionary<Guid, Transport> _ById = new Dictionary<Guid, Transport>();
			private readonly Dictionary<string, Transport> _ByKey = new Dictionary<string, Transport>(Transport.KeyComparer);

			#endregion

			#region Properties

			public IEnumerable<Transport> Transports
			{
				get { return this._ByKey.Values; }
			}

			#endregion

			#region Events

			public event EventHandler<TransportEventArgs> Registered;

			public event EventHandler<TransportEventArgs> Unregistered;

			public event EventHandler<CountChangedEventArgs<Guid>> UsageCountChanged;

			#endregion

			#region Constructors

			#endregion

			#region Methods

			private void OnRegistered(Transport transport)
			{
				var handler = this.Registered;
				if(handler != null)
					handler(this, new TransportEventArgs(transport));
			}

			private void OnUnregistered(Transport transport)
			{
				var handler = this.Unregistered;
				if(handler != null)
					handler(this, new TransportEventArgs(transport));
			}

			private void OnUsageCountChanged(Guid id, int count)
			{
				var handler = this.UsageCountChanged;
				if(handler != null)
					handler(this, new CountChangedEventArgs<Guid>(id, count));
			}

			private void Add(Transport transport)
			{
				this._ById.Add(transport.Id, transport);
				this._ByKey.Add(transport.Key, transport);
				this.OnRegistered(transport);
			}

			private void Remove(Transport transport)
			{
				this._ById.Remove(transport.Id);
				this._ByKey.Remove(transport.Key);
				this.OnUnregistered(transport);
			}

			public Transport Find(Guid id)
			{
				Transport transport;
				return _ById.TryGetValue(id, out transport) ? transport : null;
			}

			public Transport Find(string key)
			{
				Transport transport;
				return _ByKey.TryGetValue(key, out transport) ? transport : null;
			}

			public int GetUsageCount(Guid id)
			{
				var transport = Find(id);
				return transport == null ? 0 : transport.RefCount;
			}

			public Transport Acquire(string key)
			{
				if(String.IsNullOrEmpty(key))
					return null;
				Transport transport;
				if(!this._ByKey.TryGetValue(key, out transport))
				{
					transport = new Transport(key);
					this.Add(transport);
				}
				transport.IncrementRefCount();
				this.OnUsageCountChanged(transport.Id, transport.RefCount);
				return transport;
			}

			public bool Release(Guid id)
			{
				Transport transport;
				if(!this._ById.TryGetValue(id, out transport))
					return false;
				if(transport.DecrementRefCount())
				{
					this.Remove(transport);
					return true;
				}
				this.OnUsageCountChanged(transport.Id, transport.RefCount);
				return false;
			}

			#endregion

		}

		#endregion

		private static readonly RegCore _Core = new RegCore();
		private static int _LastId;

		#region Properties

		internal static IEnumerable<Transport> RegisteredTransports
		{
			get { return _Core.Transports; }
		}

		#endregion

		#region Events

		internal static event EventHandler<TransportEventArgs> Registered
		{
			add { _Core.Registered += value; }
			remove { _Core.Registered -= value; }
		}

		internal static event EventHandler<TransportEventArgs> Unregistered
		{
			add { _Core.Unregistered += value; }
			remove { _Core.Unregistered -= value; }
		}

		internal static event EventHandler<CountChangedEventArgs<Guid>> UsageCountChanged
		{
			add { _Core.UsageCountChanged += value; }
			remove { _Core.UsageCountChanged -= value; }
		}

		#endregion

		#region Methods

		internal static int GetUsageCount(Guid id)
		{
			return _Core.GetUsageCount(id);
		}

		internal static int GetUsageCount(Transport transport)
		{
			return transport == null ? 0 : GetUsageCount(transport.Id);
		}

		private static string NextKey()
		{
			return "Trans" + (++_LastId);
		}

		internal static Transport Acquire(string key)
		{
			//if(String.IsNullOrEmpty(key))
			//    key = NextKey();
			return _Core.Acquire(key);
		}

		internal static Transport Find(Guid id)
		{
			return _Core.Find(id);
		}

		internal static bool Release(Guid id)
		{
			return _Core.Release(id);
		}

		internal static bool Release(Transport transport)
		{
			return transport != null && Release(transport.Id);
		}

		#endregion

	}

	#endregion

}
