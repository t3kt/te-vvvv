using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Animator.Core
{

	#region NativeMethods

	internal static class NativeMethods
	{

		private const string winmm = "winmm.dll";

		[DllImport(winmm)]
		public static extern uint timeBeginPeriod(uint uMilliseconds);

		[DllImport(winmm)]
		public static extern uint timeEndPeriod(uint uMilliseconds);

		[DllImport(winmm)]
		public static extern uint timeGetTime();

		internal delegate void TimerEventHandler(uint id, uint msg, ref UIntPtr userCtx, UIntPtr rsv1, UIntPtr rsv2);

		[DllImport(winmm, SetLastError = true)]
		public static extern uint timeSetEvent(uint msDelay, uint msResolution,
							TimerEventHandler handler, UIntPtr userCtx, fuEvent eventType);

		[DllImport(winmm, SetLastError = true)]
		public static extern uint timeKillEvent(uint timerEventId);

		//Timer type definitions
		[Flags]
		public enum fuEvent : uint
		{
			TIME_ONESHOT = 0,      //Event occurs once, after uDelay milliseconds. 
			TIME_PERIODIC = 1,
			TIME_CALLBACK_FUNCTION = 0x0000,  /* callback is function */
			//TIME_CALLBACK_EVENT_SET = 0x0010, /* callback is event - use SetEvent */
			//TIME_CALLBACK_EVENT_PULSE = 0x0020  /* callback is event - use PulseEvent */
		}

	}

	#endregion

}
