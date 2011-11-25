using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using VVVV.Utils.OSC;

namespace OscSeq.Core
{

	#region Transmitter

	public sealed class Transmitter : IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private UdpClient _Client;
		private string _Host;
		private int _Port;

		#endregion

		#region Properties

		public string Host
		{
			get { return this._Host; }
			set
			{
				if(value != _Host)
				{
					_Host = value;
					if(_Client != null)
						this.Open();
				}
			}
		}

		public int Port
		{
			get { return this._Port; }
			set
			{
				if(value != _Port)
				{
					_Port = value;
					if(_Client != null)
						this.Open();
				}
			}
		}

		public bool IsConnected
		{
			get { return _Client != null; }
		}

		#endregion

		#region Events

		public event EventHandler StatusChanged;

		private void OnStatusChanged()
		{
			var handler = this.StatusChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Constructors

		public Transmitter(string host, int port)
		{
			this._Host = host;
			this._Port = port;
		}

		public Transmitter()
		{
		}

		#endregion

		#region Methods

		public void Open(string host, int port)
		{
			_Host = host;
			_Port = port;
			this.Open();
		}

		public void Open()
		{
			this.Close();
			_Client = new UdpClient(_Host, _Port);
			this.OnStatusChanged();
		}

		public void Close()
		{
			if(_Client != null)
			{
				_Client.Close();
				_Client = null;
				this.OnStatusChanged();
			}
		}

		public void Send(OSCPacket packet)
		{
			if(_Client == null)
				throw new InvalidOperationException();
			var data = packet.BinaryData;
			_Client.Send(data, data.Length);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Close();
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
