using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region JoinKeyEventNode

	[Obsolete]
	[PluginInfo(Name = TEShared.Names.Nodes.KeyEvent,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Join,
		Author = TEShared.Names.Author,
		Warnings = TEShared.Names.Warnings.Obsolete)]
	public sealed class JoinKeyEventNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Config("UseModifiersInput", IsSingle = true)]
		private IDiffSpread<bool> _UseModifiersInputConfig;

		[Input("KeyData", DefaultEnumEntry = "None")]
		private IDiffSpread<Keys> _KeyDataInput;

		[Input("Modifiers", Visibility = PinVisibility.Hidden, DefaultEnumEntry = "None")]
		private IDiffSpread<Keys> _ModifiersInput;

		[Output("KeyEvent")]
		private ISpread<KeyEventInfo> _KeyEventOutput;

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
			if(_UseModifiersInputConfig.IsChanged || _KeyDataInput.IsChanged || _ModifiersInput.IsChanged)
			{
				if(_UseModifiersInputConfig[0])
				{
					_KeyEventOutput.SliceCount = Math.Max(_KeyDataInput.SliceCount, _ModifiersInput.SliceCount);
					for(var i = 0; i < _KeyEventOutput.SliceCount; i++)
					{
						_KeyEventOutput[i] = new KeyEventInfo(_KeyDataInput[i] | _ModifiersInput[i]);
					}
				}
				else
				{
					_KeyEventOutput.SliceCount = _KeyDataInput.SliceCount;
					for(var i = 0; i < _KeyEventOutput.SliceCount; i++)
						_KeyEventOutput[i] = new KeyEventInfo(_KeyDataInput[i]);
				}
			}
		}

		#endregion
	}

	#endregion

}
