using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CommandNodes
{

	#region NativeMethods

	[TESharedAnnotations.Incomplete]
	internal static class NativeMethods
	{

		#region Nested Types

		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		public struct KEYBDINPUT
		{
			public short wVk;
			public short wScan;
			public int dwFlags;
			public int time;
			public IntPtr dwExtraInfo;
		}

		public struct MOUSEINPUT
		{
			public int dx;
			public int dy;
			public int mouseData;
			public int dwFlags;
			public int time;
			public IntPtr dwExtraInfo;
		}

		public struct HARDWAREINPUT
		{
			public int uMsg;
			public short wParamL;
			public short wParamH;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct INPUTUNION
		{
			[FieldOffset(0)]
			public HARDWAREINPUT hi;

			[FieldOffset(0)]
			public KEYBDINPUT ki;

			[FieldOffset(0)]
			public MOUSEINPUT mi;
		}

		public struct INPUT
		{
			public int type;
			public INPUTUNION inputUnion;
		}

		private enum HookType
		{
			WH_JOURNALRECORD = 0,
			WH_JOURNALPLAYBACK = 1,
			WH_KEYBOARD = 2,
			WH_GETMESSAGE = 3,
			WH_CALLWNDPROC = 4,
			WH_CBT = 5,
			WH_SYSMSGFILTER = 6,
			WH_MOUSE = 7,
			WH_HARDWARE = 8,
			WH_DEBUG = 9,
			WH_SHELL = 10,
			WH_FOREGROUNDIDLE = 11,
			WH_CALLWNDPROCRET = 12,
			WH_KEYBOARD_LL = 13,
			WH_MOUSE_LL = 14
		}

		public const int WH_KEYBOARD = 2;
		public const int WH_MOUSE = 7;
		public const int WH_HARDWARE = 8;
		public const int WH_KEYBOARD_LL = 13;
		public const int WH_MOUSE_LL = 14;

		public const int WM_KEYDOWN = 0x0100;
		public const int WM_SYSKEYDOWN = 0x104;
		public const int WM_SYSKEYUP = 0x105;
		public const int WM_KEYUP = 0x101;

		public const byte VK_CAPITAL = 0x14;
		public const byte VK_SHIFT = 0x10;

		public class KBDLLHOOKSTRUCT
		{
			public uint vkCode;
			public uint scanCode;
			public KBDLLHOOKSTRUCTFlags flags;
			public uint time;
			public UIntPtr dwExtraInfo;
		}

		[Flags]
		public enum KBDLLHOOKSTRUCTFlags : uint
		{
			LLKHF_EXTENDED = 0x01,
			LLKHF_INJECTED = 0x10,
			LLKHF_ALTDOWN = 0x20,
			LLKHF_UP = 0x80,
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MSLLHOOKSTRUCT
		{
			public POINT pt;
			public uint mouseData;
			public uint flags;
			public uint time;
			public UIntPtr dwExtraInfo;
		}

		#endregion

		private const string User32Dll = "user32.dll";
		private const string Kernel32Dll = "kernel32.dll";

		public static readonly HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		[DllImport(User32Dll, CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowsHookEx(int hookType, HookProc lpfn, HandleRef hMod, int dwThreadId);

		[DllImport(User32Dll, CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowsHookEx(int hookType, HookProc lpfn, IntPtr hMod, int dwThreadId);

		[DllImport(User32Dll, CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool UnhookWindowsHookEx(HandleRef hhook);

		[DllImport(Kernel32Dll, CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string modName);



		[DllImport(User32Dll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport(User32Dll)]
		public static extern int GetDoubleClickTime();

		[DllImport(User32Dll)]
		public static extern short GetKeyboardState(byte[] pbKeyState);

		[DllImport(User32Dll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern short GetKeyState(int vKey);

		[DllImport(User32Dll)]
		public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

		[DllImport(Kernel32Dll)]
		public static extern int GetCurrentThreadId();






		private static IntPtr SetHook(int hookType, HookProc proc)
		{
			using(var process = Process.GetCurrentProcess())
			using(var module = process.MainModule)
				return SetWindowsHookEx(hookType, proc, GetModuleHandle(module.ModuleName), 0);
		}

	}

	#endregion

}
