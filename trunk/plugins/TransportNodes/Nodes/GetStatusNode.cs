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

	#region GetStatusNode

	[PluginInfo(Name = TEShared.Names.Nodes.Status,
		Category = TEShared.Names.Categories.Transport,
		Author = TEShared.Names.Author)]
	public sealed class GetStatusNode : TransportNodeBase, IPluginEvaluate
	{

		#region Static/Constant

		#endregion

		#region Fields

		private bool _IsChanged;

		[Output("State", IsSingle = true)]
		private ISpread<TransportState> _StateOut;

		[Output("State Name", IsSingle = true)]
		private ISpread<string> _StateNameOut;

		[Output("Playing", IsSingle = true)]
		private ISpread<bool> _IsPlayingOut;

		[Output("Changed", IsBang = true, IsSingle = true)]
		private ISpread<bool> _IsChangedOut;

		[Output("Guid", IsSingle = true)]
		private ISpread<string> _GuidOut;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public GetStatusNode(IPluginHost host, [Config("TransportKey", IsSingle = true)] IDiffSpread<string> transportKeyConfig)
			: base(host, transportKeyConfig)
		{
		}

		#endregion

		#region Methods

		private void Transport_StateChanged(object sender, EventArgs eventArgs)
		{
			this._IsChanged = true;
		}

		protected override void OnTransportAcquired(Transport transport)
		{
			if(transport != null)
				transport.StateChanged += this.Transport_StateChanged;
			this._IsChanged = true;
		}

		protected override void OnTransportReleased(Transport transport)
		{
			if(transport != null)
				transport.StateChanged -= this.Transport_StateChanged;
			this._IsChanged = true;
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			this._IsChangedOut[0] = this._IsChanged;
			if(this._IsChanged)
			{
				var transport = this.Transport;
				if(transport == null)
				{
					this._StateOut[0] = TransportState.Stopped;
					this._IsPlayingOut[0] = false;
					this._GuidOut[0] = null;
					this._StateNameOut[0] = null;
				}
				else
				{
					this._StateOut[0] = transport.State;
					this._IsPlayingOut[0] = transport.IsPlaying;
					this._GuidOut[0] = transport.Id.ToString();
					this._StateNameOut[0] = transport.State.ToString();
				}
				this._IsChanged = false;
			}
		}

		#endregion

	}

	#endregion

}
