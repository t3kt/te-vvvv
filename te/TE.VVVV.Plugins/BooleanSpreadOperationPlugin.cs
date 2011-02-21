using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace TE.VVVV.Plugins
{

	#region BooleanSpreadOperationPlugin

	//[PluginInfo(Name = "BoolOp", Category = "Boolean Spreads")]
	public class BooleanSpreadOperationPlugin : BooleanOperationPluginBase
	{

		#region Static / Constant

		private static bool EvaluateResult(IEnumerable<bool> inputs, BooleanOperation op)
		{
			switch(op)
			{
			case BooleanOperation.And:
				return inputs.All(x => x);
			case BooleanOperation.Or:
				return inputs.Any(x => x);
			case BooleanOperation.XOr:
				throw new NotSupportedException();
			default:
				throw new NotSupportedException();
			}
		}

		#endregion

		#region Fields

		[Input("InputValues")]
		private ISpread<bool> _InputValues;

		#endregion

		#region Properties

		protected override int InputCount
		{
			get { return _InputValues.SliceCount; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override bool EvaluateOutputValue(int index, BooleanOperation op)
		{
			throw new NotImplementedException();
		}

		protected override void EvaluateOutputValues(ISpread<bool> outputValues, int count)
		{
			var andResult = false;
			var orResult = false;
			var xorResult = false;

			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
