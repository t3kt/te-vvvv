using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using SendReceiveNodes.Core;
using VVVV.PluginInterfaces.V2;

namespace SendReceiveNodes.Nodes
{

	#region SendNode<T>

	public abstract class SendNode<T> : IPartImportsSatisfiedNotification, IDisposable
	{

		#region Static/Constant

		private static readonly BusCore<T> _Core = BusCore<T>.Instance;

		#endregion

		#region Fields

		[Input("Input")]
		private IDiffSpread<T> _ValuesInput;

		[Input("Key")]
		private IDiffSpread<string> _KeysInput;

		private readonly Dictionary<string, Port<T>> _Ports;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		protected SendNode()
		{
			this._Ports = new Dictionary<string, Port<T>>();
		}

		#endregion

		#region Methods

		private void ReleasePort(string key )
		{
			if(this._Ports.Remove(key))
			{
				_Core.ReleasePort(key);
			}
		}

		private void RequestPort(string key)
		{
			if(!this._Ports.ContainsKey(key))
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			throw new NotImplementedException();
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
