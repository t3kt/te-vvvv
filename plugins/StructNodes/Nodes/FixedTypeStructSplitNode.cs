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

	#region FixedTypeStructSplitNode

	// this works
	[PluginInfo(Name = TEShared.Names.Nodes.Struct,
		Category = TEShared.Names.Categories.Struct,
		Version = TEShared.Names.Versions.Split +
			TEShared.Names.AND +
			TEShared.Names.Versions.FixedType,
		Author = TEShared.Names.Author)]
	public sealed class FixedTypeStructSplitNode : FixedTypeStructNodeBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input")]
		private IDiffSpread<StructData> _Input;

		private readonly List<IPluginOut> _PartOutputs = new List<IPluginOut>();

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public FixedTypeStructSplitNode(IPluginHost host, [Import] ILogger logger, [Config("PartTypes", IsSingle = true)]IDiffSpread<string> partTypesConfig)
			: base(host, logger, partTypesConfig)
		{
		}

		#endregion

		#region Methods

		protected override void OnTypeChanged()
		{
			_Host.ClearPins(_PartOutputs);
			if(this.Type != null)
				_PartOutputs.AddRange(this.Type.CreatePartOutputPins(_Host));
		}

		private void AssertStructTypeValid(StructTypeDefinition type)
		{
			if(type != null && type != this.Type)
				throw new InvalidOperationException(String.Format("Invalid Struct Type {0} (expecting: {1})", type, this.Type));
		}

		public override void Evaluate(int spreadMax)
		{
			if(_NeedsUpdate || _Input.IsChanged)
			{
				if(this.Type == null || _PartOutputs.Count == 0)
				{
					foreach(var output in _PartOutputs)
						output.SliceCount = 0;
				}
				else
				{
					var count = Math.Min(spreadMax, _Input.SliceCount);
					foreach(var output in _PartOutputs)
						output.SliceCount = count;
					for(var i = 0; i < count; i++)
					{
						var data = _Input[i];
						if(data == null)
							continue;
						AssertStructTypeValid(data.TypeDefinition);
						data.WriteOutputs(_PartOutputs, i);
					}
				}
				_NeedsUpdate = false;
			}
		}

		protected override void Dispose()
		{
			_Host.ClearPins(_PartOutputs);
			base.Dispose();
		}

		#endregion

	}

	#endregion

}
