using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using VVVV.Lib;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region KeyGestureToStringNode

	//[PluginInfo(Name = TEShared.Names.Nodes.AsString,
	//    Category = TEShared.Names.Categories.KeyGesture,
	//    Author = TEShared.Names.Author)]
	[TESharedAnnotations.Incomplete]
	public sealed class KeyGestureToStringNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("KeyGesture")]
		private IDiffSpread<KeyGesture> _KeyGestureInput;

		[Output("DisplayString")]
		private ISpread<string> _DisplayStringOutput;

		[Output("KeyString")]
		private ISpread<string> _KeyStringOutput;

		[Output("ModifiersString")]
		private ISpread<string> _ModifiersStringOutput;

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
			if(_KeyGestureInput.IsChanged)
			{
				var count = _DisplayStringOutput.SliceCount = _KeyStringOutput.SliceCount = _ModifiersStringOutput.SliceCount = _KeyGestureInput.SliceCount;
				for(var i = 0; i < count; i++)
				{
					if(_KeyGestureInput[i] == null)
					{
						_DisplayStringOutput[i] = _KeyStringOutput[i] = _ModifiersStringOutput[i] = null;
					}
					else
					{
						//_DisplayStringOutput[i] = _KeyGestureInput[i].DisplayString;
						_DisplayStringOutput[i] = _KeyGestureInput[i].GetDisplayStringForCulture(CultureInfo.CurrentUICulture);
						_KeyStringOutput[i] = KeysUtil.KeyConverter.ConvertToString(_KeyGestureInput[i].Key);
						_ModifiersStringOutput[i] = KeysUtil.ModifierKeysConverter.ConvertToString(_KeyGestureInput[i].Modifiers);
					}
				}
			}
		}

		#endregion
	}

	#endregion

}
