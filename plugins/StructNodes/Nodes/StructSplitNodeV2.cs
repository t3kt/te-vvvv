using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region StructSplitNodeV2

	[PluginInfo(Name = "Struct", Category = "Struct", Version = "Split V2")]
	public class StructSplitNodeV2 : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly IPluginHost _Host;

		private StructTypeDefinition _Type;

		[Input("Input")]
		protected IDiffSpread<StructData> _Input;

		private readonly List<IPluginOut> _Outputs = new List<IPluginOut>();

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public StructSplitNodeV2(IPluginHost host)
		{
			_Host = host;
			StructTypeRegistry.OfferHost(host);
		}

		#endregion

		#region Methods

		private void ClearOutputPins()
		{
			foreach(var output in _Outputs)
				_Host.DeletePin(output);
			_Outputs.Clear();
		}

		private void PrepareOutputPins()
		{
			ClearOutputPins();
			if(_Type != null)
			{
				for(var i = 0; i < _Type.PartTypes.Count; i++)
				{
					var partType = _Type.PartTypes[i];
					_Outputs.Add(StructTypeDefinition.CreatePartOutputPin(_Host, partType, String.Format("Input{0}{1}", i, StructTypeDefinition.GetPartTypeAbbreviation(partType)), 1, TSliceMode.Dynamic, TPinVisibility.True));
				}
			}
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_Type == null)
			{
				ClearOutputPins();
			}
			if(_Input.IsChanged)
			{
				if(_Input.SliceCount == 0)
				{
					ClearOutputPins();
				}
				else
				{
					if(_Input[0].TypeDefinition != _Type)
					{
						_Type = _Input[0].TypeDefinition;
						PrepareOutputPins();
					}
					for(var i = 0; i < _Input.SliceCount; i++)
					{
						Debug.Assert(_Input[i].TypeDefinition == _Type);
						_Input[i].WriteOutputs(_Outputs, i);
					}
				}
			}
		}

		#endregion
	}

	#endregion

}
