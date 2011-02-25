using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region StructNodeBase

	public abstract class StructNodeBase : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		protected readonly IPluginHost _Host;
		private StructTypeDefinition _Type;
		private bool _Disposed;
		private readonly bool _ProvidedLogger;

		#endregion

		#region Properties

		protected StructTypeDefinition Type
		{
			get { return _Type; }
		}

		#endregion

		#region Constructors

		protected StructNodeBase(IPluginHost host, ILogger logger)
		{
			_Host = host;
			_ProvidedLogger = StructTypeRegistry.OfferLogger(logger);
		}

		#endregion

		#region Methods

		protected void CheckDisposed()
		{
			if(_Disposed)
				throw new ObjectDisposedException(GetType().FullName);
		}

		protected void SetType(string partTypesKey)
		{
			if((_Type == null && !String.IsNullOrEmpty(partTypesKey)) ||
				(_Type != null && partTypesKey != _Type.PartTypesKey))
			{
				StructTypeRegistry.ReleaseTypeDefinition(_Type);
				_Type = StructTypeRegistry.RequestTypeDefinition(partTypesKey);
				OnTypeChanged();
			}
		}

		protected virtual void OnTypeChanged() { }

		#endregion

		#region IPluginEvaluate Members

		public abstract void Evaluate(int spreadMax);

		#endregion

		#region IDisposable Members

		protected virtual void Dispose()
		{
			SetType(null);
		}

		void IDisposable.Dispose()
		{
			if(!_Disposed)
			{
				Dispose();
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
