using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace CommandNodes
{

	#region HookManager

	[TESharedAnnotations.Incomplete]
	internal static class HookManager
	{

		#region Nested Types

		#endregion

		private static readonly object _InstallLock = new object();

		private static IntPtr _KeyboardHookHandle;
		private static NativeMethods.HookProc _KeyboardHookProc;

		private static IntPtr _MouseHookHandle;
		private static NativeMethods.HookProc _MouseHookProc;

		private static void InstallMouseHook()
		{
			lock(_InstallLock)
			{
				if(_MouseHookHandle == IntPtr.Zero)
				{
					_MouseHookProc = MouseHookProc;
					_MouseHookHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE, _MouseHookProc, new HandleRef(null, NativeMethods.GetModuleHandle(null)), NativeMethods.GetCurrentThreadId());
				}
			}
		}

		private static void UninstallMouseHook()
		{
			lock(_InstallLock)
			{
				if(_MouseHookHandle != IntPtr.Zero)
				{
					NativeMethods.UnhookWindowsHookEx(new HandleRef(null, _MouseHookHandle));
					_MouseHookHandle = IntPtr.Zero;
				}
			}
		}

		private static IntPtr MouseHookProc(int nCode, IntPtr intPtr, IntPtr lParam)
		{
			throw new NotImplementedException();
		}

	}

	#endregion

	#region KeyboardHookManager

	[TESharedAnnotations.Incomplete]
	internal static class KeyboardHookManager
	{

		private static NativeMethods.HookProc _HookProc = HookProc;
		private static IntPtr _HookId;

		private static EventHandler<KeyInputEventArgs> _KeyDown;
		private static EventHandler<KeyInputEventArgs> _KeyUp;
		private static EventHandler<KeyInputEventArgs> _KeyPress;

		private static IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			var handled = false;
			if(nCode >= 0)
			{
				var keyboardData = (NativeMethods.KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.KEYBDINPUT));
				var wm = (int)wParam;
				if(_KeyDown != null && (wm == NativeMethods.WM_KEYDOWN || wm == NativeMethods.WM_SYSKEYDOWN))
				{
					var e = new KeyInputEventArgs(keyboardData);
					_KeyDown(null, e);
					handled = e.Handled;
				}
				if(_KeyPress != null && wm == NativeMethods.WM_KEYDOWN)
				{
					var isDownShift = (NativeMethods.GetKeyState(NativeMethods.VK_SHIFT) & 0x80) == 0x80;
					var isDownCapsLock = NativeMethods.GetKeyState(NativeMethods.VK_CAPITAL) != 0;
					var keyState = new byte[256];
					NativeMethods.GetKeyboardState(keyState);
					var inBuffer = new byte[2];
					if(NativeMethods.ToAscii(keyboardData.wVk, keyboardData.wScan, keyState, inBuffer, keyboardData.dwFlags) == 1)
					{
						var key = (char)inBuffer[0];
						if((isDownCapsLock ^ isDownShift) && Char.IsLetter(key))
							key = Char.ToUpper(key);
						var e = new KeyInputEventArgs(keyboardData, key);
						_KeyPress(null, e);
						handled |= e.Handled;
					}
				}
				if(_KeyUp != null && (wm == NativeMethods.WM_KEYUP || wm == NativeMethods.WM_SYSKEYUP))
				{
					var e = new KeyInputEventArgs(keyboardData);
					_KeyUp(null, e);
					handled |= e.Handled;
				}
			}
			if(handled)
				return new IntPtr(-1);
			return NativeMethods.CallNextHookEx(_HookId, nCode, wParam, lParam);
		}

		private static IntPtr SetHook(NativeMethods.HookProc proc)
		{
			using(var process = Process.GetCurrentProcess())
			using(var module = process.MainModule)
				return NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL, proc, NativeMethods.GetModuleHandle(module.ModuleName), 0);
		}

	}

	#endregion

	#region MouseHookManager

	[TESharedAnnotations.Incomplete]
	internal static class MouseHookManager
	{

		private static NativeMethods.HookProc _HookProc = HookProc;
		private static IntPtr _HookId = IntPtr.Zero;

		private static IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			throw new NotImplementedException();
		}

	}

	#endregion

}
