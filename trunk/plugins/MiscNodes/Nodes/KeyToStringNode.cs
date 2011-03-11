using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region KeyToStringNode

	[TESharedAnnotations.Incomplete]
	public sealed class KeyToStringNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Key")]
		private IDiffSpread<Keys> _KeyInput;

		private ISpread<string> _NameOutput;

		private ISpread<string> _KeyboardStrOutput;

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
				var count = _NameOutput.SliceCount = _KeyboardStrOutput.SliceCount = _KeyInput.SliceCount;
				for(var i = 0; i < count; i++)
				{
					_NameOutput[i] = _KeyInput[i].ToString();
					throw new NotImplementedException();
				}
			}
		}

		#endregion
	}

	#endregion

}
