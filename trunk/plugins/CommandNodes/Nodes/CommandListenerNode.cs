using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region CommandListenerNode

	[PluginInfo(Name = TEShared.Names.Nodes.Listener,
		Category = TEShared.Names.Categories.Command,
		Author = TEShared.Names.Author,
		Help = "Listen for specific commands")]
	public sealed class CommandListenerNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("CommandId")]
		private IDiffSpread<string> _CommandIdInput;

		[Output("Trigger", IsBang = true)]
		private ISpread<bool> _TriggerOutput;

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
			if(_CommandIdInput.IsChanged || _StateTracker.IsChanged)
			{
				var count = _TriggerOutput.SliceCount = _CommandIdInput.SliceCount;
				for(var i = 0; i < count; i++)
				{
					_TriggerOutput[i] = _StateTracker[_CommandIdInput[i]];
				}
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
