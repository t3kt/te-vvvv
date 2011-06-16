using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Resources;

namespace Animator.Common
{

	#region Interval

	internal struct Interval : IComparable<Interval>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly TimeSpan _Start;
		private readonly TimeSpan _Duration;

		#endregion

		#region Properties

		public TimeSpan Start
		{
			get { return this._Start; }
		}

		public TimeSpan Duration
		{
			get { return this._Duration; }
		}

		public TimeSpan End
		{
			get { return this._Start + this._Duration; }
		}

		#endregion

		#region Constructors

		public Interval(TimeSpan start, TimeSpan duration)
		{
			if(duration < TimeSpan.Zero)
				throw new ArgumentOutOfRangeException("duration", CoreStrings.DurationMustNotBeNegative);
			this._Start = start;
			this._Duration = duration;
		}

		#endregion

		#region Methods

		public bool Overlaps(Interval other)
		{
			return CommonUtil.IsOverlap(this._Start, this.End, other._Start, other.End);
		}

		public override string ToString()
		{
			return String.Format("{0} -> {1} (dur: {2})", this._Start, this.End, this._Duration);
		}

		#endregion

		#region IComparable<Interval> Members

		public int CompareTo(Interval other)
		{
			return this._Start.CompareTo(other._Start);
		}

		#endregion

	}

	#endregion

}
