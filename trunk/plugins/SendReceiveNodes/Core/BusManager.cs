using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendReceiveNodes.Core
{

	#region BusManager

	internal class BusManager
	{

		private static class CoreHolder<T>
		{
			public static readonly BusCore<T> Instance;

			static CoreHolder()
			{
				Instance = new BusCore<T>();
			}

		}

		public static BusCore<T> GetCore<T>()
		{
			return CoreHolder<T>.Instance;
		}

		public static IEnumerable<string> GetPortKeys<T>()
		{
			throw new NotImplementedException();
		}

	}

	#endregion

}
