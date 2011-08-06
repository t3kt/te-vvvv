using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Resources;

namespace Animator.Common
{

	#region Interval

	internal struct Interval : IComparable<Interval>, IEquatable<Interval>
	{

		#region Static / Constant

		public static bool operator ==(Interval x, Interval y)
		{
			return x._Start == y._Start &&
				   x._Duration == y._Duration;
		}

		public static bool operator !=(Interval x, Interval y)
		{
			return x._Start != y._Start ||
				   x._Duration != y._Duration;
		}

		//internal static Interval PreventOverlap(Interval target, Interval other)
		//{
		//    //#######  CASE A: other before target
		//    //        00          10          20          30          40          50
		//    //target:                                      [----------]
		//    //other:               [----------]
		//    //result:                                      [----------]
		//    //result.Start = target.Start
		//    //result.End   = target.End
		//    //result.Dur   = target.Dur
		//    if(other.End <= target.Start)
		//    {
		//        return target;
		//    }

		//    //#######  CASE B: target before other
		//    //        00          10          20          30          40          50
		//    //target:              [----------]
		//    //other:                                       [----------]
		//    //result:              [----------]
		//    //result.Start = target.Start
		//    //result.End   = target.End
		//    //result.Dur   = target.Dur
		//    if(other.Start >= target.End)
		//    {
		//        return target;
		//    }

		//    //#######  CASE C: other contains target
		//    //        00          10          20          30          40          50
		//    //target:                          [----------]
		//    //other:               [----------------------------------]
		//    //result:                                                  |
		//    //result.Start = other.End
		//    //result.End   = other.End
		//    //result.Dur   = 0
		//    //#######  CASE C2:
		//    //        00          10          20          30          40          50
		//    //target:                          [----------------------]
		//    //other:               [----------------------------------]
		//    //result:                                                  |
		//    //result.Start = other.End
		//    //result.End   = other.End
		//    //result.Dur   = 0
		//    if(other.Start < target.Start && other.End >= target.End)
		//    {
		//        return new Interval(other.End, TimeSpan.Zero);
		//    }

		//    //#######  CASE D: other overlaps start of target
		//    //        00          10          20          30          40          50
		//    //target:                          [----------------------]
		//    //other:               [----------------------]
		//    //result:                                      [----------]
		//    //result.Start = other.End
		//    //result.End   = target.End
		//    //result.Dur   = target.End - other.Start
		//    if(other.Start < target.Start && other.End < target.End)
		//    {
		//        return FromBounds(other.End, target.End);
		//    }

		//    //#######  CASE E: target contains other
		//    //        00          10          20          30          40          50
		//    //target:              [----------------------------------]
		//    //other:                           [----------]
		//    //result:              [----------]
		//    //result.Start = target.Start
		//    //result.End   = other.Start
		//    //result.Dur   = other.Start - target.Start
		//    if(other.Start > target.Start && other.End < target.End)
		//    {
		//        return FromBounds(target.Start, other.Start);
		//    }

		//    //#######  CASE F: other overlaps end of target
		//    //        00          10          20          30          40          50
		//    //target:              [----------------------------------]
		//    //other:                           [----------------------------------]
		//    //result:              [----------]
		//    //result.Start = target.Start
		//    //result.End   = other.Start
		//    //result.Dur   = other.Start - target.Start
		//    if(other.Start > target.Start && other.Start < target.Start && other.End > target.End)
		//    {
		//        return FromBounds(target.Start, other.Start);
		//    }


		//    throw new NotImplementedException();
		//}

		internal static Interval PreventOverlap(Interval target, Interval other)
		{
			//#######  CASE A: other before target
			//        00          10          20          30          40          50
			//target:                                      [----------]
			//other:               [----------]
			//result:                                      [----------]
			//result.Start = target.Start
			//result.End   = target.End
			//result.Dur   = target.Dur
			//#######  CASE B: target before other
			//        00          10          20          30          40          50
			//target:              [----------]
			//other:                                       [----------]
			//result:              [----------]
			//result.Start = target.Start
			//result.End   = target.End
			//result.Dur   = target.Dur
			if(other.End <= target.Start || other.Start >= target.End)
			{
				return target;
			}

			//#######  CASE C: other contains target
			//        00          10          20          30          40          50
			//target:                          [----------]
			//other:               [----------------------------------]
			//result:                                                  |
			//result.Start = other.End
			//result.End   = other.End
			//result.Dur   = 0
			//#######  CASE C2:
			//        00          10          20          30          40          50
			//target:                          [----------------------]
			//other:               [----------------------------------]
			//result:                                                  |
			//result.Start = other.End
			//result.End   = other.End
			//result.Dur   = 0
			if(other.Start < target.Start && other.End >= target.End)
			{
				return new Interval(other.End, TimeSpan.Zero);
			}

			//#######  CASE D: other overlaps start of target
			//        00          10          20          30          40          50
			//target:                          [----------------------]
			//other:               [----------------------]
			//result:                                      [----------]
			//result.Start = other.End
			//result.End   = target.End
			//result.Dur   = target.End - other.Start
			if(other.Start < target.Start && other.End < target.End)
			{
				return FromBounds(other.End, target.End);
			}

			//#######  CASE E: target contains other
			//        00          10          20          30          40          50
			//target:              [----------------------------------]
			//other:                           [----------]
			//result:              [----------]
			//result.Start = target.Start
			//result.End   = other.Start
			//result.Dur   = other.Start - target.Start
			if(other.Start > target.Start && other.End < target.End)
			{
				return FromBounds(target.Start, other.Start);
			}

			//#######  CASE F: other overlaps end of target
			//        00          10          20          30          40          50
			//target:              [----------------------------------]
			//other:                           [----------------------------------]
			//result:              [----------]
			//result.Start = target.Start
			//result.End   = other.Start
			//result.Dur   = other.Start - target.Start
			if(other.Start > target.Start && other.Start < target.Start && other.End > target.End)
			{
				return FromBounds(target.Start, other.Start);
			}


			throw new NotImplementedException();
		}

		internal static bool PreventOverlap(ref Interval target, Interval other)
		{
			if(!target.Overlaps(other))
				return false;



			if(other.Start < target.Start)
			{
				// is other overlapping target's start
				//       [-target-]
				//    [-other-]
				// or [----other----]
				if(other.End >= target.End)
				{
					//       [-target-]
					//    [----other----]
					target = new Interval(other.End, TimeSpan.Zero);
				}
				else
				{
					//       [-target-]
					//    [-other-]
					target = FromBounds(other.End, target.End);
				}
			}
			else
			{
				// is other overlapping target's start
				//       [---target---]
				//          [-other-]
				// or       [---other---]
				target = FromBounds(target.Start, other.Start);
			}
			return true;
		}

		internal static Interval FromBounds(TimeSpan start, TimeSpan end)
		{
			if(start > end)
				throw new ArgumentOutOfRangeException("start", String.Format(CoreStrings.Interval_StartMustNotBeAfterEnd, start, end));
			return new Interval(start, end - start);
		}

		internal static Interval FromStartAndDuration(TimeSpan start, TimeSpan duration)
		{
			return new Interval(start, duration);
		}

		internal static Interval FromEndAndDuration(TimeSpan end, TimeSpan duration)
		{
			return new Interval(end - duration, duration);
		}

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

		public Interval(double secondsStart, double secondsDuration)
			: this(TimeSpan.FromSeconds(secondsStart), TimeSpan.FromSeconds(secondsDuration))
		{
		}

		internal Interval(TimeSpan start, double secondsDuration)
			: this(start, TimeSpan.FromSeconds(secondsDuration))
		{
		}

		internal Interval(double secondsStart, TimeSpan seconds)
			: this(TimeSpan.FromSeconds(secondsStart), seconds)
		{
		}

		#endregion

		#region Methods

		public bool Overlaps(Interval other)
		{
			return CommonUtil.IsOverlap(this._Start, this.End, other._Start, other.End);
		}

		public bool Contains(Interval other)
		{
			return other._Start >= this._Start && other.End <= this.End;
		}

		public bool Contains(TimeSpan position)
		{
			return position >= this._Start && position < this.End;
		}

		public override string ToString()
		{
			//return String.Format("{0} -> {1} (dur: {2})", this._Start, this.End, this._Duration);
			return String.Format("[{0} <--- {1} ---> {2}]", this._Start, this._Duration, this.End);
		}

		public override bool Equals(object obj)
		{
			if(obj is Interval)
				return this.Equals((Interval)obj);
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			var s = this._Start.GetHashCode();
			var d = this._Duration.GetHashCode();
			return ((s << 5) + s) ^ d;
		}

		#endregion

		#region IEquatable<Interval> Members

		public bool Equals(Interval other)
		{
			return this._Start == other._Start &&
				   this._Duration == other._Duration;
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
