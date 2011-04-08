using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Common.Diagnostics;

namespace Animator.Common
{

	#region CommonUtil

	internal static class CommonUtil
	{

		internal static IDisposable SuspendNotifyScope(this ISuspendableNotify target)
		{
			if(target == null)
				return ActionScope.Null;
			target.SuspendNotify();
			return new ActionScope(target.ResumeNotify);
		}

		internal static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			Require.ArgNotNull(collection, "collection");
			Require.ArgNotNull(items, "items");
			foreach(var item in items)
				collection.Add(item);
		}

	}

	#endregion

}
