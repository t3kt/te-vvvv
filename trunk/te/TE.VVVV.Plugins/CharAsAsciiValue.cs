using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region CharAsAsciiValue

	[PluginInfo(Name = "AsciiChar", Category = "String")]
	public class CharAsAsciiValue : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input")]
		private ISpread<string> _Input;

		[Output("Values", AsInt = true)]
		private ISpread<double> _Output;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			var outputs = new List<double>();
			var inCount = Math.Min(spreadMax, _Input.SliceCount);
			for(var i = 0; i < inCount; i++)
			{
				foreach(var c in _Input[i])
					outputs.Add(Convert.ToInt32(c));
			}
			_Output.SliceCount = outputs.Count;
			for(var i = 0; i < outputs.Count; i++)
				_Output[i] = outputs[i];
		}

		#endregion
	}

	#endregion

}
