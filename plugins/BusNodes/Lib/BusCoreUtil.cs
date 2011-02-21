using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VVVV.Lib
{

	#region BusCoreUtil

	internal static class BusCoreUtil
	{

		public static IEqualityComparer<string> KeyComparer
		{
			get { return StringComparer.OrdinalIgnoreCase; }
		}

	}

	#endregion

}
