using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

namespace TransportNodes.Core
{

	#region TransportController

	internal class TransportController : IDisposable
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly IPluginHost _Host;
		private readonly Transport _Transport;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public TransportController(IPluginHost host, Transport transport)
		{
			this._Host = host;
			this._Transport = transport;
		}

		#endregion

		#region Methods

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
