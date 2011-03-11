using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region TriggerKeyboardCommandNode

	[PluginInfo(Name = TEShared.Names.Nodes.Trigger,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.KeyCode,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		Help = "Trigger commands from keyboard data")]
	public sealed class TriggerKeyboardCommandNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("KeyCode")]
		private IDiffSpread<int> _KeyCodeInput;

		//[Output("Dummy")]
		//private ISpread<double> _DummyOutput;

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
				CommandModifiers modifiers;
				var keys = CommandUtil.ProcessKeyCodes(_KeyCodeInput, out modifiers);
				foreach(var key in keys)
				{
					CommandRegistry.TriggerKeyCommands(key, modifiers);
				}
			}
		}

		#endregion
	}

	#endregion

}
