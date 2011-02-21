using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace TE.VVVV.Plugins
{

	#region BooleanOperationPlugin

	[PluginInfo(Name = "BoolOp", Category = "Boolean")]
	public class BooleanOperationPlugin : IPluginEvaluate
	{

		#region Static / Constant

		public static T GetItemWrapped<T>(ISpread<T> spread, int index, int count)
		{
			return spread[Math.Abs(Math.Min(index, count - 1)) % spread.SliceCount];
		}

		#endregion

		#region Fields

		[Input("Input1Values")]
		private ISpread<bool> _Input1Values;

		[Input("Input2Values")]
		private ISpread<bool> _Input2Values;

		[Input("Operations", DefaultEnumEntry = "And")]
		private ISpread<BooleanOperation> _Operations;

		[Output("OutputValues")]
		private ISpread<bool> _OutputValues;

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
			var count = _OutputValues.SliceCount =
				Math.Min(
					_Input1Values.SliceCount.CombineSpreads(_Input2Values.SliceCount)
											.CombineSpreads(_Operations.SliceCount),
					spreadMax);
			for(var i = 0; i < count; i++)
			{
				switch(GetItemWrapped(_Operations, i, count))
				{
				case BooleanOperation.And:
					_OutputValues[i] = GetItemWrapped(_Input1Values, i, count) & GetItemWrapped(_Input2Values, i, count);
					break;
				case BooleanOperation.Or:
					_OutputValues[i] = GetItemWrapped(_Input1Values, i, count) | GetItemWrapped(_Input2Values, i, count);
					break;
				case BooleanOperation.XOr:
					_OutputValues[i] = GetItemWrapped(_Input1Values, i, count) ^ GetItemWrapped(_Input2Values, i, count);
					break;
				}
			}
		}

		#endregion

	}

	#endregion

}
