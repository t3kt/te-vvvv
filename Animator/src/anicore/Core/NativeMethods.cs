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

		[DllImport(winmm_dll)]
		public static extern uint timeBeginPeriod(uint uMilliseconds);

		[DllImport(winmm_dll)]
		public static extern uint timeEndPeriod(uint uMilliseconds);

		[DllImport(winmm_dll)]
		public static extern uint timeGetTime();

	}

	#endregion

}
