using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region CommandModifiers

	[Flags]
	public enum CommandModifiers
	{
		None	= 0x000000,
		Alt		= 0x100000,
		Control	= 0x200000,
		Shift	= 0x400000
	}

	#endregion

}
