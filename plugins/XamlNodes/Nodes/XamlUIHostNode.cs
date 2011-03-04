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

		private static readonly string[] _SharedAssemblies = new[] { "WindowsBase", "PresentationCore", "PresentationFramework", XamlNodesShared.Names.Assembly };

		private static IEnumerable<string> GetSharedAssemblies()
		{
			return _SharedAssemblies;
		}

		private static readonly NamespaceMapEntry[] _SharedNamespaceMapEntries =
			new[]
			{
				new NamespaceMapEntry(XamlNodesShared.Names.XmlCore, XamlNodesShared.Names.AssemblyFull, XamlNodesShared.Names.ClrCore),
				new NamespaceMapEntry(XamlNodesShared.Names.XmlCorePins, XamlNodesShared.Names.AssemblyFull, XamlNodesShared.Names.ClrCorePins)
			};

		private static IEnumerable<NamespaceMapEntry> GetSharedNamespaceMapEntries()
		{
			return _SharedNamespaceMapEntries;
		}

		#endregion

		#region Fields

		[Import]
		private IPluginHost _PluginHost;

		[Config("LoadNode", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _LoadNodePin;

		[Config("XamlPath", IsSingle = true, StringType = StringType.Filename, FileMask = "XAML files (*.xaml)|*.xaml|All files (*.*)|*.*")]
		private IDiffSpread<string> _XamlPathPin;

		[Config("XamlSource", IsSingle = true)]
		private IDiffSpread<string> _XamlSourcePin;

		[Config("XamlControlType", IsSingle = true)]
		private IDiffSpread<string> _XamlControlTypePin;

		[Config("ParserBaseUri", IsSingle = true)]
		private ISpread<string> _ParserBaseUriConfig;

		[Config("ParserNamespaces")]
		private ISpread<string> _ParserNamespaceEntriesConfig;

		[Config("ParserAssemblies")]
		private ISpread<string> _ParserAssembliesConfig;

		private IXamlNode _Node;

		private string _XamlPath;
		private string _XamlSource;
		private string _XamlControlTypeName;
		private bool _FirstLoad = true;
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

		internal void Log(string message, params object[] args)
		{
			Log(TLogType.Debug, message, args);
		}

		internal void Log(TLogType logType, string message, params object[] args)
		{
			_PluginHost.Log(logType, String.Format(message, args));
		}

		private void Xaml_Changed(IDiffSpread<string> spread)
		{
			ReloadXaml(false);
		}

		private void Load_Changed(IDiffSpread<bool> spread)
		{
			ReloadXaml(true);
		}

		private NamespaceMapEntry[] GetNamespaceMapEntries()
		{
			var entries = new List<NamespaceMapEntry>(GetSharedNamespaceMapEntries());
			entries.AddRange(_ParserNamespaceEntriesConfig.Select(ParseNamespaceMapEntry).Where(e => e != null));
			if(entries.Count == 0)
				return null;
			return entries.ToArray();
		}

		private string[] GetAssemblies()
		{
			var assemblies = new List<string>(GetSharedAssemblies());
			assemblies.AddRange(_ParserAssembliesConfig.Where(a => !String.IsNullOrEmpty(a)));
			return assemblies.ToArray();
		}

		private ParserContext PrepareParserContext()
		{
			var ctx = new ParserContext();
			if(!String.IsNullOrEmpty(_ParserBaseUriConfig[0]))
				ctx.BaseUri = new Uri(_ParserBaseUriConfig[0]);
			var nsEntries = GetNamespaceMapEntries();
			var assemblies = GetAssemblies();
			if(nsEntries != null)
				ctx.XamlTypeMapper = new XamlTypeMapper(assemblies, nsEntries);
			return ctx;
		}

		private object LoadXamlNode()
		{
			if(!String.IsNullOrEmpty(_XamlSource))
				return XamlReader.Parse(_XamlSource, PrepareParserContext());
			if(!String.IsNullOrEmpty(_XamlControlTypeName))
			{
				var ctlType = Type.GetType(_XamlControlTypeName, false, true);
				if(ctlType != null)
					return Activator.CreateInstance(ctlType, true);
			}
			if(!String.IsNullOrEmpty(_XamlPath) && File.Exists(_XamlPath))
			{
				using(var stream = File.OpenRead(_XamlPath))
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
				this.Refresh();
			}
		}

		private void ReloadXaml(bool force)
		{
			Log("Reload XAML (force: {0})", force);
			var xamlPath = _XamlPathPin[0].OrNullIfEmpty();
			var xamlSource = _XamlSourcePin[0].OrNullIfEmpty();
			var xamlControlTypeName = _XamlControlTypePin[0].OrNullIfEmpty();
			if(force ||
				xamlPath != _XamlPath ||
				xamlSource != _XamlSource ||
				xamlControlTypeName != _XamlControlTypeName)
			{
				_XamlPath = xamlPath;
				_XamlSource = xamlSource;
				_XamlControlTypeName = xamlControlTypeName;
				UnloadXamlNode();
				var nodeObj = this.LoadXamlNode();
				AttachXamlNode(nodeObj);
				_FirstLoad = false;
			}
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
			_XamlPathPin.Changed += this.Xaml_Changed;
			_XamlSourcePin.Changed += this.Xaml_Changed;
			_XamlControlTypePin.Changed += this.Xaml_Changed;
			_LoadNodePin.Changed += this.Load_Changed;
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

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_FirstLoad)
			{
				ReloadXaml(true);
			}
			if(_Node != null)
				_Node.Evaluate(spreadMax);
		}

		#endregion

	}

	#endregion

}
