using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Lib;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region StructInputTestNodeV2

	[PluginInfo(Name = "InputTest", Category = "Struct", Version = "V2")]
	public class StructInputTestNodeV2 : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input Struct")]
		protected IDiffSpread<StructData> _Input;

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
