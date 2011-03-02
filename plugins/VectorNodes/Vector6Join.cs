using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace VectorNodes
{

	#region Vector6Join

	[PluginInfo(Name = "Vector", Category = "Spreads", Version = "6d Join", Author = "te")]
	internal class Vector6Join : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input1")]
		private ISpread<double> _Input0;

		[Input("Input2")]
		private ISpread<double> _Input1;

		[Input("Input3")]
		private ISpread<double> _Input2;

		[Input("Input4")]
		private ISpread<double> _Input3;

		[Input("Input5")]
		private ISpread<double> _Input4;

		[Input("Input6")]
		private ISpread<double> _Input5;

		[Output("Output")]
		private ISpread<double> _Output;

		#endregion

		#region Properties

		private ISpread<double>[] Inputs
		{
			get { return new[] { _Input0, _Input1, _Input2, _Input3, _Input4, _Input5 }; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			var inputs = this.Inputs;
			spreadMax = Math.Max(spreadMax, inputs.Max(i => i.SliceCount));
			var sliceCount = spreadMax * 5;
			_Output.SliceCount = sliceCount;
			for(var i = 0; i < sliceCount; i++)
			{
				var input = inputs[i % 5];
				_Output[i] = input[(i / 5) % input.SliceCount];
			}
		}

		#endregion

	}

	#endregion

}
