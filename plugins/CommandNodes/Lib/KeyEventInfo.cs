using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommandNodes
{

	#region KeyEventInfo

	[Obsolete]
	public sealed class KeyEventInfo
	{

		#region Static / Constant

		private static readonly KeysConverter _Converter = new KeysConverter();

		private static readonly Dictionary<string, Keys> _KeyboardOutputToKeyCode;
		private static readonly Dictionary<Keys, string> _KeyCodeToKeyboardOutput;

		static KeyEventInfo()
		{
			_KeyboardOutputToKeyCode = new Dictionary<string, Keys>(StringComparer.OrdinalIgnoreCase);
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
			_KeyboardOutputToKeyCode.Add(output, key);
			_KeyCodeToKeyboardOutput.Add(key, output);
		}

		internal static Keys ParseKeyboardOutputData(string output)
		{
			if(String.IsNullOrEmpty(output))
				return Keys.None;
			Keys data;
			return _KeyboardOutputToKeyCode.TryGetValue(output.Trim(), out data) ? data : Keys.None;
			//return ParseKeyboardOutputDataOrNull(output) ?? Keys.None;
		}

		internal static Keys? ParseKeyboardOutputDataOrNull(string output)
		{
			if(String.IsNullOrEmpty(output))
				return null;
			Keys data;
			return _KeyboardOutputToKeyCode.TryGetValue(output, out data) ? (Keys?)data : null;
		}

		internal static Keys MakeModifiers(bool control, bool shift, bool alt)
		{
			var mods = Keys.None;
			if(control)
				mods |= Keys.Control;
			if(shift)
				mods |= Keys.Shift;
			if(alt)
				mods |= Keys.Alt;
			return mods;
		}

		#endregion

		#region Fields

		private readonly Keys _KeyData;

		#endregion

		#region Properties

		public Keys KeyCode
		{
			get
			{
				var keys = _KeyData & Keys.KeyCode;
				return Enum.IsDefined(typeof(Keys), (int)keys) ? keys : Keys.None;
			}
		}

		public Keys KeyData
		{
			get { return _KeyData; }
		}

		public int KeyValue
		{
			get { return ((int)_KeyData) & (int)Keys.KeyCode; }
		}

		public bool Control
		{
			get { return (_KeyData & Keys.Control) == Keys.Control; }
		}

		public bool Shift
		{
			get { return (_KeyData & Keys.Shift) == Keys.Shift; }
		}

		public bool Alt
		{
			get { return (_KeyData & Keys.Alt) == Keys.Alt; }
		}

		public Keys Modifiers
		{
			get { return _KeyData & Keys.Modifiers; }
		}

		#endregion

		#region Constructors

		public KeyEventInfo(Keys keyData)
		{
			_KeyData = keyData;
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			// ReSharper disable AssignNullToNotNullAttribute
			return _Converter.ConvertToString(_KeyData);
			// ReSharper restore AssignNullToNotNullAttribute
		}

		public override int GetHashCode()
		{
			return _KeyData.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(obj is KeyEventInfo)
				return _KeyData == ((KeyEventInfo)obj)._KeyData;
			if(obj is Keys)
				return _KeyData == (Keys)obj;
			if(obj is int)
				return (int)_KeyData == (int)obj;
			return false;
		}

		#endregion

	}

	#endregion

}
