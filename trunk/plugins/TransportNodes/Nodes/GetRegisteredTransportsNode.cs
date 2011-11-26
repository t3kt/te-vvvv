using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransportNodes.Core;
using VVVV.PluginInterfaces.V2;

namespace TransportNodes.Nodes
{

	#region GetRegisteredTransportsNode

	[PluginInfo(Name = TEShared.Names.Nodes.Transports,
		Category = TEShared.Names.Categories.Transport,
		Author = TEShared.Names.Author)]
	public sealed class GetRegisteredTransportsNode : IPluginEvaluate, IDisposable
	{

		#region Static/Constant

		#endregion

		#region Fields

		private bool _Invalidate = true;

		[Input("Refresh", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _RefreshInput;

		[Output("Keys")]
		private ISpread<string> _KeysOutput;

		[Output("Guids")]
		private ISpread<string> _GuidsOutput;

		[Output("Usages")]
		private ISpread<int> _UsagesOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public GetRegisteredTransportsNode()
		{
			TransportRegistry.Registered += this.Registry_RegChanged;
			TransportRegistry.Unregistered += this.Registry_RegChanged;
			TransportRegistry.UsageCountChanged += this.Registry_UsageCountChanged;
		}

		#endregion

		#region Methods

		private void Registry_UsageCountChanged(object sender, CountChangedEventArgs<Guid> e)
		{
			this._Invalidate = true;
		}

		private void Registry_RegChanged(object sender, TransportEventArgs e)
		{
			this._Invalidate = true;
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(this._Invalidate || this._RefreshInput.IsChanged)
			{
				var transports = TransportRegistry.RegisteredTransports.ToList();
				var count = _KeysOutput.SliceCount = _GuidsOutput.SliceCount = _UsagesOutput.SliceCount = transports.Count;
				for(var i = 0; i < count; i++)
				{
					var transport = transports[i];
					_KeysOutput[i] = transport.Key;
					_GuidsOutput[i] = transport.Id.ToString();
					_UsagesOutput[i] = TransportRegistry.GetUsageCount(transport);
				}
				this._Invalidate = false;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			TransportRegistry.Registered -= this.Registry_RegChanged;
			TransportRegistry.Unregistered -= this.Registry_RegChanged;
			TransportRegistry.UsageCountChanged -= this.Registry_UsageCountChanged;
		}

		#endregion
	}

	#endregion

}
