using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace TE.VVVV.Plugins
{

	#region BooleanOperationPluginBase

	public abstract class BooleanOperationPluginBase : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Operations", DefaultEnumEntry = "And")]
		private ISpread<BooleanOperation> _Operations;

		[Output("OutputValues")]
		private ISpread<bool> _OutputValues;

		#endregion

		#region Properties

		protected abstract int InputCount { get; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected abstract bool EvaluateOutputValue(int index, BooleanOperation op);

		protected virtual void EvaluateOutputValues(ISpread<bool> outputValues, int count)
		{
			outputValues.SliceCount = count;
			for(var i = 0; i < count; i++)
				_OutputValues[i] = EvaluateOutputValue(i, _Operations[i]);
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			var count = Math.Min(this.InputCount, spreadMax);
			EvaluateOutputValues(_OutputValues, count);
		}

		#endregion

	}

	#endregion

}
