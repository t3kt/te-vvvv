<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	Enum.GetName(typeof(Keys), Keys.Prior).Dump();
	Enum.GetName(typeof(Keys), Keys.PageUp).Dump();
	Keys.BrowserBack.ToString().Dump();
	Keys.BrowserBack.ToString("d").Dump();
	String.Format("<{0:d}>", Keys.Oem3).Dump();
	init();
	_KeyboardOutputToKeyCode.Dump("KeyboardOutput->KeyCode");
	_KeyCodeToKeyboardOutput.Dump("KeyCode->KeyboardOutput");
	var missing = Enum.GetValues(typeof(Keys))
						.Cast<Keys>()
						.Where(k=>!_KeyCodeToKeyboardOutput.ContainsKey(k))
						.ToList();
	missing.Dump("missing");
}



		private static  Dictionary<string, Keys> _KeyboardOutputToKeyCode;
		private static  Dictionary<Keys, string> _KeyCodeToKeyboardOutput;

		static void init()
		{
			_KeyboardOutputToKeyCode = new Dictionary<string, Keys>();
			_KeyCodeToKeyboardOutput = new Dictionary<Keys, string>();
			AddOutputKeyCode("<SHIFT>", Keys.ShiftKey);
			AddOutputKeyCode("<CONTROL>", Keys.ControlKey);
			AddOutputKeyCode("<ALT>", Keys.Menu);
			AddOutputKeyCodes(Keys.Home, Keys.Next, Keys.Prior, Keys.Insert, Keys.Delete);
			AddOutputKeyCodes(Keys.NumLock,
							  Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
							  Keys.Add, Keys.Subtract, Keys.Multiply, Keys.Divide);
			AddOutputKeyCodes(Keys.Scroll, Keys.PrintScreen, Keys.Pause);
			AddOutputKeyCodes(Keys.LWin, Keys.RWin, Keys.Apps);
			AddOutputKeyCodes(Keys.Space, Keys.Back);
			AddOutputKeyCode("<RETURN>", Keys.Return);
			AddOutputKeyCodes(Keys.Escape,
							  Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
							  Keys.F13, Keys.F14, Keys.F15, Keys.F16, Keys.F17, Keys.F18, Keys.F19, Keys.F20, Keys.F21, Keys.F22, Keys.F23, Keys.F24);
			AddOutputKeyCodes(Keys.Up, Keys.Down, Keys.Left, Keys.Right);
			AddOutputKeyCodes(Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
							  Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z);
			AddOutputKeyCodes(Keys.Tab, Keys.Capital);
			AddNumericOutputKeyCodes(Keys.Oemtilde, Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.OemPipe, Keys.OemQuotes, Keys.OemMinus, Keys.Oemplus, Keys.OemSemicolon, Keys.Oemcomma, Keys.OemPeriod, Keys.OemQuestion);
		}

		private static void AddNumericOutputKeyCodes(params Keys[] keys)
		{
			foreach(var key in keys)
				AddNumericOutputKeyCode(key);
		}

		private static void AddNumericOutputKeyCode(Keys key)
		{
			AddOutputKeyCode(String.Format("<KEY{0:d}>", key), key);
		}

		private static void AddOutputKeyCodes(params Keys[] keys)
		{
			foreach(var key in keys)
				AddOutputKeyCode(key);
		}

		private static void AddOutputKeyCode(Keys key)
		{
			AddOutputKeyCode(String.Format("<{0}>", Enum.GetName(typeof(Keys), key).ToUpper()), key);
		}

		private static void AddOutputKeyCode(string output, Keys key)
		{
			try
			{
				_KeyboardOutputToKeyCode.Add(output, key);
				_KeyCodeToKeyboardOutput.Add(key, output);
			}
			catch(ArgumentException ex)
			{
				throw new Exception(String.Format("Error adding output: '{0}', key: '{1}', err: {2}", output, key, ex.Message), ex);
			}
		}

// Define other methods and classes here
