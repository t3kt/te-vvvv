using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Windows.Data;
using Animator.Common.Diagnostics;
using Animator.Resources;

namespace Animator.Core.Transport
{

	#region Time

	[TypeConverter(typeof(TimeConverter))]
	public struct Time : IEquatable<Time>, IComparable<Time>, IComparable
	{

		#region Static / Constant

		public static implicit operator Time(float beats)
		{
			return new Time(beats);
		}

		public static explicit operator float(Time time)
		{
			return time._Beats;
		}

		public static bool operator ==(Time x, Time y)
		{
			return x._Beats == y._Beats;
		}

		public static bool operator !=(Time x, Time y)
		{
			return x._Beats != y._Beats;
		}

		public static Time operator +(Time x, Time y)
		{
			return new Time(x._Beats + y._Beats);
		}

		public static Time operator -(Time x, Time y)
		{
			return new Time(x._Beats - y._Beats);
		}

		public static bool operator <(Time x, Time y)
		{
			return x._Beats < y._Beats;
		}

		public static bool operator >(Time x, Time y)
		{
			return x._Beats > y._Beats;
		}

		public static Time operator %(Time x, Time y)
		{
			return x._Beats % y._Beats;
		}

		public static Time operator /(Time x, float y)
		{
			return new Time(x._Beats / y);
		}

		public static Time operator *(Time x, float y)
		{
			return new Time(x._Beats * y);
		}

		public static Time operator /(Time x, Time y)
		{
			return new Time(x._Beats / y._Beats);
		}

		public static Time operator *(Time x, Time y)
		{
			return new Time(x._Beats * y._Beats);
		}

		public static readonly Time Infinite = new Time(Single.PositiveInfinity);

		/*	beats * bpm * ticksPerMin = ticks
			beats * bpm = ticks / ticksPerMin
			beats = ticks / (bpm * ticksPerMin)
		*/

		internal static long BeatsToTicks(float beats, float bpm)
		{
			return (long)(beats * bpm * TimeSpan.TicksPerMinute);
		}

		internal static float TicksToBeats(long ticks, float bpm)
		{
			Require.ArgNotZero(ticks, "ticks");
			Require.ArgNotZero(bpm, "bpm");
			return ticks / (bpm * TimeSpan.TicksPerMinute);
		}

		public static bool TryParse(string value, out Time time)
		{
			if(!String.IsNullOrEmpty(value))
			{
				if(String.Equals(value.Trim(), "inf", StringComparison.OrdinalIgnoreCase) ||
					String.Equals(value.Trim(), "infinite", StringComparison.OrdinalIgnoreCase))
				{
					time = Infinite;
					return true;
				}
				float f;
				if(Single.TryParse(value, out f))
				{
					time = new Time(f);
					return true;
				}
			}
			time = default(Time);
			return false;
		}

		public static Time Parse(string value)
		{
			Time time;
			if(!TryParse(value, out time))
				throw new ArgumentException(String.Format(CoreStrings.Time_CannotParseString, value), "value");
			return time;
		}

		public static Time FromTicks(long ticks, float bpm)
		{
			return new Time(TicksToBeats(ticks, bpm));
		}

		#endregion

		#region Fields

		private readonly float _Beats;

		#endregion

		#region Properties

		public float Beats
		{
			get { return _Beats; }
		}

		public int WholeBeats
		{
			get { return (int)_Beats; }
		}

		public float PartialBeat
		{
			get { return _Beats % 1; }
		}

		#endregion

		#region Constructors

		public Time(float beats)
		{
			this._Beats = beats;
		}

		#endregion

		#region Methods

		public TimeSpan ToTimeSpan(float bpm)
		{
			return TimeSpan.FromTicks(BeatsToTicks(_Beats, bpm));
		}

		public override string ToString()
		{
			if(this == Infinite)
				return "Infinite";
			return _Beats.ToString();
		}

		public override bool Equals(object obj)
		{
			return obj is Time && Equals((Time)obj);
		}

		public override int GetHashCode()
		{
			return _Beats.GetHashCode();
		}

		#endregion

		#region IEquatable<Time> Members

		public bool Equals(Time other)
		{
			return this._Beats == other._Beats;
		}

		#endregion

		#region IComparable<Time> Members

		public int CompareTo(Time other)
		{
			return this._Beats.CompareTo(other._Beats);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if(obj == null)
				return 1;
			if(obj is Time)
				return CompareTo((Time)obj);
			return this._Beats.CompareTo(obj);
		}

		#endregion

	}

	#endregion

	//#region TimeValueConverter

	//[ValueConversion(typeof(Time), typeof(string))]
	//internal class TimeValueConverter : IValueConverter
	//{

	//    #region Static/Constant

	//    #endregion

	//    #region Fields

	//    #endregion

	//    #region Properties

	//    #endregion

	//    #region Constructors

	//    #endregion

	//    #region Methods

	//    #endregion

	//    #region IValueConverter Members

	//    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	//    {
	//        var time = (Time)value;
	//        return time.ToString();
	//    }

	//    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	//    {
	//        if(value == null)
	//            return null;
	//        var str = value.ToString();
	//        float f;
	//        if(Single.TryParse(str, out f))
	//            return new Time(f);
	//        return value;
	//    }

	//    #endregion
	//}

	//#endregion

	#region TimeConverter

	internal class TimeConverter : TypeConverter
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(float);
		}

		//public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		//{
		//    return destinationType == typeof(InstanceDescriptor) || base.CanConvertFrom(context, destinationType);
		//}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || destinationType == typeof(float) ||
				base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if(value == null)
				throw this.GetConvertFromException(value);
			if(value is string)
			{
				return Time.Parse((string)value);
			}
			if(value is float)
			{
				return new Time((float)value);
			}
			throw new ArgumentException("Cannot parse value as Time", "value");
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if(value is Time)
			{
				if(destinationType == typeof(string))
					return value.ToString();
				if(destinationType == typeof(float))
					return (float)((Time)value);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion

	}

	#endregion

}
