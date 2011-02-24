using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region GetStructTypesNode

	[PluginInfo(Name = Names.Nodes.GetTypes,
		Category = Names.Category,
		Author = Names.Author)]
	public sealed class GetStructTypesNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input")]
		private IDiffSpread<StructData> _StructInput;

		[Output("PartTypeKeys")]
		private ISpread<string> _PartTypeKeysOutput;

		[Output("Guids")]
		private ISpread<string> _GuidsOutput;

		[Output("FriendlyNames")]
		private ISpread<string> _FriendlyNamesOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public GetStructTypesNode(IPluginHost host)
		{
			StructTypeRegistry.OfferHost(host);
		}

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_StructInput.IsChanged)
			{
				var count = Math.Min(spreadMax, _StructInput.SliceCount);
				_PartTypeKeysOutput.SliceCount = _GuidsOutput.SliceCount = _FriendlyNamesOutput.SliceCount = count;
				for(var i = 0; i < count; i++)
				{
					var data = _StructInput[i];
					_PartTypeKeysOutput[i] = data == null ? null : data.TypeDefinition.PartTypesKey;
					_GuidsOutput[i] = data == null ? null : data.TypeDefinition.Id.ToString();
					_FriendlyNamesOutput[i] = data == null ? null : data.TypeDefinition.FriendlyTypeName;
				}
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
