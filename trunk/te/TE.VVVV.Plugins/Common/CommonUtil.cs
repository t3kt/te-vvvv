using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VVVV.PluginInterfaces.V1;

namespace TE.VVVV.Plugins.Common
{

	#region CommonUtil

	public static class CommonUtil
	{

		//public static IStringIn CreateNewStringInput(this IPluginHost host, string name, TSliceMode sliceMode = TSliceMode.Dynamic, TPinVisibility visibility = TPinVisibility.True, string defaultValue = "", bool isFileName = false, IPinUpdater updater = null)
		//{
		//    if(host == null)
		//        return null;
		//    IStringIn pin;
		//    host.CreateStringInput(name, sliceMode, visibility, out pin);
		//    pin.SetSubType(defaultValue, isFileName);
		//    if(updater != null)
		//        pin.SetPinUpdater(updater);
		//    return pin;
		//}

		//public static IStringOut CreateNewStringOutput(this IPluginHost host, string name, TSliceMode sliceMode = TSliceMode.Dynamic, TPinVisibility visibility = TPinVisibility.True, string defaultValue = "", bool isFileName = false, IPinUpdater updater = null)
		//{
		//    if(host == null)
		//        return null;
		//    IStringOut pin;
		//    host.CreateStringOutput(name, sliceMode, visibility, out pin);
		//    pin.SetSubType(defaultValue, isFileName);
		//    if(updater != null)
		//        pin.SetPinUpdater(updater);
		//    return pin;
		//}

		//public static IEnumIn CreateNewEnumInput(this IPluginHost host, string name, TSliceMode sliceMode = TSliceMode.Dynamic, TPinVisibility visibility = TPinVisibility.True, string enumName = null, IPinUpdater updater = null)
		//{
		//    if(host == null)
		//        return null;
		//    IEnumIn pin;
		//    host.CreateEnumInput(name, sliceMode, visibility, out pin);
		//    if(enumName != null)
		//        pin.SetSubType(enumName);
		//    if(updater != null)
		//        pin.SetPinUpdater(updater);
		//    return pin;
		//}

		//public static IEnumIn CreateNewEnumInput<TEnum>(this IPluginHost host, string name, TSliceMode sliceMode = TSliceMode.Dynamic, TPinVisibility visibility = TPinVisibility.True, IPinUpdater updater = null)
		//    where TEnum : struct
		//{
		//    return CreateNewEnumInput(host, name, sliceMode, visibility, typeof(TEnum).Name, updater);
		//}

		//public static void RegisterEnum<TEnum>(this IPluginHost host, string name = null, TEnum defaultValue = default(TEnum))
		public static void RegisterEnum<TEnum>(this IPluginHost host, string name, TEnum defaultValue)
			where TEnum : struct
		{
			var type = typeof(TEnum);
			if(!type.IsEnum)
				throw new InvalidOperationException();
			var values = (TEnum[])Enum.GetValues(type);
			host.UpdateEnum(name ?? type.Name, defaultValue.ToString(),
				values.Select(v => v.ToString()).ToArray());
		}

		public static string[] GetAllStrings(this IStringIn pin)
		{
			if(pin == null)
				return null;
			if(pin.SliceCount == 0)
				return new string[0];
			var vals = new string[pin.SliceCount];
			for(var i = 0; i < pin.SliceCount; i++)
			{
				string val;
				pin.GetString(i, out val);
				vals[i] = val;
			}
			return vals;
		}

		public static void SetAllStrings(this IStringOut pin, IList<string> vals)
		{
			if(pin == null)
				return;
			if(vals == null || vals.Count == 0)
			{
				pin.SliceCount = 0;
			}
			else
			{
				pin.SliceCount = vals.Count;
				for(var i = 0; i < vals.Count; i++)
					pin.SetString(i, vals[i]);
			}
		}

	}

	#endregion

}
