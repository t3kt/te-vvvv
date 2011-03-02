using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region DefaultStructNode

	// this works
	[PluginInfo(Name = Names.Nodes.Default,
		Category = Names.Category,
		Author = TEShared.Names.Author)]
	public sealed class DefaultStructNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private StructTypeDefinition _Type;

		private readonly IDiffSpread<string> _PartTypesConfig;

		[Output("OutputStruct", IsSingle = true)]
		private ISpread<StructData> _StructOutput;

		[Output("StructDisplayString", IsSingle = true)]
		private ISpread<string> _StructDisplayStringOutput;

		[Output("ActualPartTypes", IsSingle = true)]
		private ISpread<string> _ActualPartTypesOutput;

		private bool _Disposed;
		private readonly bool _ProvidedLogger;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public DefaultStructNode(IPluginHost host, [Import] ILogger logger, [Config("PartTypes", IsSingle = true)]IDiffSpread<string> partTypes)
		{
			_PartTypesConfig = partTypes;
			_PartTypesConfig.Changed += this.PartTypes_Changed;
			_ProvidedLogger = StructTypeRegistry.OfferLogger(logger);
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
			_StructOutput.SliceCount = _StructDisplayStringOutput.SliceCount = _ActualPartTypesOutput.SliceCount = 1;
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
		}

		#endregion

		#region IPluginEvaluate Members

		void IPluginEvaluate.Evaluate(int spreadMax)
		{
			//CheckDisposed();
			//if(_NeedsUpdate)
			//{
			//    UpdateOutputs();
			//}
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if(!_Disposed)
			{
				if(_PartTypesConfig != null)
					_PartTypesConfig.Changed -= this.PartTypes_Changed;
				StructTypeRegistry.ReleaseTypeDefinition(_Type);
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
