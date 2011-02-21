using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region Port<T>

	public class Port<T> : IPort<T>
	{

		#region Nested Types

		#region ComponentCollection

		private sealed class ComponentCollection<TComponent> : KeyedCollection<Guid, TComponent>
			where TComponent : class, IGuidId
		{

			#region Static / Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			protected override Guid GetKeyForItem(TComponent item)
			{
				Debug.Assert(item != null);
				return item.Id;
			}

			public new bool Add(TComponent item)
			{
				if(item == null || !Contains(item.Id))
					return false;
				Add(item);
				return true;
			}

			public bool TryPop(out TComponent item)
			{
				if(this.Count == 0)
				{
					item = null;
					return false;
				}
				item = this[0];
				RemoveAt(0);
				return true;
			}

			#endregion

		}

		#endregion

		#endregion

		#region Static / Constant

		#endregion

		#region Fields

		private readonly IBus<T> _Bus;
		private readonly string _Key;
		private readonly DiffSpreadHolder<T> _ValuesHolder;
		private readonly ComponentCollection<ISender<T>> _Senders;
		private readonly ComponentCollection<IClient<T>> _Clients;
		private Guid? _LastSenderId;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public Port(IBus<T> bus, string key)
		{
			if(String.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");
			_Bus = bus;
			_Key = key;
			_ValuesHolder = new DiffSpreadHolder<T>();
			_Senders = new ComponentCollection<ISender<T>>();
			_Clients = new ComponentCollection<IClient<T>>();
		}

		#endregion

		#region Methods

		private void SetCurrentSender(ISender<T> sender)
		{
			if(sender == null)
			{
				if(_LastSenderId != null)
				{
					_LastSenderId = null;
					_ValuesHolder.SetSpread(null);
					_ValuesHolder.NotifySpreadChanged();
				}
			}
			else
			{
				if(_LastSenderId != sender.Id)
				{
					_LastSenderId = sender.Id;
					_ValuesHolder.SetSpread(sender.Values);
					_ValuesHolder.NotifySpreadChanged();
				}
			}
		}

		private void CloseIfEmpty()
		{
			if(this.IsEmpty)
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region IPort<T> Members

		public string Key
		{
			get { return _Key; }
		}

		public IDiffSpread<T> Values
		{
			get { return _ValuesHolder; }
		}

		public bool IsEmpty
		{
			get { return this.SenderCount == 0 && this.ClientCount == 0; }
		}

		public int SenderCount
		{
			get { return _Senders.Count; }
		}

		public int ClientCount
		{
			get { return _Clients.Count; }
		}

		public void AddSender(ISender<T> sender)
		{
			if(sender == null)
				throw new ArgumentNullException("sender");
			Debug.Assert(!_Senders.Contains(sender.Id));
			_Senders.Add(sender);
			if(_Senders.Count == 1)
				SetCurrentSender(sender);
		}

		public void RemoveSender(ISender<T> sender)
		{
			if(sender == null)
				throw new ArgumentNullException("sender");
			Debug.Assert(_Senders.Contains(sender.Id));
			_Senders.Remove(sender);
			if(_LastSenderId == sender.Id)
				SetCurrentSender(_Senders.LastOrDefault());
			CloseIfEmpty();
		}

		public void AddClient(IClient<T> client)
		{
			if(client == null)
				throw new ArgumentNullException("client");
			Debug.Assert(!_Clients.Contains(client.Id));
			_Clients.Add(client);
		}

		public void RemoveClient(IClient<T> client)
		{
			if(client == null)
				throw new ArgumentNullException("client");
			Debug.Assert(_Clients.Contains(client.Id));
			_Clients.Remove(client);
			CloseIfEmpty();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion

	}

	#endregion

}
