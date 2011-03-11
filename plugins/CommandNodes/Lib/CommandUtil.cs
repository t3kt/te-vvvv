using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;
using Keys = System.Windows.Forms.Keys;

namespace CommandNodes
{

	#region CommandUtil

	internal static class CommandUtil
	{

		internal const int AltKeyCode = 18;
		internal const int ControlKeyCode = 17;
		internal const int ShiftKeyCode = 16;

		internal static readonly IEqualityComparer<string> CommandIdComparer = StringComparer.OrdinalIgnoreCase;

		internal const int InvalidKeyCode = -1;

		internal const char ButtonNameSeparator = '+';

		internal const int CommandModifiersMask = (int)(CommandModifiers.Alt | CommandModifiers.Shift | CommandModifiers.Control);

		internal const int MouseButtonsMask = (int)(MouseButtons.Left | MouseButtons.Middle | MouseButtons.Right);

		internal static readonly string[] NoCommands = new string[0];

		static CommandUtil()
		{
			InitKeyCodeNames();
		}

		[TESharedAnnotations.Unused]
		public static CommandModifiers MakeModifiers(bool alt, bool control, bool shift)
		{
			var mods = CommandModifiers.None;
			if(alt)
				mods |= CommandModifiers.Alt;
			if(control)
				mods |= CommandModifiers.Control;
			if(shift)
				mods |= CommandModifiers.Shift;
			return mods;
		}

		public static CommandModifiers GetKeyCodeModifiers(IEnumerable<int> keyCodes)
		{
			var mods = CommandModifiers.None;
			if(keyCodes != null)
			{
				foreach(var keyCode in keyCodes)
				{
					switch(keyCode)
					{
					case AltKeyCode:
						mods |= CommandModifiers.Alt;
						break;
					case ControlKeyCode:
						mods |= CommandModifiers.Control;
						break;
					case ShiftKeyCode:
						mods |= CommandModifiers.Shift;
						break;
					}
				}
			}
			return mods;
		}

		internal static int[] ProcessKeyCodes(IEnumerable<int> keyCodes, out CommandModifiers modifiers)
		{
			var keys = new List<int>();
			modifiers = CommandModifiers.None;
			if(keyCodes != null)
			{
				foreach(var keyCode in keyCodes)
				{
					switch(keyCode)
					{
					case AltKeyCode:
						modifiers |= CommandModifiers.Alt;
						break;
					case ControlKeyCode:
						modifiers |= CommandModifiers.Control;
						break;
					case ShiftKeyCode:
						modifiers |= CommandModifiers.Shift;
						break;
					default:
						keys.Add(keyCode);
						break;
					}
				}
			}
			return keys.ToArray();
		}

		[TESharedAnnotations.Unused]
		internal static MouseButtons GetMouseButtons(bool left, bool middle, bool right)
		{
			var btns = MouseButtons.None;
			if(left)
				btns |= MouseButtons.Left;
			if(middle)
				btns |= MouseButtons.Middle;
			if(right)
				btns |= MouseButtons.Right;
			return btns;
		}

		internal static int EncodeKeyCommand(int keyCode, CommandModifiers modifiers)
		{
			return keyCode | (int)modifiers;
		}

		internal static int EncodeMouseCommand(MouseButtons buttons, CommandModifiers modifiers)
		{
			return (int)buttons | (int)modifiers;
		}

		internal static int EncodeMouseCommand(bool left, bool middle, bool right, CommandModifiers modifiers)
		{
			var code = 0;
			if(left)
				code |= (int)MouseButtons.Left;
			if(middle)
				code |= (int)MouseButtons.Middle;
			if(right)
				code |= (int)MouseButtons.Right;
			code |= (int)modifiers;
			return code;
		}

		#region Key Code Names

		private static Dictionary<string, int> _KeyCodesByName;

		private static void AddKeyCode(Keys key)
		{
			_KeyCodesByName.Add(String.Format("<{0}>", key), (int)key);
		}

		private static void AddNumericKeyCode(Keys key)
		{
			_KeyCodesByName.Add(String.Format("<KEY{0:D}>", key), (int)key);
		}

		private static void AddKeyCodes(params Keys[] keys)
		{
			foreach(var key in keys)
				AddKeyCode(key);
		}

		private static void AddNumericKeyCodes(params Keys[] keys)
		{
			foreach(var key in keys)
				AddNumericKeyCode(key);
		}

		private static void AddKeyCode(string name, Keys key)
		{
			_KeyCodesByName.Add(name, (int)key);
		}

		private static void InitKeyCodeNames()
		{
			_KeyCodesByName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			AddKeyCode("<SHIFT>", Keys.ShiftKey);
			AddKeyCode("<CONTROL>", Keys.ControlKey);
			AddKeyCode("<ALT>", Keys.Menu);
			AddKeyCodes(Keys.Home, Keys.Next, Keys.Prior, Keys.Insert, Keys.Delete);
			AddKeyCodes(Keys.NumLock, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9);
			AddKeyCodes(Keys.Scroll, Keys.PrintScreen, Keys.Pause);
			AddKeyCodes(Keys.LWin, Keys.RWin, Keys.Apps);
			AddKeyCodes(Keys.Space, Keys.Back);
			AddKeyCode("<RETURN>", Keys.Return);
			AddKeyCodes(Keys.Escape,
							  Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
							  Keys.F13, Keys.F14, Keys.F15, Keys.F16, Keys.F17, Keys.F18, Keys.F19, Keys.F20, Keys.F21, Keys.F22, Keys.F23, Keys.F24);
			AddKeyCodes(Keys.Up, Keys.Down, Keys.Left, Keys.Right);
			AddKeyCodes(Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
							  Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z);
			AddKeyCodes(Keys.Tab, Keys.Capital);
			AddNumericKeyCodes(Keys.Oemtilde, Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.OemPipe, Keys.OemQuotes, Keys.OemMinus, Keys.Oemplus, Keys.OemSemicolon, Keys.Oemcomma, Keys.OemPeriod, Keys.OemQuestion);
		}

		#endregion

		private static bool TryParseKeyCode(string str, out int keyCode)
		{
			if(!String.IsNullOrEmpty(str))
			{
				if(Int32.TryParse(str, out keyCode))
					return true;
				if(_KeyCodesByName.TryGetValue(str, out keyCode))
					return true;
			}
			keyCode = InvalidKeyCode;
			return false;
		}

		private static bool TryParseModifier(string str, out CommandModifiers modifier)
		{
			if(!String.IsNullOrEmpty(str))
			{
				switch(str.Trim().ToUpper())
				{
				case "<SHIFT>":
					modifier = CommandModifiers.Shift;
					return true;
				case "<CONTROL>":
				case "<CTRL>":
					modifier = CommandModifiers.Control;
					return true;
				case "<ALT>":
					modifier = CommandModifiers.Alt;
					return true;
				}
			}
			modifier = CommandModifiers.None;
			return false;
		}

		private static bool TryParseMouseButton(string str, out MouseButtons button)
		{
			if(!String.IsNullOrEmpty(str))
			{
				switch(str.Trim().ToUpper())
				{
				case "<LEFT>":
				case "<LBUTTON>":
					button = MouseButtons.Left;
					return true;
				case "<MIDDLE>":
				case "<MBUTTON>":
					button = MouseButtons.Middle;
					return true;
				case "<RIGHT>":
				case "<RBUTTON>":
					button = MouseButtons.Right;
					return true;
				}
			}
			button = MouseButtons.None;
			return false;
		}

		internal static bool TryParseKeyTrigger(string str, out int keyCode, out CommandModifiers modifiers)
		{
			if(!String.IsNullOrEmpty(str))
			{
				var parts = str.Split(new[] { ButtonNameSeparator }, StringSplitOptions.RemoveEmptyEntries);
				if(parts.Length > 0)
				{
					keyCode = 0;
					modifiers = CommandModifiers.None;
					foreach(var part in parts)
					{
						CommandModifiers cm;
						int kc;
						if(TryParseModifier(part, out cm))
						{
							modifiers |= cm;
						}
						else if(TryParseKeyCode(part, out kc))
						{
							keyCode |= kc;
						}
						else
						{
							keyCode = InvalidKeyCode;
							modifiers = CommandModifiers.None;
							return false;
						}
					}
					if(keyCode != 0)
						return true;
				}
			}
			keyCode = InvalidKeyCode;
			modifiers = CommandModifiers.None;
			return false;
		}

		internal static bool TryParseMouseTrigger(string str, out MouseButtons buttons, out CommandModifiers modifiers)
		{
			if(!String.IsNullOrEmpty(str))
			{
				var parts = str.Split(new[] { ButtonNameSeparator }, StringSplitOptions.RemoveEmptyEntries);
				if(parts.Length > 0)
				{
					buttons = MouseButtons.None;
					modifiers = CommandModifiers.None;
					foreach(var part in parts)
					{
						CommandModifiers cm;
						MouseButtons mb;
						if(TryParseModifier(part, out cm))
						{
							modifiers |= cm;
						}
						else if(TryParseMouseButton(part, out mb))
						{
							buttons |= mb;
						}
						else
						{
							buttons = MouseButtons.None;
							modifiers = CommandModifiers.None;
							return false;
						}
					}
					if(buttons != MouseButtons.None)
						return true;
				}
			}
			buttons = MouseButtons.None;
			modifiers = CommandModifiers.None;
			return false;
		}

		private static bool TryParseTriggerType(string str, out CommandTriggerType triggerType)
		{
			if(!String.IsNullOrEmpty(str))
			{
				switch(str.Trim().ToUpper())
				{
				case "#M":
				case "#MOUSE#":
					triggerType = CommandTriggerType.Mouse;
					return true;
				case "#K":
				case "#KEYBOARD#":
					triggerType = CommandTriggerType.Keyboard;
					return true;
				}
			}
			triggerType = CommandTriggerType.None;
			return false;
		}

		internal static CommandMapping ParseMapping(string triggerTypeStr, string triggerStr, string commandId)
		{
			if(String.IsNullOrEmpty(commandId))
				return null;
			CommandTriggerType triggerType;
			if(!TryParseTriggerType(triggerTypeStr, out triggerType))
				return null;
			CommandModifiers modifiers;
			switch(triggerType)
			{
			case CommandTriggerType.Keyboard:
				int keyCode;
				if(!TryParseKeyTrigger(triggerStr, out keyCode, out modifiers))
					return null;
				return new CommandMapping(keyCode, modifiers, commandId);
			case CommandTriggerType.Mouse:
				MouseButtons buttons;
				if(!TryParseMouseTrigger(triggerStr, out buttons, out modifiers))
					return null;
				return new CommandMapping(buttons, modifiers, commandId);
			default:
				return null;
			}
		}

		internal static CommandMapping ParseMapping(string triggerTypeStr, string triggerStr, string commandId, ILogger logger)
		{
			var mapping = ParseMapping(triggerTypeStr, triggerStr, commandId);
			if(logger != null && mapping == null)
				logger.Log(new FormatException(String.Format("Cannot parse command mapping triggerType: '{0}' trigger: '{1}' commandId: '{2}'", triggerTypeStr, triggerStr, commandId)));
			return mapping;
		}
	}

	#endregion

}
