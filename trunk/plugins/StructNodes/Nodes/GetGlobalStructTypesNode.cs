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

	#region GetGlobalStructTypesNode

	// this works
	[PluginInfo(Name = Names.Nodes.GetTypes,
		Category = Names.Category,
		Version = Names.Versions.Global,
		Author = Names.Author)]
	public sealed class GetGlobalStructTypesNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private bool _Invalidate = true;

		[Input("Refresh", IsSingle = true, IsBang = true)]
		private IDiffSpread<double> _RefreshInput;

		[Output("PartTypeKeys")]
		private ISpread<string> _PartTypeKeysOutput;

		[Output("Guids")]
		private ISpread<string> _GuidsOutput;

		[Output("FriendlyNames")]
		private ISpread<string> _FriendlyNamesOutput;

		[Output("UsageCounts")]
		private ISpread<int> _UsageCounts;

		[Output("Changed", IsBang = true, IsSingle = true)]
		private ISpread<bool> _Changed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public GetGlobalStructTypesNode(IPluginHost host)
		{
			StructTypeRegistry.TypeRegistered += this.TypeRegistry_Changed;
			StructTypeRegistry.TypeUnregistered += this.TypeRegistry_Changed;
			StructTypeRegistry.TypeUsageCountChanged += this.TypeRegistry_CountChanged;
			StructTypeRegistry.OfferHost(host);
		}

		#endregion

		#region Methods

		private void TypeRegistry_Changed(object sender, StructTypeEventArgs e)
		{
			_Invalidate = true;
		}

		private void TypeRegistry_CountChanged(object sender, CountChangedEventArgs<Guid> e)
		{
			_Invalidate = true;
		}

		#endregion

		#region IPlugin Members

		public void Evaluate(int spreadMax)
		{
			if(_Invalidate || _RefreshInput.IsChanged)
			{
				var types = StructTypeRegistry.RegisteredTypes.OrderBy(t => t.PartTypesKey).ToArray();
				_PartTypeKeysOutput.SliceCount = _GuidsOutput.SliceCount = _FriendlyNamesOutput.SliceCount = _UsageCounts.SliceCount = types.Length;
				for(var i = 0; i < types.Length; i++)
				{
					var type = types[i];
					_PartTypeKeysOutput[i] = type.PartTypesKey;
					_GuidsOutput[i] = type.Id.ToString();
					_FriendlyNamesOutput[i] = type.FriendlyTypeName;
					_UsageCounts[i] = StructTypeRegistry.GetTypeUsageCount(type.Id);
				}
				_Changed[0] = true;
				_Invalidate = false;
			}
			else
			{
				_Changed[0] = false;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			StructTypeRegistry.TypeRegistered -= this.TypeRegistry_Changed;
			StructTypeRegistry.TypeUnregistered -= this.TypeRegistry_Changed;
			StructTypeRegistry.TypeUsageCountChanged -= this.TypeRegistry_CountChanged;
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
