using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region TriggerMouseCommandNode

	[PluginInfo(Name = TEShared.Names.Nodes.Trigger,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Mouse,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		Help = "Trigger commands from mouse data")]
	public sealed class TriggerMouseCommandNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Left Button")]
		private IDiffSpread<bool> _LeftButtonInput;

		[Input("Middle Button")]
		private IDiffSpread<bool> _MiddleButtonInput;

		[Input("Right Button")]
		private IDiffSpread<bool> _RightButtonInput;

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
			if(_LeftButtonInput.IsChanged || _MiddleButtonInput.IsChanged || _RightButtonInput.IsChanged)
			{
				var count = Math.Max(_LeftButtonInput.SliceCount, Math.Max(_MiddleButtonInput.SliceCount, _RightButtonInput.SliceCount));
				for(var i = 0; i < count; i++)
				{
					CommandRegistry.TriggerMouseCommands(_LeftButtonInput[i], _MiddleButtonInput[i], _RightButtonInput[i], CommandModifiers.None);
				}
			}
		}

		#endregion
	}

	#endregion

}
