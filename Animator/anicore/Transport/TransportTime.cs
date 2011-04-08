using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Core.Transport
{
	/*
	 		|<------------ Block ------------>|
	 		|<---- Bar ----->|
	 		|<-Beat->|
	 		([ . . . | . . . ][ . . . | . . . ]) ([ . . . | . . . ][ . . . | . . . ])
	 */

	#region TransportTime

	public struct TransportTime
	{

		#region Static / Constant

		internal static long BeatsToTicks(float beats, int beatsPerMinute)
		{
			return (long)(beatsPerMinute * beats * TimeSpan.TicksPerMinute);
		}

		public static readonly TransportTime Zero = new TransportTime(0);

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

		public float PartialBeats
		{
			get { return _Beats % 1; }
		}

		#endregion

		#region Constructors

		public TransportTime(float beats)
		{
			_Beats = beats;
		}

		#endregion

		#region Methods

		public TimeSpan ToTimeSpan(ITransportContext context)
		{
			if(context == null)
				throw new ArgumentNullException("context");
			return TimeSpan.FromTicks(BeatsToTicks(_Beats, context.BeatsPerMinute));
		}

		#endregion

	}

	#endregion

}
