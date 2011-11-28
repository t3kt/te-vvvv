using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendReceiveNodes.Core
{

	#region PortEventHandler

	internal delegate void PortEventHandler(string key);

	#endregion

	#region PortEventArgs

	internal class PortEventArgs : EventArgs
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly string _Key;
		private readonly IPort _Port;

		#endregion

		#region Properties

		public string Key
		{
			get { return this._Key; }
		}

		public IPort Port
		{
			get { return this._Port; }
		}

		#endregion

		#region Constructors

		public PortEventArgs(string key)
		{
			this._Key = key;
		}

		public PortEventArgs(IPort port)
		{
			this._Port = port;
			if(port != null)
				this._Key = port.Key;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
