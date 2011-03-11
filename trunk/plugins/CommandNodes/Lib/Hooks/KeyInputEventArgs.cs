using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region KeyInputEventArgs

	[TESharedAnnotations.Incomplete]
	internal sealed class KeyInputEventArgs : InputEventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly NativeMethods.KEYBDINPUT? _KeyInputData;
		private readonly char? _KeyChar;

		#endregion

		#region Properties

		public NativeMethods.KEYBDINPUT? KeyInputData
		{
			get { return _KeyInputData; }
		}

		public char? KeyChar
		{
			get { return _KeyChar; }
		}

		#endregion

		#region Constructors

		public KeyInputEventArgs(NativeMethods.KEYBDINPUT keyInputData)
		{
			_KeyInputData = keyInputData;
		}

		public KeyInputEventArgs(char keyChar)
		{
			_KeyChar = keyChar;
		}

		public KeyInputEventArgs(NativeMethods.KEYBDINPUT keyInputData, char keyChar)
		{
			_KeyInputData = keyInputData;
			_KeyChar = keyChar;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
