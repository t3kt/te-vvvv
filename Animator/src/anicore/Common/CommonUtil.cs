using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Common
{

	#region CommonUtil

	public static class CommonUtil
	{

		internal static void AddRange<T>([NotNull] this ICollection<T> collection, [NotNull] IEnumerable<T> items)
		{
			Require.ArgNotNull(collection, "collection");
			Require.ArgNotNull(items, "items");
			foreach(var item in items)
				collection.Add(item);
		}

		internal static void ReplaceContents<T>([NotNull] this ICollection<T> collection, [NotNull] IEnumerable<T> items)
		{
			Require.ArgNotNull(collection, "collection");
			Require.ArgNotNull(items, "items");
			collection.Clear();
			collection.AddRange(items);
		}

		public static void PushAll<T>([NotNull]Stack<T> stack, [NotNull]IEnumerable<T> items)
		{
			Require.ArgNotNull(stack, "stack");
			Require.ArgNotNull(items, "items");
			foreach(var item in items)
				stack.Push(item);
		}

		public static bool TryPop<T>([NotNull]Stack<T> stack, out T item)
		{
			Require.ArgNotNull(stack, "stack");
			if(stack.Count == 0)
			{
				item = default(T);
				return false;
			}
			item = stack.Pop();
			return true;
		}

		[NotNull]
		public static List<T> ClearStackAtTarget<T>([NotNull] Stack<T> stack, T target, IEqualityComparer<T> comparer = null)
		{
			Require.ArgNotNull(stack, "stack");
			if(comparer == null)
				comparer = EqualityComparer<T>.Default;
			var keepStack = new Stack<T>(stack.Count);
			var removedList = new List<T>(stack.Count);
			T item;
			while(stack.Count > 0)
			{
				item = stack.Pop();
				if(comparer.Equals(item, target))
				{
					removedList.Add(item);
					while(stack.Count > 0)
					{
						item = stack.Pop();
						removedList.Add(item);
					}
					break;
				}
				keepStack.Push(item);
			}
			while(keepStack.Count > 0)
			{
				item = keepStack.Pop();
				stack.Push(item);
			}
			return removedList;
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

		internal static float MapFloating(float input, float inMinimum, float inMaximum, float outMinimum, float outMaximum)
		{
			if(inMaximum - inMinimum == 0)
				return 0f;
			var range = inMaximum - inMinimum;
			var normalized = (input - inMinimum) / range;
			return outMinimum + normalized * (outMaximum - outMinimum);
		}

		internal static float MapClamped(float input, float inMinimum, float inMaximum, float outMinimum, float outMaximum)
		{
			if(inMaximum - inMinimum == 0)
				return 0;
			var range = inMaximum - inMinimum;
			var normalized = (input - inMinimum) / range;
			var output = outMinimum + normalized * (outMaximum - outMinimum);
			var min = Math.Min(outMinimum, outMaximum);
			var max = Math.Max(outMinimum, outMaximum);
			return Math.Min(Math.Max(output, min), max);
		}

		internal static float MapWrapping(float input, float inMinimum, float inMaximum, float outMinimum, float outMaximum)
		{
			if(inMaximum - inMinimum == 0)
				return 0;
			var range = inMaximum - inMinimum;
			var normalized = (input - inMinimum) / range;

			if(normalized < 0)
				normalized = 1 + normalized;
			return outMinimum + (normalized % 1) * (outMaximum - outMinimum);
		}

		internal static float Clamp(float input, float min, float max)
		{
			var minTemp = Math.Min(min, max);
			var maxTemp = Math.Max(min, max);
			return Math.Min(Math.Max(input, minTemp), maxTemp);
		}

		internal static int Clamp(int input, int min, int max)
		{
			var minTemp = Math.Min(min, max);
			var maxTemp = Math.Max(min, max);
			return Math.Min(Math.Max(input, minTemp), maxTemp);
		}

		internal static TimeSpan Min(TimeSpan x, TimeSpan y)
		{
			return x < y ? x : y;
		}

		internal static TimeSpan Max(TimeSpan x, TimeSpan y)
		{
			return x > y ? x : y;
		}

		internal static TimeSpan Clamp(TimeSpan input, TimeSpan min, TimeSpan max)
		{
			var minTemp = Min(min, max);
			var maxTemp = Max(min, max);
			return Min(Max(input, minTemp), maxTemp);
		}

		internal static bool IsOverlap(TimeSpan aStart, TimeSpan aEnd, TimeSpan bStart, TimeSpan bEnd)
		{
			if(bEnd < aStart)
				return false;
			if(bStart > aEnd)
				return false;
			return true;
		}

		internal const float ValueComparisonEpsilon = 0.0001f;

		public static bool AreValuesClose(float x, float y)
		{
			return Math.Abs(x - y) > ValueComparisonEpsilon;
		}

	}

	#endregion

}
