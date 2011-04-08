using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Transport
{

	#region TriggerBehavior

	[Unused]
	internal struct TriggerBehavior
	{

		#region TriggerBehaviorType

		private enum TriggerBehaviorType
		{
			Automatic,
			Aligned,
			Immediate
		}

		#endregion

		#region Static / Constant

		public static readonly TriggerBehavior Automatic = new TriggerBehavior(TriggerBehaviorType.Automatic);
		public static readonly TriggerBehavior Immediate = new TriggerBehavior(TriggerBehaviorType.Immediate);

		#endregion

		#region Fields

		private readonly TriggerBehaviorType _Type;
		private readonly int _BeatAlignment;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		private TriggerBehavior(TriggerBehaviorType type)
		{
			this._Type = type;
			this._BeatAlignment = -1;
		}

		public TriggerBehavior(int beatAlignment)
		{
			Require.ArgNotNegative(beatAlignment, "beatAlignment");
			this._Type = TriggerBehaviorType.Aligned;
			this._BeatAlignment = beatAlignment;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
