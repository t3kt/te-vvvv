using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region MouseButtons

	[Flags]
	internal enum MouseButtons
	{
		None = 0,
		Left = 1,
		Right = 2,
		Middle = 4
	}

	#endregion

}
