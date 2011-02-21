using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region IBusSender<T>

	public interface IBusSender<T> : IBusComponent<T>
	{

		IDiffSpread<T> Values { get; }
		IDiffSpread<T> DefaultValues { get; }

		event BusSenderEventHandler<T> Changed;

	}

	#endregion

}
