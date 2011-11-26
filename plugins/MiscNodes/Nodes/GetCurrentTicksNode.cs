using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region GetCurrentTicksNode

	[PluginInfo(Name = TEShared.Names.Nodes.CurrentTicks,
		Category = TEShared.Names.Categories.Animation,
		Author = TEShared.Names.Author)]
	public sealed class GetCurrentTicksNode : IPluginEvaluate
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly IPluginHost _Host;

		[Output("Ticks", IsSingle = true)]
		private ISpread<double> _TicksOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public GetCurrentTicksNode(IPluginHost host)
		{
			_Host = host;
		}

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			double ticks;
			_Host.GetCurrentTime(out ticks);
			_TicksOutput[0] = ticks;
		}

		#endregion

	}

	#endregion

}
