using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Nodes
{

	#region SValueCountNode

	public class SValueCountNode : IPlugin, IDisposable
	{

		#region Static / Constant

		public static IPluginInfo PluginInfo
		{
			get
			{
				return
					new PluginInfo
					{
						Name = "SCount",
						Category = "Value",
						Version = "Advanced",
						Help = "Gets number of senders for S (Value Advanced)",
						Bugs = String.Empty,
						Credits = String.Empty,
						Warnings = String.Empty,
						Author = "te",
						Namespace = typeof(SValueCountNode).Namespace,
						Class = typeof(SValueCountNode).Name
					};
			}
		}

		#endregion

		#region Fields

		private DoubleDataHolder _Data;
		private Dictionary<string, int> _InstanceCounts;

		private string _Key;
		private IStringIn _KeyInput;

		private IValueOut _CountOutput;

		private bool _Invalidate;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void DataUpdated(string key)
		{
			if(key == _Key)
				_Invalidate = true;
		}

		#endregion

		#region IPlugin Members

		bool IPlugin.AutoEvaluate
		{
			get { return false; }
		}

		void IPlugin.Configurate(IPluginConfig input)
		{
		}

		public void Evaluate(int spreadMax)
		{
			if(_KeyInput.PinIsChanged)
			{
				_KeyInput.GetString(0, out _Key);
				_Key = _Key ?? String.Empty;
				_Invalidate = true;
			}
			if(_Invalidate)
			{
				int count;
				if(!_InstanceCounts.TryGetValue(_Key, out count))
					count = 0;
				_CountOutput.SliceCount = 1;
				_CountOutput.SetValue(0, count);
				_Invalidate = false;
			}
		}

		public void SetPluginHost(IPluginHost host)
		{
			_Data = DoubleDataHolder.Instance;
			// ReSharper disable PossibleNullReferenceException
			_InstanceCounts = (Dictionary<string, int>)typeof(DataHolder<double>)
				.GetField("instancecount", BindingFlags.Instance | BindingFlags.NonPublic)
				.GetValue(this._Data);
			// ReSharper restore PossibleNullReferenceException
			_Data.OnAdd += this.DataUpdated;
			_Data.OnRemove += this.DataUpdated;
			host.CreateStringInput("Receive String", TSliceMode.Single, TPinVisibility.True, out _KeyInput);
			_KeyInput.SetSubType(String.Empty, false);
			host.CreateValueOutput("Count", 1, null, TSliceMode.Single, TPinVisibility.True, out _CountOutput);
			_CountOutput.SetSubType(0, Double.MaxValue, 1, 0, false, false, true);
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if(_Data != null)
			{
				_Data.OnAdd -= this.DataUpdated;
				_Data.OnRemove -= this.DataUpdated;
			}
			_Data = null;
			_InstanceCounts = null;
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
