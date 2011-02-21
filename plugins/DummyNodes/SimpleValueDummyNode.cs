using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Nodes
{

	#region SimpleValueDummyNode

	public class SimpleValueDummyNode : IPlugin
	{

		#region Static / Constant

		private const double MaxCount = 500;

		private static IPluginInfo _PluginInfo;

		public static IPluginInfo PluginInfo
		{
			get
			{
				if(_PluginInfo == null)
				{
					_PluginInfo = new PluginInfo
								  {
									  Name = "Dummy",
									  Category = "Value",
									  Version = "Simple",
									  Author = "te",
									  Namespace = typeof(SimpleValueDummyNode).Namespace,
									  Class = typeof(SimpleValueDummyNode).Name
								  };
				}
				return _PluginInfo;
			}
		}

		#endregion

		#region Fields

		private IPluginHost _Host;
		private IValueConfig _InputCount;
		private IValueConfig _OutputCount;
		private readonly Stack<IValueIn> _Inputs = new Stack<IValueIn>();
		private readonly Stack<IValueOut> _Outputs = new Stack<IValueOut>();

		#endregion

		#region Properties

		private int InputCount
		{
			get
			{
				if(_InputCount == null || _InputCount.SliceCount == 0)
					return 0;
				double d;
				_InputCount.GetValue(0, out d);
				return d < 0 ? 0 : (int)d;
			}
		}

		private int OutputCount
		{
			get
			{
				if(_OutputCount == null || _OutputCount.SliceCount == 0)
					return 0;
				double d;
				_OutputCount.GetValue(0, out d);
				return d < 0 ? 0 : (int)d;
			}
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void DbgLog(string str, params object[] args)
		{
			if(_Host != null)
				_Host.Log(TLogType.Debug, "SimpleValueDummyNode: " + String.Format(str, args));
		}

		private void InitInputs()
		{
			var oldCount = _Inputs.Count;
			var count = this.InputCount;
			if(count < oldCount)
			{
				DbgLog("Removing input pins");
				while(_Inputs.Count > count)
				{
					var input = _Inputs.Pop();
					_Host.DeletePin(input);
				}
			}
			else if(count > oldCount)
			{
				DbgLog("Adding input pins");
				for(var i = 0; i < count - oldCount; i++)
				{
					IValueIn input;
					_Host.CreateValueInput("In" + i, 1, null, TSliceMode.Dynamic, TPinVisibility.True, out input);
					input.Order = i;
					_Inputs.Push(input);
				}
			}
		}

		private void InitOutputs()
		{
			var oldCount = _Outputs.Count;
			var count = this.OutputCount;
			if(count < oldCount)
			{
				DbgLog("Removing output pins");
				while(_Outputs.Count > count)
				{
					var output = _Outputs.Pop();
					_Host.DeletePin(output);
				}
			}
			else if(count > oldCount)
			{
				DbgLog("Adding output pins");
				for(var i = 0; i < count - oldCount; i++)
				{
					IValueOut output;
					_Host.CreateValueOutput("Out" + i, 1, null, TSliceMode.Dynamic, TPinVisibility.True, out output);
					output.Order = i;
					_Outputs.Push(output);
				}
			}
		}

		#endregion

		#region IPlugin Members

		public bool AutoEvaluate
		{
			get { return true; }
		}

		public void Configurate(IPluginConfig input)
		{
			if(input == _InputCount)
			{
				InitInputs();
			}
			else if(input == _OutputCount)
			{
				InitOutputs();
			}
			DbgLog("Configurate pin: " + input.Name);
		}

		public void Evaluate(int spreadMax)
		{
			InitInputs();
			InitOutputs();
		}

		public void SetPluginHost(IPluginHost host)
		{
			_Host = host;
			host.CreateValueConfig("InputCount", 1, null, TSliceMode.Single, TPinVisibility.OnlyInspector, out _InputCount);
			_InputCount.SetSubType(0, MaxCount, 1, 1, false, false, true);
			host.CreateValueConfig("OutputCount", 1, null, TSliceMode.Single, TPinVisibility.OnlyInspector, out _OutputCount);
			_OutputCount.SetSubType(0, MaxCount, 1, 1, false, false, true);
			InitInputs();
			InitOutputs();
		}

		#endregion
	}

	#endregion

}
