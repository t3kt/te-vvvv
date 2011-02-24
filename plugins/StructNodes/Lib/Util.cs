using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

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

		internal static void ClearPins<TPin>(this IPluginHost host, IList<TPin> pins)
			where TPin : IPluginIO
		{
			if(pins != null)
			{
				foreach(var pin in pins)
					host.DeletePin(pin);
				pins.Clear();
			}
		}

		internal static void SetSliceCounts<TPin>(this IEnumerable<TPin> pins, int sliceCount)
			where TPin : class, IPluginOut
		{
			if(pins != null)
			{
				foreach(var pin in pins)
					pin.SliceCount = sliceCount;
			}
		}

		internal static T[] ToArray<T>(this ISpread<T> spread)
		{
			if(spread == null)
				throw new ArgumentNullException("spread");
			var array = new T[spread.SliceCount];
			for(var i = 0; i < array.Length; i++)
				array[i] = spread[i];
			return array;
		}

	}

	#endregion

}
