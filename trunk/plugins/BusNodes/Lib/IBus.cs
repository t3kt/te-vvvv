using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VVVV.Lib
{

	#region IBus<T>

	public interface IBus<T> : IDisposable
	{

		bool ContainsPort(string key);

		IPort<T> RequestPort(string key);

		void ReleasePort(string key);

	}

	#endregion

}
