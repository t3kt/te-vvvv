using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region TriggerCommandNode

	[PluginInfo(Name = TEShared.Names.Nodes.Trigger,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Manual,
		Author = TEShared.Names.Author)]
	public sealed class TriggerCommandNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("CommandId")]
		private IDiffSpread<string> _CommandIdInput;

		[Input("Trigger", IsBang = true)]
		private IDiffSpread<bool> _TriggerInput;

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
			if(_CommandIdInput.IsChanged || _TriggerInput.IsChanged)
			{
				var count = Math.Max(_CommandIdInput.SliceCount, _TriggerInput.SliceCount);
				for(var i = 0; i < count; i++)
				{
					if(_TriggerInput[i])
						CommandRegistry.TriggerCommand(_CommandIdInput[i]);
				}
			}
		}

		#endregion

	}

	#endregion

}
