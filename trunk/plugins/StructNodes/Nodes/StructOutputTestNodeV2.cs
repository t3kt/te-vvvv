using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Lib;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region StructOutputTestNodeV2

	[PluginInfo(Name = "OutputTest", Category = "Struct", Version = "V2")]
	public class StructOutputTestNodeV2 : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Output("Output Struct")]
		protected ISpread<StructData> _Output;

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
		}

		#endregion
	}

	#endregion

}
