using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransportNodes.Core;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace TransportNodes.Nodes
{

	#region TransportNodeBase

	public abstract class TransportNodeBase : IDisposable
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly IPluginHost _Host;
		private readonly IDiffSpread<string> _TransportKeyConfig;

		private bool _Disposed;

		private Transport _Transport;

		#endregion

		#region Properties

		protected Transport Transport
		{
			get { return this._Transport; }
		}

		#endregion

		#region Constructors

		protected TransportNodeBase(IPluginHost host, IDiffSpread<string> transportKeyConfig)
		{
			this._Host = host;
			this._TransportKeyConfig = transportKeyConfig;
			transportKeyConfig.Changed += this.TransportKey_Changed;
		}

		#endregion

		#region Methods

		private void TransportKey_Changed(IDiffSpread<string> transportNames)
		{
			this.CheckDisposed();
			this.SetTransportKey(transportNames[0]);
		}

		private void AcquireTransport(string key)
		{
			this._Transport = TransportRegistry.Acquire(key);
			this.OnTransportAcquired(this._Transport);
		}

		private void ReleaseTransport()
		{
			if(TransportRegistry.Release(this._Transport))
			{
				var transport = this._Transport;
				this._Transport = null;
				this.OnTransportReleased(transport);
			}
		}

		protected void SetTransportKey(string key)
		{
			if(key == null)
			{
				this.ReleaseTransport();
			}
			else
			{
				if(this._Transport != null)
				{
					if(Transport.KeyComparer.Equals(this._Transport.Key, key))
						return;
					this.ReleaseTransport();
				}
				this.AcquireTransport(key);
			}
			this.OnTransportChanged();
		}

		protected void CheckDisposed()
		{
			if(_Disposed)
				throw new ObjectDisposedException(this.GetType().FullName);
		}

		protected virtual void OnTransportReleased(Transport transport) { }

		protected virtual void OnTransportAcquired(Transport transport) { }

		protected virtual void OnTransportChanged() { }

		#endregion

		#region IDisposable Members

		protected virtual void Dispose()
		{
			if(this._TransportKeyConfig != null)
				this._TransportKeyConfig.Changed -= this.TransportKey_Changed;
		}

		void IDisposable.Dispose()
		{
			if(!_Disposed)
			{
				Dispose();
				_Disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
