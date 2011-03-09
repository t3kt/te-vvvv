using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEShared
{

	#region Names

	internal static class Names
	{

		public const string Author = "te";

		public const string AND = " ";

		public static class Nodes
		{
			public const string Struct = "Struct";
			public const string Default = "Default";
			public const string AsString = "AsString";
			public const string EnsureType = "EnsureType";
			public const string GetTypes = "GetTypes";
			public const string OfType = "OfType";
			public const string Cons = "Cons";
			public const string Parse = "Parse";
			public const string LastChanged = "LastChanged";
		}

		public static class Categories
		{
			public const string GUI = "GUI";
			public const string Debug = "Debug";
			public const string Xaml = "Xaml";
			public const string Value = "Value";
			public const string String = "String";
			public const string Color = "Color";
			public const string Struct = "Struct";
		}

		public static class Versions
		{
			public const string Global = "Global";
			public const string Join = "Join";
			public const string Split = "Split";
			public const string Single = "Single";
			public const string FixedType = "FixedType";
			public const string Test = "Test";
		}

	}

	#endregion

}
