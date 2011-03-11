using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region SplitModifiersNode

	[Obsolete]
	[PluginInfo(Name = TEShared.Names.Nodes.Modifiers,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Split,
		Author = TEShared.Names.Author,
		Warnings = TEShared.Names.Warnings.Obsolete)]
	public sealed class SplitModifiersNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Modifiers", DefaultEnumEntry = "None")]
		private IDiffSpread<Keys> _ModifiersInput;

		[Output("Control")]
		private ISpread<bool> _ControlOutput;

		[Output("Shift")]
		private ISpread<bool> _ShiftOutput;

		[Output("Alt")]
		private ISpread<bool> _AltOutput;

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
			if(_ModifiersInput.IsChanged)
			{
				var count =
					_ControlOutput.SliceCount =
					_ShiftOutput.SliceCount =
					_AltOutput.SliceCount =
					_ModifiersInput.SliceCount;
				for(var i = 0; i < count; i++)
				{
					var mods = _ModifiersInput[i];
					_ControlOutput[i] = (mods & Keys.Control) == Keys.Control;
					_ShiftOutput[i] = (mods & Keys.Shift) == Keys.Shift;
					_AltOutput[i] = (mods & Keys.Alt) == Keys.Alt;
				}
			}
		}

		#endregion
	}

	#endregion

}
