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

	#region DefaultStructNode

	[PluginInfo(Name = "Default", Category = "Struct")]
	public class DefaultStructNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly IPluginHost _Host;
		private StructTypeDefinition _Type;

		//[Config("PartTypes", IsSingle = true)]
		private readonly IDiffSpread<string> _PartTypesConfig;

		[Output("OutputStruct", IsSingle = true)]
		protected ISpread<StructData> _StructOutput;

		[Output("StructDisplayString", IsSingle = true)]
		protected ISpread<string> _StructDisplayStringOutput;

		[Output("ActualPartTypes", IsSingle = true)]
		protected ISpread<string> _ActualPartTypesOutput;

		private bool _NeedsUpdate;
		private bool _Disposed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public DefaultStructNode(IPluginHost host, [Config("PartTypes", IsSingle = true)]IDiffSpread<string> partTypes)
		{
			_Host = host;
			_PartTypesConfig = partTypes;
			_PartTypesConfig.Changed += this.PartTypes_Changed;
			_NeedsUpdate = true;
			StructTypeRegistry.OfferHost(host);
		}

		#endregion

		#region Methods

		private void CheckDisposed()
		{
			if(_Disposed)
				throw new ObjectDisposedException(GetType().FullName);
		}

		private void PartTypes_Changed(IDiffSpread<string> partTypes)
		{
			CheckDisposed();
			var partTypesKey = StructTypeDefinition.FilterPartTypesKey(partTypes[0]);
			if(_Type == null || _Type.PartTypesKey != partTypesKey)
			{
				StructTypeRegistry.ReleaseTypeDefinition(_Type);
				_Type = StructTypeRegistry.RequestTypeDefinition(partTypesKey);
				UpdateOutputs();
			}
		}

		private void UpdateOutputs()
		{
			CheckDisposed();
			_StructOutput.SliceCount = 1;
			_StructDisplayStringOutput.SliceCount = 1;
			_ActualPartTypesOutput.SliceCount = 1;
			if(_Type == null)
			{
				//_StructOutput.SliceCount = 0;
				_StructOutput[0] = null;
				_StructDisplayStringOutput[0] = null;
				_ActualPartTypesOutput[0] = null;
			}
			else
			{
				var data = _Type.CreateInstance();
				_StructOutput[0] = data;
				_StructDisplayStringOutput[0] = data.ToString();
				_ActualPartTypesOutput[0] = _Type.PartTypesKey;
			}
			_NeedsUpdate = false;
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			CheckDisposed();
			if(_NeedsUpdate)
			{
				UpdateOutputs();
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(!_Disposed)
			{
				if(_PartTypesConfig != null)
					_PartTypesConfig.Changed -= this.PartTypes_Changed;
				StructTypeRegistry.ReleaseTypeDefinition(_Type);
				_Disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
