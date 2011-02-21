#region usings
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "ValueRSCount", Category = "Value", Help = "Gets number of senders for S (Value Advanced)", Tags = "")]
	#endregion PluginInfo
	public class ValueRSCountNode : IPluginEvaluate
	{
		#region fields & pins
		
		[Input("On", IsSingle=true)]
		ISpread<int> FOn;

		[Output("Count", IsSingle=true, AsInt=true)]
		ISpread<int> FCount;

		[Import]
		ILogger FLogger;
		#endregion fields & pins

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
			var instanceCountsField = type.BaseType.GetField("instancecount", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			if(instanceCountsField == null)
				throw new Exception("Error finding RSNodes DataHolder instancecounts field");
			var instanceCounts = instanceCountsField.GetValue(instance);
			if(instanceCounts == null)
				throw new Exception("Error getting RSNodes DataHolder instancecounts");
			return (Dictionary<string, int>)instanceCounts;
		}

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if(FOn.SliceCount>0&&FOn[0]!=0)
			{
				Dictionary<string, int> valueInstanceCounts=null;
				try
				{
					valueInstanceCounts = GetValueInstanceCounts();
					FLogger.Log(LogType.Debug, "Success getting value instance counts. Count="+valueInstanceCounts.Count);
				}
				catch(Exception ex)
				{
					FLogger.Log(LogType.Debug, "Error getting value instance counts: Message: "+ex.Message);
					FLogger.Log(LogType.Debug, "--stack trace: "+ex.StackTrace);
				}
			}
			

			//FLogger.Log(LogType.Debug, "Logging to Renderer (TTY)");
		}
	}
}
