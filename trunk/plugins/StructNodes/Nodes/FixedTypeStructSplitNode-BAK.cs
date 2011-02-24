using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region FixedTypeStructSplitNode

	[PluginInfo(Name = "StructSplit", Category = "Struct", Version = "FixedType V2")]
	public class FixedTypeStructSplitNode : StructNodeBaseV2
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly IDiffSpread<string> _PartTypesConfig;

		[Input("Input")]
		private IDiffSpread<StructData> _Input;

		private readonly List<IPluginOut> _PartOutputs = new List<IPluginOut>();

		private bool _Invalidate;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public FixedTypeStructSplitNode(IPluginHost host, [Config("PartTypes", IsSingle = true)]IDiffSpread<string> partTypesConfig)
			: base(host)
		{
			_PartTypesConfig = partTypesConfig;
			_PartTypesConfig.Changed += this.PartTypes_Changed;
		}

		#endregion

		#region Methods

		protected virtual void PartTypes_Changed(IDiffSpread<string> partTypes)
		{
			CheckDisposed();
			SetType(StructTypeDefinition.FilterPartTypesKey(partTypes[0]));
		}

		private void ClearPartOutputs()
		{
			foreach(var output in _PartOutputs)
				_Host.DeletePin(output);
			_PartOutputs.Clear();
		}

		protected override void OnTypeChanged()
		{
			ClearPartOutputs();
			var type = this.Type;
			if(type != null)
			{
				for(var i = 0; i < type.PartTypes.Count; i++)
				{
					var partType = type.PartTypes[i];
					_PartOutputs.Add(StructTypeDefinition.CreatePartOutputPin(_Host,
 partType, String.Format("Input{0}{1}", i, StructTypeDefinition.GetPartTypeAbbreviation(partType)), 1, TSliceMode.Dynamic, TPinVisibility.True));
				}
			}
			_Invalidate = true;
		}

		private void AssertStructTypeValid(StructTypeDefinition type)
		{
			if(type != null && type != this.Type)
				throw new InvalidOperationException(String.Format("Invalid Struct Type {0} (expecting: {1})", type, this.Type));
		}

		public override void Evaluate(int spreadMax)
		{
			if(_Invalidate || _Input.IsChanged)
			{
				var count = _Input.SliceCount;
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
				_Invalidate = false;
			}
		}

		protected override void Dispose()
		{
			if(_PartTypesConfig != null)
				_PartTypesConfig.Changed -= this.PartTypes_Changed;
			ClearPartOutputs();
			base.Dispose();
		}

		#endregion

	}

	#endregion

}
