using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using SendReceiveNodes.Core;
using VVVV.PluginInterfaces.V2;

namespace SendReceiveNodes.Nodes
{

	#region SingleSendNode<T>

	public abstract class SingleSendNode<T> : IPluginEvaluate, IDisposable
	{

		#region Static/Constant

		#endregion

		#region Fields

		[Input("Input")]
		private IDiffSpread<T> _ValuesInput;

		[Input("Key", IsSingle = true)]
		private IDiffSpread<string> _KeyInput;

		private Port<T> _Port;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void SetKey(string key)
		{
			if(String.IsNullOrEmpty(key))
			{
				if(_Port != null)
				{
					BusCore<T>.Instance.ReleasePort(_Port.Key);
					_Port = null;
				}
			}
			else
			{
				if(_Port != null)
				{
					if(StringComparer.OrdinalIgnoreCase.Equals(key, _Port.Key))
						return;
					BusCore<T>.Instance.ReleasePort(_Port.Key);
				}
				_Port = BusCore<T>.Instance.RequestPort(key);
			}
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(this._KeyInput.IsChanged || this._ValuesInput.IsChanged)
			{
				if(this._KeyInput.IsChanged)
				{
					this.SetKey(this._KeyInput[0]);
				}
				if(this._Port != null)
				{
					var data = this._ValuesInput.ToArray();
					this._Port.SetData(data);
				}
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.SetKey(null);
		}

		#endregion

	}

	#endregion

	#region Derived SingleSendNode Classes

	//public sealed class SingleSendValueNode:SingleSendNode<double>
	//{
	//}

	#endregion

}
