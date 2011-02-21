using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region BusPort<T>

	public class BusPort<T> : IBusPort<T>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly string _Key;
		private readonly BusComponentCollection<IBusClient<T>> _Clients = new BusComponentCollection<IBusClient<T>>();
		private readonly BusComponentCollection<IBusSender<T>> _Senders = new BusComponentCollection<IBusSender<T>>();
		private IBusSender<T> _CurrentSender;
		private readonly DiffSpreadHolder<T> _ValuesHolder = new DiffSpreadHolder<T>();
		private readonly DiffSpreadHolder<T> _DefaultValuesHolder = new DiffSpreadHolder<T>();

		#endregion

		#region Properties

		public string Key
		{
			get { return _Key; }
		}

		public IDiffSpread<T> Values
		{
			get { return _ValuesHolder; }
		}

		public IDiffSpread<T> DefaultValues
		{
			get { return _DefaultValuesHolder; }
		}

		public int SenderCount
		{
			get { return _Senders.Count; }
		}

		public int ClientCount
		{
			get { return _Clients.Count; }
		}

		public IBusSender<T> CurrentSender
		{
			get { return this._CurrentSender; }
		}

		#endregion

		#region Events

		public event BusSenderEventHandler<T> ValueUpdated;

		public event BusSenderEventHandler<T> SenderAdded;

		public event BusSenderEventHandler<T> SenderRemoved;

		public event BusClientEventHandler<T> ClientAdded;

		public event BusClientEventHandler<T> ClientRemoved;

		public event BusPortEventHandler<T> Closing;

		#endregion

		#region Constructors

		public BusPort(string key)
		{
			if(String.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");
			this._Key = key;
		}

		#endregion

		#region Methods

		private void OnValueUpdated(IBusSender<T> sender)
		{
			var handler = this.ValueUpdated;
			if(handler != null)
				handler(this, sender);
		}

		private void OnSenderAdded(IBusSender<T> sender)
		{
			var handler = this.SenderAdded;
			if(handler != null)
				handler(this, sender);
			OnComponentAdded(sender);
			if(sender != null)
			{
				if(_Senders.Count == 0)
					ChangeSender(sender);
				sender.Changed += this.Sender_Changed;
			}
		}

		private void OnSenderRemoved(IBusSender<T> sender)
		{
			var handler = this.SenderRemoved;
			if(handler != null)
				handler(this, sender);
			OnComponentRemoved(sender);
			if(sender != null)
			{
				if(sender == _CurrentSender)
				{
					ChangeSender(_Senders.LastOrDefault());
				}
				sender.Changed -= this.Sender_Changed;
			}
		}

		private void OnClientAdded(IBusClient<T> client)
		{
			var handler = this.ClientAdded;
			if(handler != null)
				handler(this, client);
			OnComponentAdded(client);
		}

		private void OnClientRemoved(IBusClient<T> client)
		{
			var handler = this.ClientRemoved;
			if(handler != null)
				handler(this, client);
			OnComponentRemoved(client);
		}

		private void OnComponentAdded(IBusComponent<T> component)
		{
			if(component != null)
			{
				component.AttachToPort(this);
				throw new NotImplementedException();
			}
		}

		private void OnComponentRemoved(IBusComponent<T> component)
		{
			if(component != null)
			{
				component.DetachFromPort(this);
				if(_Clients.Count == 0 && _Senders.Count != 0)
				{
					OnEmpty();
				}
			}
		}

		private void ChangeSender(IBusSender<T> sender)
		{
			Debug.Assert(_Senders.Contains(sender));
			if(sender != _CurrentSender)
			{
				_ValuesHolder.SetSpread(sender == null ? null : sender.Values);
				_DefaultValuesHolder.SetSpread(sender == null ? null : sender.DefaultValues);
				_CurrentSender = sender;
			}
		}

		private void Sender_Changed(IBusPort<T> port, IBusSender<T> sender)
		{
			Debug.Assert(port == this);
			ChangeSender(sender);
		}

		private void OnEmpty()
		{
			Close();
		}

		private void OnClosing()
		{
			var handler = this.Closing;
			if(handler != null)
				handler(this);
		}

		public void AddSender(IBusSender<T> sender)
		{
			if(sender != null && _Senders.Add(sender))
			{
				OnSenderAdded(sender);
			}
		}

		public void RemoveSender(IBusSender<T> sender)
		{
			if(sender != null && _Senders.Remove(sender))
			{
				OnSenderRemoved(sender);
			}
		}

		public void AddClient(IBusClient<T> client)
		{
			if(client != null && _Clients.Add(client))
			{
				OnClientAdded(client);
			}
		}

		public void RemoveClient(IBusClient<T> client)
		{
			if(client != null && _Clients.Remove(client.Id))
			{
				OnClientRemoved(client);
			}
		}

		internal void Close()
		{
			OnClosing();
			_Clients.ApplyAndFlush(this.OnClientRemoved);
			_Senders.ApplyAndFlush(this.OnSenderRemoved);
			this.ValueUpdated = null;
			this.SenderAdded = null;
			this.SenderRemoved = null;
			this.ClientAdded = null;
			this.ClientRemoved = null;
			this.Closing = null;
		}

		public void Dispose()
		{
			Close();
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
