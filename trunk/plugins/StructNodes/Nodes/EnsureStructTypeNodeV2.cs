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

	#region EnsureStructTypeNodeV2

	[PluginInfo(Name = "EnsureType", Category = "Struct", Version = "Single V2")]
	public sealed class EnsureStructTypeNodeV2 : StructNodeBaseV2
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly IDiffSpread<string> _PartTypesConfig;

		//private bool _Attached;

		//[Input("PartTypes", IsSingle = true)]
		//private IDiffSpread<string> _PartTypesInput;

		private bool _Invalidate = true;
		private bool _InvalidateCount = true;

		[Output("ActualPartTypeKey", IsSingle = true)]
		private ISpread<string> _ActualPartTypeKeyOutput;

		[Output("Guid", IsSingle = true)]
		private ISpread<string> _GuidOutput;

		[Output("FriendlyName", IsSingle = true)]
		private ISpread<string> _FriendlyNameOutput;

		[Output("UsageCount", IsSingle = true)]
		private ISpread<int> _UsageCountOutput;

		[Output("Changed", IsBang = true, IsSingle = true)]
		private ISpread<bool> _Changed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public EnsureStructTypeNodeV2(IPluginHost host, [Config("PartTypes", IsSingle = true)]IDiffSpread<string> partTypesConfig)
			: base(host)
		{
			_PartTypesConfig = partTypesConfig;
			_PartTypesConfig.Changed += this.PartTypes_Changed;
			StructTypeRegistry.TypeUsageCountChanged += this.TypeRegistry_CountChanged;
		}

		//[ImportingConstructor]
		//public EnsureStructTypeNodeV2(IPluginHost host)
		//    : base(host)
		//{
		//}

		#endregion

		#region Methods

		private void TypeRegistry_CountChanged(object sender, CountChangedEventArgs<Guid> e)
		{
			if(this.Type != null && this.Type.Id == e.Key)
			{
				_InvalidateCount = true;
			}
		}

		private void PartTypes_Changed(IDiffSpread<string> partTypes)
		{
			CheckDisposed();
			SetType(StructTypeDefinition.FilterPartTypesKey(partTypes[0]));
			_Invalidate = true;
		}

		public override void Evaluate(int spreadMax)
		{
			//if(!_Attached)
			//{
			//    _PartTypesInput.Changed += this.PartTypes_Changed;
			//    _Attached = true;
			//}
			var type = this.Type;
			if(_Invalidate || _InvalidateCount)
			{
				if(type == null)
					_UsageCountOutput[0] = 0;
				else
					_UsageCountOutput[0] = StructTypeRegistry.GetTypeUsageCount(type.Id);
				_InvalidateCount = false;
				_Changed[0] = true;
				if(_Invalidate)
				{
					_ActualPartTypeKeyOutput[0] = type == null ? null : type.PartTypesKey;
					_GuidOutput[0] = type == null ? null : type.Id.ToString();
					_FriendlyNameOutput[0] = type == null ? null : type.FriendlyTypeName;
					_Invalidate = false;
				}
			}
			else
			{
				_Changed[0] = false;
			}
		}

		protected override void Dispose()
		{
			if(_PartTypesConfig != null)
				_PartTypesConfig.Changed -= this.PartTypes_Changed;
			//if(_PartTypesInput != null)
			//    _PartTypesInput.Changed -= this.PartTypes_Changed;
			base.Dispose();
		}

		#endregion

	}

	#endregion

}
