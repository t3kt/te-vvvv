using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using VVVV.Core.Logging;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region FixedTypeStructJoinNode

	// THIS WORKS
	[PluginInfo(Name = TEShared.Names.Nodes.Struct,
		Category = TEShared.Names.Categories.Struct,
		Version = TEShared.Names.Versions.Join +
			TEShared.Names.AND +
			TEShared.Names.Versions.FixedType,
		Author = TEShared.Names.Author)]
	public sealed class FixedTypeStructJoinNode : FixedTypeStructNodeBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly List<IPluginIn> _PartInputs = new List<IPluginIn>();

		[Output("Output")]
		private ISpread<StructData> _Output;

		#endregion

		#region Properties

		private bool ShouldEvaluate
		{
			get
			{
				if(_NeedsUpdate)
					return true;
				if(_PartInputs.Count == 0)
					return false;
				return _PartInputs.Any(p => p.PinIsChanged);
			}
		}

		#endregion

		#region Constructors

		[ImportingConstructor]
		public FixedTypeStructJoinNode(IPluginHost host, [Import] ILogger logger, [Config("PartTypes", IsSingle = true)]IDiffSpread<string> partTypesConfig)
			: base(host, logger, partTypesConfig)
		{
		}

		#endregion

		#region Methods

		protected override void OnTypeChanged()
		{
			_Host.ClearPins(_PartInputs);
			if(this.Type != null)
				_PartInputs.AddRange(this.Type.CreatePartInputPins(_Host));
		}

		public override void Evaluate(int spreadMax)
		{
			if(this.ShouldEvaluate)
			{
				if(this.Type == null || _PartInputs.Count == 0)
				{
					_Output.SliceCount = 0;
				}
				else
				{
					var count = Math.Min(spreadMax, _PartInputs.Max(p => p.SliceCount));
					_Output.SliceCount = count;
					for(var i = 0; i < count; i++)
					{
						var data = this.Type.CreateInstance();
						data.ReadInputs(_PartInputs, i);
						_Output[i] = data;
					}
				}
			}
			_NeedsUpdate = false;
		}

		protected override void Dispose()
		{
			_Host.ClearPins(_PartInputs);
			base.Dispose();
		}

		#endregion

	}

	#endregion

}
