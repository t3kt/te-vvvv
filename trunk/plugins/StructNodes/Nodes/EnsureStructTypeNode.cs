using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using VVVV.Core.Logging;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region EnsureStructTypeNode

	// THIS WORKS
	[PluginInfo(Name = TEShared.Names.Nodes.EnsureType,
		Category = TEShared.Names.Categories.Struct,
		Author = TEShared.Names.Author)]
	public sealed class EnsureStructTypeNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly List<StructTypeDefinition> _Types = new List<StructTypeDefinition>();
		private bool _Disposed;
		private readonly bool _ProvidedLogger;

		[Input("PartTypes")]
		private IDiffSpread<string> _PartTypesInput;

		private bool _InputAttached;

		private bool _Invalidate = true;
		private bool _InvalidateCount = true;

		[Output("ActualPartTypeKey")]
		private ISpread<string> _ActualPartTypeKeyOutput;

		[Output("Guid")]
		private ISpread<string> _GuidOutput;

		[Output("FriendlyName")]
		private ISpread<string> _FriendlyNameOutput;

		[Output("UsageCount")]
		private ISpread<int> _UsageCountOutput;

		[Output("Changed", IsBang = true, IsSingle = true)]
		private ISpread<bool> _Changed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public EnsureStructTypeNode(IPluginHost host, [Import] ILogger logger)
		{
			_ProvidedLogger = StructTypeRegistry.OfferLogger(logger);
			StructTypeRegistry.TypeUsageCountChanged += this.TypeRegistry_CountChanged;
		}

		#endregion

		#region Methods

		private void CheckDisposed()
		{
			if(_Disposed)
				throw new ObjectDisposedException(GetType().FullName);
		}

		private void ResizeTypeList(int requiredCount)
		{
			Debug.Assert(requiredCount >= 0);
			var diff = requiredCount - _Types.Count;
			if(diff == 0)
				return;
			if(diff > 0)//grow
			{
				while(_Types.Count < requiredCount)
					_Types.Add(null);
			}
			else//shrink
			{
				while(_Types.Count > requiredCount)
				{
					StructTypeRegistry.ReleaseTypeDefinition(_Types[_Types.Count - 1]);
					_Types.RemoveAt(_Types.Count - 1);
				}
			}
		}

		private void SetType(int i, string partTypesKey)
		{
			Debug.Assert(i >= 0);
			//if(i >= _Types.Count)
			//    ResizeTypeList(i + 1);
			var type = _Types[i];
			if((type == null && !String.IsNullOrEmpty(partTypesKey)) ||
				(type != null && partTypesKey != type.PartTypesKey))
			{
				StructTypeRegistry.ReleaseTypeDefinition(type);
				type = StructTypeRegistry.RequestTypeDefinition(partTypesKey);
				_Types[i] = type;
			}
		}

		private void SetTypes(IList<string> partTypeKeys)
		{
			ResizeTypeList(partTypeKeys == null ? 0 : partTypeKeys.Count);
			if(partTypeKeys != null)
			{
				for(var i = 0; i < partTypeKeys.Count; i++)
					SetType(i, partTypeKeys[i]);
			}
		}

		private void TypeRegistry_CountChanged(object sender, CountChangedEventArgs<Guid> e)
		{
			if(_Types.FindIndex(t => t != null && t.Id == e.Key) != -1)
				_InvalidateCount = true;
		}

		private void PartTypes_Changed(IDiffSpread<string> partTypes)
		{
			CheckDisposed();
			SetTypes(partTypes.ToArray());
			_Invalidate = true;
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(!_InputAttached)
			{
				_PartTypesInput.Changed += this.PartTypes_Changed;
				_InputAttached = true;
				PartTypes_Changed(_PartTypesInput);
			}
			if(_Invalidate || _InvalidateCount || _PartTypesInput.IsChanged)
			{
				_ActualPartTypeKeyOutput.SliceCount =
					_GuidOutput.SliceCount =
					_FriendlyNameOutput.SliceCount =
					_UsageCountOutput.SliceCount = _Types.Count;
				for(var i = 0; i < _Types.Count; i++)
					_UsageCountOutput[i] = StructTypeRegistry.GetTypeUsageCount(this._Types[i]);
				_InvalidateCount = false;
				_Changed[0] = true;
				if(_Invalidate)
				{
					for(var i = 0; i < _Types.Count; i++)
					{
						var type = _Types[i];
						_ActualPartTypeKeyOutput[i] = type == null ? null : type.PartTypesKey;
						_GuidOutput[i] = type == null ? null : type.Id.ToString();
						_FriendlyNameOutput[i] = type == null ? null : type.FriendlyTypeName;
					}
					_Invalidate = false;
				}
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
			if(!_Disposed)
			{
				SetTypes(null);
				if(_InputAttached && _PartTypesInput != null)
					_PartTypesInput.Changed -= this.PartTypes_Changed;
				StructTypeRegistry.TypeUsageCountChanged -= this.TypeRegistry_CountChanged;
				if(_ProvidedLogger)
					StructTypeRegistry.RescindLogger();
				_Disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
