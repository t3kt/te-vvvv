using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using TransportNodes.Core;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace TransportNodes.Nodes
{

	#region ControllerNode

	[PluginInfo(Name = TEShared.Names.Nodes.Controller,
		Category = TEShared.Names.Categories.Transport,
		Author = TEShared.Names.Author)]
	public sealed class ControllerNode : TransportNodeBase, IPluginEvaluate, IPartImportsSatisfiedNotification
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		[Input("State", IsSingle = true)]
		private IDiffSpread<TransportState> _StateInput;

		[Input("Play", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _PlayInput;

		[Input("Stop", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _StopInput;

		#endregion

		#region Constructors

		[ImportingConstructor]
		public ControllerNode(IPluginHost host, [Config("TransportKey", IsSingle = true)] IDiffSpread<string> transportKeyConfig)
			: base(host, transportKeyConfig)
		{
		}

		#endregion

		#region Methods

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
