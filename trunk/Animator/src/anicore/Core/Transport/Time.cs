using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Transport
{

	#region Time

	public struct Time
	{

		#region Static / Constant

		internal static long BeatsToTicks(float beats, float beatsPerMinute)
		{
			return (long)(beats * beatsPerMinute * TimeSpan.TicksPerMinute);
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

		public TimeSpan ToTimeSpan(ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			return TimeSpan.FromTicks(BeatsToTicks(_Beats, transport.BeatsPerMinute));
		}

		#endregion

	}

	#endregion

}
