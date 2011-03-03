using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsDefinition(XamlNodesShared.Names.XmlCore, XamlNodesShared.Names.ClrCore)]
[assembly: XmlnsDefinition(XamlNodesShared.Names.XmlCorePins, XamlNodesShared.Names.ClrCorePins)]

namespace XamlNodesShared
{
	internal static class Names
	{
		public const string ClrCore = "XamlNodes.Core";
		public const string ClrCorePins = ClrCore + ".Pins";
		public const string XmlCore = "http://te-vvvv/plugins/xaml/core";
		public const string XmlCorePins = XmlCore + "/pins";
		public static readonly string AssemblyFull;
		public static readonly string Assembly;
		static Names()
		{
			var assembly = typeof(Names).Assembly;
			var name = assembly.GetName();
			AssemblyFull = name.FullName;
			Assembly = name.Name;
		}
	}
}
