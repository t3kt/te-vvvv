using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransportNodes.Core
{

	#region TransportEventArgs

	internal class TransportEventArgs : EventArgs
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly string _TransportKey;
		private readonly Transport _Transport;

		#endregion

		#region Properties

		public string TransportKey
		{
			get { return this._TransportKey; }
		}

		public Transport Transport
		{
			get { return this._Transport; }
		}

		#endregion

		#region Constructors

		public TransportEventArgs(string transportKey)
		{
			this._TransportKey = transportKey;
		}

		public TransportEventArgs(Transport transport)
		{
			this._Transport = transport;
			if(transport != null)
				this._TransportKey = transport.Key;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region CountChangedEventArgs<TKey>

	internal sealed class CountChangedEventArgs<TKey> : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly TKey _Key;
		private readonly int _Count;

		#endregion

		#region Properties

		public TKey Key
		{
			get { return _Key; }
		}

		public int Count
		{
			get { return _Count; }
		}

		#endregion

		#region Constructors

		public CountChangedEventArgs(TKey key, int count)
		{
			_Key = key;
			_Count = count;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
