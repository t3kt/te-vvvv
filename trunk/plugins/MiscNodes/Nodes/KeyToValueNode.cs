using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region KeyToValueNode

	//[PluginInfo(Name = TEShared.Names.Nodes.AsValue,
	//    Category = TEShared.Names.Categories.Keys,
	//    Author = TEShared.Names.Author)]
	[TESharedAnnotations.Incomplete]
	public sealed class KeyToValueNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Key")]
		private IDiffSpread<Keys> _KeyInput;

		[Output("Value")]
		private ISpread<int> _ValueOutput;

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
			if(_KeyInput.IsChanged)
			{
				_ValueOutput.SliceCount = _KeyInput.SliceCount;
				for(var i = 0; i < _ValueOutput.SliceCount; i++)
					_ValueOutput[i] = (int)_KeyInput[i];
			}
		}

		#endregion

	}

	#endregion

}
