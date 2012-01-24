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
			public const string AsKey = "AsKey";
			public const string AsString = "AsString";
			public const string AsValue = "AsValue";
			public const string Cons = "Cons";
			public const string Controller = "Controller";
			public const string Count = "Count";
			public const string CurrentTicks = "CurrentTicks";
			public const string Default = "Default";
			public const string DetectInput = "DetectInput";
			public const string EnsureType = "EnsureType";
			public const string Gate = "Gate";
			public const string GetTypes = "GetTypes";
			public const string KeyGesture = "KeyGesture";
			public const string LastChanged = "LastChanged";
			public const string Modifiers = "Modifiers";
			public const string OfType = "OfType";
			public const string Parse = "Parse";
			public const string Status = "Status";
			public const string Struct = "Struct";
			public const string Switch = "Switch";
			public const string Transports = "Transports";
			public const string Trigger = "Trigger";
		}

		public static class Categories
		{
			public const string Animation = "Animation";
			public const string Boolean = "Boolean";
			public const string Color = "Color";
			public const string Debug = "Debug";
			public const string Enumerations = "Enumerations";
			public const string GUI = "GUI";
			/*~~~MISC~~~*/public const string KeyGesture = "KeyGesture";
			/*~~~MISC~~~*/public const string Keys = "Keys";
			public const string String = "String";
			/*STRUCT*/public const string Struct = "Struct";
			public const string Transform = "Transform";
			/*TRANSPORT*/public const string Transport = "Transport";
			public const string Value = "Value";
			/*XAML*/public const string Xaml = "Xaml";

			public const string TwoD = "2D";
			public const string ThreeD = "3D";
			public const string FourD = "4D";
		}

		public static class Versions
		{
			public const string Add = "Add";
			public const string Advanced = "Advanced";
			public const string File = "File";
			/*STRUCT*/public const string FixedType = "FixedType";
			public const string Global = "Global";
			public const string Input = "Input";
			public const string Join = "Join";
			public const string Output = "Output";
			public const string Split = "Split";
			public const string Test = "Test";
			public const string GUI = "GUI";
		}

		public static class Warnings
		{
			public const string Incomplete = "This is incomplete";
			public const string Obsolete = "This is obsolete and may be removed soon.";
		}

	}

	#endregion

}
