using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Nodes
{

	#region RSCountNode

	public class RSCountNode : IPlugin
	{

		#region Static / Constant

		public static IPluginInfo PluginInfo
		{
			get
			{
				return
					new PluginInfo
					{
						Name = "RSCount",
						Category = "Value",
						Version = "Advanced",
						Help = "Gets number of senders for S (Value Advanced), S (Color Advanced), and S (String Advanced)",
						Bugs = String.Empty,
						Credits = String.Empty,
						Warnings = String.Empty,
						Author = "te",
						Namespace = typeof(RSCountNode).Namespace,
						Class = typeof(RSCountNode).Name
					};
			}
		}

		private static Dictionary<string, int> _ValueInstanceCounts;

		private static Dictionary<string, int> GetValueInstanceCounts()
		{
			var type = Type.GetType("VVVV.Lib.DoubleDataHolder, RSNodes", true);
			if(type == null)
				throw new Exception("Error finding RSNodes DataHolder type");
			var instanceProperty = type.GetProperty("Instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
			if(instanceProperty == null)
				throw new Exception("Error finding RSNodes DataHolder instance property");
			var instance = instanceProperty.GetValue(null, null);
			if(instance == null)
				throw new Exception("Error getting RSNodes DataHolder instance");
			var instanceCountsField = type.GetField("instancecount", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			if(instanceCountsField == null)
				throw new Exception("Error finding RSNodes DataHolder instancecounts field");
			var instanceCounts = instanceCountsField.GetValue(instance);
			if(instanceCounts == null)
				throw new Exception("Error getting RSNodes DataHolder instancecounts");
			return (Dictionary<string, int>)instanceCounts;
		}

		#endregion

		#region Fields

		private IPluginHost _Host;
		private IValueOut _ValueCount;
		private IValueOut _ColorCount;
		private IValueOut _StringCount;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IPlugin Members

		public bool AutoEvaluate
		{
			get { return true; }
		}

		public void Configurate(IPluginConfig input)
		{
		}

		public void Evaluate(int spreadMax)
		{
			if(_ValueInstanceCounts == null)
			{
				_ValueInstanceCounts = GetValueInstanceCounts();
			}
			_ValueCount.SliceCount = 1;
			_ValueCount.SetValue(0, _ValueInstanceCounts.Count);
		}

		public void SetPluginHost(IPluginHost host)
		{
			_Host = host;
			host.CreateValueOutput("ValueSendCount", 1, null, TSliceMode.Single, TPinVisibility.True, out _ValueCount);
			_ValueCount.SetSubType(0, Double.MaxValue, 1, 0, false, false, true);
			host.CreateValueOutput("ColorSendCount", 1, null, TSliceMode.Single, TPinVisibility.True, out _ColorCount);
			_ColorCount.SetSubType(0, Double.MaxValue, 1, 0, false, false, true);
			host.CreateValueOutput("StringSendCount", 1, null, TSliceMode.Single, TPinVisibility.True, out _StringCount);
			_StringCount.SetSubType(0, Double.MaxValue, 1, 0, false, false, true);
		}

		#endregion
	}

	#endregion

}
