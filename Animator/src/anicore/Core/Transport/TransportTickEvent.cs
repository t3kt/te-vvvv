using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region TransportTickEventArgs

	public sealed class TransportTickEventArgs : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly TimeSpan _Interval;

		#endregion

		#region Properties

		public TimeSpan Interval
		{
			get { return this._Interval; }
		}

		#endregion

		#region Constructors

		public TransportTickEventArgs(TimeSpan interval)
		{
			this._Interval = interval;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
