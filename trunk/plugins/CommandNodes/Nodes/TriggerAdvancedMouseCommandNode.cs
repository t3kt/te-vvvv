using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region TriggerAdvancedMouseCommandNode

	[PluginInfo(Name = TEShared.Names.Nodes.Trigger,
		Category = CommandNames.Categories.Command,
		Version = CommandNames.Versions.Mouse + TEShared.Names.AND + TEShared.Names.Versions.Advanced,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		Help = "Trigger commands from mouse data (with modifier keys)")]
	public class TriggerAdvancedMouseCommandNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		//[Input("X", Visibility = PinVisibility.Hidden)]
		//private IDiffSpread<double> _XInput;

		//[Input("Y", Visibility = PinVisibility.Hidden)]
		//private IDiffSpread<double> _YInput;

		//[Input("Mouse Wheel", Visibility = PinVisibility.Hidden)]
		//private IDiffSpread<int> _MouseWheelInput;

		[Input("Left Button")]
		private IDiffSpread<bool> _LeftButtonInput;

		[Input("Middle Button")]
		private IDiffSpread<bool> _MiddleButtonInput;

		[Input("Right Button")]
		private IDiffSpread<bool> _RightButtonInput;

		[Input("KeyCode")]
		private IDiffSpread<int> _KeyCodeInput;

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
			if(_LeftButtonInput.IsChanged || _MiddleButtonInput.IsChanged || _RightButtonInput.IsChanged || _KeyCodeInput.IsChanged)
			{
				var count = Math.Max(_LeftButtonInput.SliceCount, Math.Max(_MiddleButtonInput.SliceCount, _RightButtonInput.SliceCount));
				for(var i = 0; i < count; i++)
				{
					var modifiers = CommandUtil.GetKeyCodeModifiers(_KeyCodeInput);
					CommandRegistry.TriggerMouseCommands(_LeftButtonInput[i], _MiddleButtonInput[i], _RightButtonInput[i], modifiers);
				}
			}
		}

		#endregion
	}

	#endregion

}
