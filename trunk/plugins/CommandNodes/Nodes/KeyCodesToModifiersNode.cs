using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region KeyCodesToModifiersNode

	[PluginInfo(Name = TEShared.Names.Nodes.Modifiers,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Split + TEShared.Names.AND + TEShared.Names.Versions.KeyCode,
		Author = TEShared.Names.Author)]
	public sealed class KeyCodesToModifiersNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("KeyCode")]
		private IDiffSpread<int> _KeyCodeInput;

		[Output("Command Modifiers", IsSingle = true)]
		private ISpread<CommandModifiers> _ModifiersOutput;

		[Output("Alt")]
		private ISpread<bool> _AltOutput;

		[Output("Control", IsSingle = true)]
		private ISpread<bool> _ControlOutput;

		[Output("Shift", IsSingle = true)]
		private ISpread<bool> _ShiftOutput;

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
			if(_KeyCodeInput.IsChanged)
			{
				var mods = _ModifiersOutput[0] = CommandUtil.GetKeyCodeModifiers(_KeyCodeInput);
				_AltOutput[0] = (mods & CommandModifiers.Alt) == CommandModifiers.Alt;
				_ControlOutput[0] = (mods & CommandModifiers.Control) == CommandModifiers.Control;
				_ShiftOutput[0] = (mods & CommandModifiers.Shift) == CommandModifiers.Shift;
			}
		}

		#endregion
	}

	#endregion

}
