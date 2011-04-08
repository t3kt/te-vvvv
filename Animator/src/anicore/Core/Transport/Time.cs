using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Transport
{

	#region Time

	public struct Time : IEquatable<Time>
	{

		#region Static / Constant

		internal static long BeatsToTicks(float beats, float beatsPerMinute)
		{
			return (long)(beats * beatsPerMinute * TimeSpan.TicksPerMinute);
		}

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

		public static readonly Time Infinite = new Time(Single.PositiveInfinity);

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

		public TimeSpan ToTimeSpan(ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			return TimeSpan.FromTicks(BeatsToTicks(_Beats, transport.BeatsPerMinute));
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

	}

	#endregion

}
