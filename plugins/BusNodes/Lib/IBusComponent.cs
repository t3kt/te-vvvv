using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VVVV.Lib
{

	#region IBusComponent

	public interface IBusComponent : IGuidId, IDisposable
	{

	}

	#endregion

	#region IBusComponent<T>

	public interface IBusComponent<T> : IBusComponent
	{
		void AttachToPort(IBusPort<T> port);
		void DetachFromPort(IBusPort<T> port);
	}

	#endregion

}
