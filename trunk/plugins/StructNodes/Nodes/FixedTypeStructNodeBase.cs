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

	#region FixedTypeStructNodeBase

	public abstract class FixedTypeStructNodeBase : StructNodeBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		protected readonly IDiffSpread<string> _PartTypesConfig;
		protected bool _NeedsUpdate;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		protected FixedTypeStructNodeBase(IPluginHost host, ILogger logger, IDiffSpread<string> partTypesConfig)
			: base(host, logger)
		{
			_PartTypesConfig = partTypesConfig;
			_PartTypesConfig.Changed += this.PartTypes_Changed;
		}

		#endregion

		#region Methods

		protected virtual void PartTypes_Changed(IDiffSpread<string> partTypes)
		{
			CheckDisposed();
			SetType(StructTypeDefinition.FilterPartTypesKey(partTypes[0]));
			_NeedsUpdate = true;
		}

		protected override void Dispose()
		{
			if(_PartTypesConfig != null)
				_PartTypesConfig.Changed -= this.PartTypes_Changed;
			base.Dispose();
		}

		#endregion

	}

	#endregion

}
