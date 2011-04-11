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

		private const string winmm_dll = "winmm.dll";
		private const string kernel32_dll = "kernel32.dll";

		[DllImport(winmm_dll)]
		public static extern uint timeBeginPeriod(uint uMilliseconds);

		[DllImport(winmm_dll)]
		public static extern uint timeEndPeriod(uint uMilliseconds);

		[DllImport(winmm_dll)]
		public static extern uint timeGetTime();

		public delegate void TIMECALLBACK(uint id, uint msg, ref UIntPtr userCtx, UIntPtr rsv1, UIntPtr rsv2);

		[DllImport(winmm_dll, SetLastError = true)]
		public static extern uint timeSetEvent(uint msDelay, uint msResolution,
							TIMECALLBACK handler, UIntPtr userCtx, fuEvent eventType);

		[DllImport(winmm_dll, SetLastError = true)]
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

		[DllImport(kernel32_dll)]
		public static extern bool CreateTimerQueueTimer(out IntPtr phNewTimer,
			   IntPtr TimerQueue, WaitOrTimerDelegate Callback, IntPtr Parameter,
			   uint DueTime, uint Period, uint Flags);

		public delegate void WaitOrTimerDelegate(IntPtr lpParameter, bool TimerOrWaitFired);

		public enum ExecuteFlags
		{

			/// <summary>
			/// By default, the callback function is queued to a non-I/O worker thread.
			/// </summary>
			WT_EXECUTEDEFAULT = 0x00000000,
			/// <summary>
			/// The callback function is invoked by the timer thread itself. This flag should be used only for short tasks or it could affect other timer operations. 
			/// The callback function is queued as an APC. It should not perform alertable wait operations.
			/// </summary>
			WT_EXECUTEINTIMERTHREAD = 0x00000020,
			/// <summary>
			/// The callback function is queued to an I/O worker thread. This flag should be used if the function should be executed in a thread that waits in an alertable state. 
			/// The callback function is queued as an APC. Be sure to address reentrancy issues if the function performs an alertable wait operation.
			/// </summary>
			WT_EXECUTEINIOTHREAD = 0x00000001,

			/// <summary>
			/// The callback function is queued to a thread that never terminates. It does not guarantee that the same thread is used each time. This flag should be used only for short tasks or it could affect other timer operations. 
			/// Note that currently no worker thread is truly persistent, although no worker thread will terminate if there are any pending I/O requests.
			/// </summary>
			WT_EXECUTEINPERSISTENTTHREAD = 0x00000080,

			/// <summary>
			/// The callback function can perform a long wait. This flag helps the system to decide if it should create a new thread.
			/// </summary>
			WT_EXECUTELONGFUNCTION = 0x00000010,

			/// <summary>
			/// The timer will be set to the signaled state only once. If this flag is set, the Period parameter must be zero.
			/// </summary>
			WT_EXECUTEONLYONCE = 0x00000008,
			/// <summary>
			/// Callback functions will use the current access token, whether it is a process or impersonation token. If this flag is not specified, callback functions execute only with the process token.
			/// Windows XP/2000:  This flag is not supported until Windows XP with SP2 and Windows Server 2003.
			/// </summary>
			WT_TRANSFER_IMPERSONATION = 0x00000100
		};

		[DllImport(kernel32_dll)]
		public static extern IntPtr CreateTimerQueue();

	}

	#endregion

}
