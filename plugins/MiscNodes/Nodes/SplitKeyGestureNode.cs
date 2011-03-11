using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region SplitKeyGestureNode

	//[PluginInfo(Name = TEShared.Names.Nodes.KeyGesture,
	//    Category = TEShared.Names.Categories.KeyGesture,
	//    Version = TEShared.Names.Versions.Split,
	//    Author = TEShared.Names.Author)]
	[TESharedAnnotations.Incomplete]
	public sealed class SplitKeyGestureNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("KeyGesture")]
		private IDiffSpread<KeyGesture> _KeyGestureInput;

		[Output("Key")]
		private ISpread<Key> _KeyOutput;

		[Output("ModifierKeys")]
		private ISpread<ModifierKeys> _ModifierKeysOutput;

		[Output("DisplayString")]
		private ISpread<string> _DisplayStringOutput;

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
				var count = _KeyOutput.SliceCount = _ModifierKeysOutput.SliceCount = _DisplayStringOutput.SliceCount = _KeyGestureInput.SliceCount;
				for(var i = 0; i < count; i++)
				{
					var kg = _KeyGestureInput[i];
					if(kg == null)
					{
						_KeyOutput[i] = default(Key);
						_ModifierKeysOutput[i] = default(ModifierKeys);
						_DisplayStringOutput[i] = null;
					}
					else
					{
						_KeyOutput[i] = kg.Key;
						_ModifierKeysOutput[i] = kg.Modifiers;
						//_DisplayStringOutput[i] = kg.DisplayString;
						_DisplayStringOutput[i] = kg.GetDisplayStringForCulture(CultureInfo.CurrentUICulture);
					}
				}
			}
		}

		#endregion
	}

	#endregion

}
