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

	#region FilterStructsOfTypeNode

	// THIS WORKS
	[PluginInfo(Name = Names.Nodes.OfType,
		Category = Names.Category,
		Author = Names.Author)]
	public sealed class FilterStructsOfTypeNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private HashSet<string> _TypeKeys;

		[Input("Input")]
		private IDiffSpread<StructData> _StructInput;

		[Input("PartTypes")]
		private IDiffSpread<string> _PartTypesInput;

		[Input("InvertFilter", IsSingle = true, Visibility = PinVisibility.OnlyInspector)]
		private IDiffSpread<bool> _InvertFilterConfig;

		[Input("RemoveNulls", IsSingle = true, DefaultValue = 1, Visibility = PinVisibility.OnlyInspector)]
		private IDiffSpread<bool> _RemoveNullsConfig;

		private bool _Invalidate = true;

		[Output("Output")]
		private ISpread<StructData> _StructOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public FilterStructsOfTypeNode(IPluginHost host)
		{
			StructTypeRegistry.OfferHost(host);
		}

		#endregion

		#region Methods

		private void RebuildTypeKeySet()
		{
			_TypeKeys = new HashSet<string>();
			if(_PartTypesInput.SliceCount > 0)
			{
				foreach(var key in _PartTypesInput)
				{
					var filteredKey = StructTypeDefinition.FilterPartTypesKey(key);
					if(filteredKey != null)
						_TypeKeys.Add(filteredKey);
				}
			}
			_Invalidate = true;
		}

		private bool ShouldInclude(StructData data, bool invert, bool removeNulls)
		{
			if(data == null)
				return !removeNulls;
			if(_TypeKeys.Contains(data.TypeDefinition.PartTypesKey))
				return !invert;
			return invert;
		}

		private IEnumerable<StructData> FilterInputsIterator()
		{
			var count = _StructInput.SliceCount;
			var invert = _InvertFilterConfig[0];
			var removeNulls = _RemoveNullsConfig[0];
			for(var i = 0; i < count; i++)
			{
				var data = _StructInput[i];
				if(ShouldInclude(data, invert, removeNulls))
					yield return data;
				else if(!removeNulls)
					yield return null;
			}
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_TypeKeys == null || _PartTypesInput.IsChanged)
			{
				RebuildTypeKeySet();
			}
			if(_Invalidate || _StructInput.IsChanged || _InvertFilterConfig.IsChanged || _RemoveNullsConfig.IsChanged)
			{
				var count = Math.Min(spreadMax, _StructInput.SliceCount);
				var values = FilterInputsIterator().Take(count).ToList();
				_StructOutput.AssignFrom(values);
				_Invalidate = false;
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
