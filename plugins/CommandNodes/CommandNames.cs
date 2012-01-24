using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region CommandNames

	internal static class CommandNames
	{

		public static class Nodes
		{
			public const string Command = "Command";
			public const string Listener = "Listener";
			public const string Mappings = "Mappings";
			public const string KeyEvent = "KeyEvent";
		}

		public static class Categories
		{
			public const string Command = "Command";
		}

		public static class Versions
		{
			public const string KeyCode = "KeyCode";
			public const string Manual = "Manual";
			public const string Mouse = "Mouse";
		}

	}

	#endregion

}
