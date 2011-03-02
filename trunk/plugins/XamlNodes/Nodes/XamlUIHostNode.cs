using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using XamlNodes.Core;

namespace XamlNodes.Nodes
{

	#region XamlUIHostNode

	[PluginInfo(Name = "Host",
		Category = TEShared.Names.Categories.Xaml,
		Author = TEShared.Names.Author,
		InitialBoxWidth = 100,
		InitialBoxHeight = 100,
		InitialWindowWidth = 100,
		InitialWindowHeight = 100,
		InitialComponentMode = TComponentMode.InABox)]
	public partial class XamlUIHostNode : UserControl, IXamlNodeHost, IPartImportsSatisfiedNotification
	{

		#region Static/Constant

		private static NamespaceMapEntry ParseNamespaceMapEntry(string str)
		{
			if(String.IsNullOrEmpty(str))
				return null;
			var parts = str.Split('|');
			if(parts.Length < 3)
				return null;
			return new NamespaceMapEntry(parts[0], parts[1], parts[3]);
		}

		private static string[] GetSharedAssemblies()
		{
			return new[] { "WindowsBase", "PresentationCore", "PresentationFramework", "XamlNodes" };
		}

		private static NamespaceMapEntry[] GetSharedNamespaceMapEntries()
		{
			return new NamespaceMapEntry[0];
		}

		#endregion

		#region Fields

		[Import]
		private IPluginHost _PluginHost;

		[Config("XamlPath", IsSingle = true, StringType = StringType.Filename, FileMask = "XAML files (*.xaml)|*.xaml|All files (*.*)|*.*")]
		private IDiffSpread<string> _XamlPathConfig;

		[Config("XamlSource", IsSingle = true)]
		private IDiffSpread<string> _XamlSourceConfig;

		[Config("ParserBaseUri", IsSingle = true)]
		private ISpread<string> _ParserBaseUri;

		[Config("ParserNamespaces")]
		private ISpread<string> _ParserNamespaceEntries;

		[Config("ParserAssemblies")]
		private ISpread<string> _ParserAssemblies;

		private IXamlNode _Node;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public XamlUIHostNode()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void Xaml_Changed(IDiffSpread<string> spread)
		{
			ReloadXaml();
		}

		private NamespaceMapEntry[] GetNamespaceMapEntries()
		{
			var entries = new List<NamespaceMapEntry>(GetSharedNamespaceMapEntries());
			entries.AddRange(_ParserNamespaceEntries.Select(ParseNamespaceMapEntry).Where(e => e != null));
			if(entries.Count == 0)
				return null;
			return entries.ToArray();
		}

		private string[] GetAssemblies()
		{
			var assemblies = new List<string>(GetSharedAssemblies());
			assemblies.AddRange(_ParserAssemblies.Where(a => !String.IsNullOrEmpty(a)));
			return assemblies.ToArray();
		}

		private ParserContext PrepareParserContext()
		{
			var ctx = new ParserContext();
			if(!String.IsNullOrEmpty(_ParserBaseUri[0]))
				ctx.BaseUri = new Uri(_ParserBaseUri[0]);
			var nsEntries = GetNamespaceMapEntries();
			var assemblies = GetAssemblies();
			if(nsEntries != null)
				ctx.XamlTypeMapper = new XamlTypeMapper(assemblies, nsEntries);
			return ctx;
		}

		private object LoadXamlNode()
		{
			var xamlSource = _XamlSourceConfig[0];
			if(!String.IsNullOrEmpty(xamlSource))
				return XamlReader.Parse(xamlSource, PrepareParserContext());
			var xamlPath = _XamlPathConfig[0];
			if(!String.IsNullOrEmpty(xamlPath) || !File.Exists(xamlPath))
			{
				using(var stream = File.OpenRead(xamlPath))
					return XamlReader.Load(stream, PrepareParserContext());
			}
			return null;
		}

		private void AttachXamlNode(object nodeObj)
		{
			if(nodeObj != null)
			{
				Debug.Assert(nodeObj is IXamlNode);
				Debug.Assert(nodeObj is UIElement);
				var node = (IXamlNode)nodeObj;
				var ctl = (UIElement)nodeObj;
				node.SetHost(this);
				_ElementHost.Child = ctl;
			}
		}

		private void ReloadXaml()
		{
			UnloadXamlNode();
			var nodeObj = this.LoadXamlNode();
			AttachXamlNode(nodeObj);
		}

		private void UnloadXamlNode()
		{
			if(_Node != null)
			{
				_Node.Dispose();
				_Node = null;
			}
			_ElementHost.Child = null;
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_XamlPathConfig.Changed += this.Xaml_Changed;
			_XamlSourceConfig.Changed += this.Xaml_Changed;
		}

		#endregion

		#region IXamlNodeHost Members

		public IPluginHost PluginHost
		{
			get { return _PluginHost; }
		}

		public IXamlNode Node
		{
			get { return _Node; }
		}

		#endregion

	}

	#endregion

}
