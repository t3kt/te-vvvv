using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace TE.VVVV.Plugins.Common
{

	#region SpreadUtil

	public static class SpreadUtil
	{

		public static T GetItemWrapped<T>(this ISpread<T> spread, int index)
		{
			if(spread == null)
				throw new ArgumentNullException("spread");
			return spread[Math.Abs(index)%spread.SliceCount];
		}

	}

	#endregion

}
