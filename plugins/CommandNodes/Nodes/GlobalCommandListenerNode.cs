using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region GlobalCommandListenerNode

	[PluginInfo(Name = TEShared.Names.Nodes.Listener,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Global,
		Author = TEShared.Names.Author)]
	public sealed class GlobalCommandListenerNode : IPluginEvaluate, IDisposable, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Import]
		private ILogger _Logger;

		[Output("TriggeredCommands")]
		private ISpread<string> _TriggeredCommandsOutput;

		private TriggerStateTracker _StateTracker;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void CommandRegistry_CommandTriggered(object sender, CommandEventArgs e)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_StateTracker = new TriggerStateTracker { Logger = _Logger };
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_StateTracker.IsChanged)
			{
				var commandIds = _StateTracker.GetAllSet();
				_TriggeredCommandsOutput.AssignFrom(commandIds);
			}
			_StateTracker.Reset();
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
