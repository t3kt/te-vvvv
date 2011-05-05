using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Common
{

	#region CommonUtil

	public static class CommonUtil
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

		internal static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			Require.ArgNotNull(dictionary, "dictionary");
			TValue value;
			return dictionary.TryGetValue(key, out value) ? value : default(TValue);
		}

		public static int MaxOrZero<T>([NotNull] this IEnumerable<T> source, [NotNull] Func<T, int> selector)
		{
			Require.ArgNotNull(source, "source");
			Require.ArgNotNull(selector, "selector");
			return source.Select(selector).MaxOrZero();
		}

		public static int MaxOrZero([NotNull] this IEnumerable<int> source)
		{
			Require.ArgNotNull(source, "source");
			int? max = null;
			foreach(var value in source)
			{
				max = max == null ? value : Math.Max(max.Value, value);
			}
			return max ?? 0;
		}

		public static int MaxOrZero<T>([NotNull] this IEnumerable<T> source, [NotNull] Func<T, int?> selector)
		{
			Require.ArgNotNull(source, "source");
			Require.ArgNotNull(selector, "selector");
			return source.Select(selector).MaxOrZero();
		}

		public static int MaxOrZero([NotNull] this IEnumerable<int?> source)
		{
			Require.ArgNotNull(source, "source");
			int? max = null;
			foreach(var value in source)
			{
				if(value != null)
					max = max == null ? value : Math.Max(max.Value, value.Value);
			}
			return max ?? 0;
		}

		public static void ResizeCollection<T>([NotNull] IList<T> collection, int requestedCount, [NotNull] Func<int, T> factory)
		{
			Require.ArgNotNull(collection, "collection");
			Require.ArgNotNegative(requestedCount, "requestedCount");
			Require.ArgNotNull(factory, "factory");
			if(requestedCount == 0)
			{
				collection.Clear();
			}
			else if(collection.Count > requestedCount)
			{
				while(collection.Count > requestedCount)
				{
					collection.RemoveAt(collection.Count - 1);
				}
			}
			else if(collection.Count < requestedCount)
			{
				while(collection.Count < requestedCount)
				{
					collection.Add(factory(collection.Count));
				}
			}
		}

		public static void CropList<T>([NotNull]IList<T> list, int size)
		{
			Require.ArgNotNull(list, "list");
			Require.ArgNotNegative(size, "size");
			if(size == 0)
			{
				list.Clear();
			}
			else
			{
				while(list.Count > size)
					list.RemoveAt(list.Count - 1);
			}
		}

	}

	#endregion

}
