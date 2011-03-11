using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region GlobalCommandListenerNode

	[PluginInfo(Name = TEShared.Names.Nodes.Listener,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Global,
		Author = TEShared.Names.Author)]
	public sealed class GlobalCommandListenerNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Output("TriggeredCommands", IsBang = true)]
		private ISpread<string> _TriggeredCommandsOutput;

		private readonly TriggerStateTracker _StateTracker = new TriggerStateTracker();

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
			if(_StateTracker.States.IsChanged)
			{
				var commandIds = _StateTracker.States.GetAllSet();
				_TriggeredCommandsOutput.AssignFrom(commandIds);
			}
			_StateTracker.States.Reset();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_StateTracker.Dispose();
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
