using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region JoinKeyGestureNode

	//[PluginInfo(Name = TEShared.Names.Nodes.KeyGesture,
	//    Category = TEShared.Names.Categories.KeyGesture,
	//    Version = TEShared.Names.Versions.Join,
	//    Author = TEShared.Names.Author)]
	[TESharedAnnotations.Incomplete]
	public sealed class JoinKeyGestureNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Key")]
		private IDiffSpread<Key> _KeyInput;

		[Input("ModifierKeys")]
		private IDiffSpread<ModifierKeys> _ModifierKeysInput;

		[Input("DisplayString")]
		private IDiffSpread<string> _DisplayStringInput;

		[Output("KeyGesture")]
		private ISpread<KeyGesture> _KeyGestureOutput;

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
			if(_KeyInput.IsChanged || _ModifierKeysInput.IsChanged || _DisplayStringInput.IsChanged)
			{
				var count = _KeyGestureOutput.SliceCount = Math.Min(spreadMax, Math.Max(_KeyInput.SliceCount, Math.Max(_ModifierKeysInput.SliceCount, _DisplayStringInput.SliceCount)));
				for(var i = 0; i < count; i++)
					_KeyGestureOutput[i] = new KeyGesture(_KeyInput[i], _ModifierKeysInput[i], _DisplayStringInput[i]);
			}
		}

		#endregion
	}

	#endregion

}
