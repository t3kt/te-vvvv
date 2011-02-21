using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region ISender<T>

	public interface ISender<T> : IGuidId, IDisposable
	{

		IDiffSpread<T> Values { get; }

	}

	#endregion

}
