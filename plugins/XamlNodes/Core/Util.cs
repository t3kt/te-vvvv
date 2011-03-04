using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlNodes.Core
{

	#region Util

	internal static class Util
	{

		public static T[] ToArrayOrNull<T>(this IEnumerable<T> source)
		{
			return source == null ? null : source.ToArray();
		}

		public static List<T> ToListOrNull<T>(this IEnumerable<T> source)
		{
			return source == null ? null : source.ToList();
		}

		public static string OrNullIfEmpty(this string str)
		{
			return String.IsNullOrEmpty(str) ? null : str;
		}

	}

	#endregion

}
