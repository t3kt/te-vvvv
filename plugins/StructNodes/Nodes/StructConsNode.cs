using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Lib;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region StructConsNode

	[PluginInfo(Name = Names.Nodes.Cons,
		Category = Names.Category,
		Author = Names.Author)]
	public sealed class StructConsNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input", IsPinGroup = true)]
		private ISpread<ISpread<StructData>> _Input;

		[Output("Output")]
		private ISpread<ISpread<StructData>> _Output;

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
			_Output.SliceCount = _Input.SliceCount;
			for(var i = 0; i < _Input.SliceCount; i++)
				_Output[i] = _Input[i];
		}

		#endregion

	}

	#endregion

}
