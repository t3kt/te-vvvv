using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region StructNodeBaseV2

	public abstract class StructNodeBaseV2 : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		protected readonly IPluginHost _Host;
		private StructTypeDefinition _Type;
		private bool _Disposed;

		#endregion

		#region Properties

		protected StructTypeDefinition Type
		{
			get { return _Type; }
		}

		#endregion

		#region Constructors

		protected StructNodeBaseV2(IPluginHost host)
		{
			_Host = host;
			StructTypeRegistry.OfferHost(host);
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
			}
		}

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
				_Disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
