#region usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;
#endregion usings

namespace VVVV.Nodes
{

	#region StrSplitPlugin

	#region PluginInfo
	[PluginInfo(Name = "StrSplit", Category = "String")]
	#endregion PluginInfo
	public class StrSplitPlugin : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input")]
		private ISpread<string> _Input;

		[Input("Separators", DefaultString = ",")]
		private ISpread<string> _Separators;

		[Input("SeparateLines", IsSingle = true)]
		private ISpread<bool> _SeparateLines;

		[Input("SplitOptions", DefaultEnumEntry = "None", IsSingle = true)]
		private ISpread<StringSplitOptions> _SplitOptions;

		[Output("Output")]//, IsPinGroup = true)]
		private ISpread<ISpread<string>> _Output;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private string[] GetSeparators()
		{
			var separators = _Separators.ToList();
			if(_SeparateLines.SliceCount > 0 && _SeparateLines[0])
				separators.Add("\n");
			return separators.ToArray();
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			var count = _Output.SliceCount =
						Math.Min(spreadMax, _Input.SliceCount);
			var separators = GetSeparators();
			var splitOptions = _SplitOptions.FirstOrDefault();
			for(var i = 0; i < count; i++)
			{
				var input = _Input[i];
				var parts = input.Split(separators, splitOptions);
				_Output[i] = new Spread<string>(parts);
			}
		}

		#endregion

	}

	#endregion

}
