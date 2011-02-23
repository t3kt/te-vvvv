using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Lib
{

	#region Util

	internal static class Util
	{

		internal static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if(dictionary == null)
				throw new ArgumentNullException("dictionary");
			TValue value;
			return dictionary.TryGetValue(key, out value) ? value : default(TValue);
		}

		internal static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			if(dictionary == null)
				throw new ArgumentNullException("dictionary");
			TValue value;
			return dictionary.TryGetValue(key, out value) ? value : defaultValue;
		}

	}

	#endregion

}
