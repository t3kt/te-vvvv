using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region TriggerCommandNode

	[PluginInfo(Name = TEShared.Names.Nodes.Trigger,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Manual,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		Help = "Manually trigger commands by name")]
	public sealed class TriggerCommandNode : IPluginBase, IDisposable, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("CommandId")]
		private ISpread<string> _CommandIdInput;

		[Input("Trigger", IsBang = true)]
		private IDiffSpread<bool> _TriggerInput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void Trigger_Changed(IDiffSpread<bool> triggerInput)
		{
			this.TriggerCommands();
		}

		private void TriggerCommands()
		{
			if(!_TriggerInput[0])
				return;
			var count = Math.Max(_CommandIdInput.SliceCount, _TriggerInput.SliceCount);
			for(var i = 0; i < count; i++)
			{
				if(_TriggerInput[i])
					CommandRegistry.TriggerCommand(_CommandIdInput[i]);
			}
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_TriggerInput.Changed += this.Trigger_Changed;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(_TriggerInput != null)
				_TriggerInput.Changed -= this.Trigger_Changed;
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
