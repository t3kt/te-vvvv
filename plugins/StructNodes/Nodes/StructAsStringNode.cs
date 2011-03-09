using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.Lib;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region StructAsStringNode

	// this works
	[PluginInfo(Name = TEShared.Names.Nodes.AsString,
		Category = TEShared.Names.Categories.Struct,
		Author = TEShared.Names.Author)]
	public sealed class StructAsStringNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input")]
		private IDiffSpread<StructData> _Input;

		[Output("Output")]
		private ISpread<string> _Output;

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
			if(_Input.IsChanged)
			{
				var count = Math.Min(spreadMax, _Input.SliceCount);
				_Output.SliceCount = count;
				for(var i = 0; i < count; i++)
					_Output[i] = Convert.ToString(_Input[i]);
			}
		}

		#endregion
	}

	#endregion

}
