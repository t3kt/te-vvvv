using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region KeyCodeToKeyNode

	//[PluginInfo(Name = TEShared.Names.Nodes.AsKey,
	//    Category = TEShared.Names.Categories.Keys,
	//    Author = TEShared.Names.Author)]
	[TESharedAnnotations.Incomplete]
	public sealed class KeyCodeToKeyNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("KeyCode")]
		private IDiffSpread<int> _CodeInput;

		[Output("Key")]
		private ISpread<Keys> _KeyOutput;

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
			if(_CodeInput.IsChanged)
			{
				_KeyOutput.SliceCount = _CodeInput.SliceCount;
				for(var i = 0; i < _KeyOutput.SliceCount; i++)
					_KeyOutput[i] = (Keys)_CodeInput[i];
			}
		}

		#endregion
	}

	#endregion

}
