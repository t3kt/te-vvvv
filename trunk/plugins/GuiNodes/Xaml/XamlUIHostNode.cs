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

namespace VVVV.Xaml
{

	#region XamlUIHostNode

	[PluginInfo(Name = "XamlUIHost",
		Category = TEShared.Names.Categories.GUI,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		InitialComponentMode = TComponentMode.InABox,
		InitialBoxHeight = 100,
		InitialWindowHeight = 100,
		InitialBoxWidth = 100,
		InitialWindowWidth = 100)]
	public partial class XamlUIHostNode : UserControl, IPluginEvaluate, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		private static UIElement ReadXamlElement(string path)
		{
			using(var stream = File.OpenRead(path))
			{
				var content = XamlReader.Load(stream);
				Debug.Assert(content is UIElement);
				return (UIElement)content;
			}
		}

		#endregion

		#region Fields

		[Import]
		private IPluginHost _Host;

		[Config("Path", IsSingle = true, StringType = StringType.Filename)]
		private IDiffSpread<string> _Path;

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

		private void LoadXaml(string path)
		{
			if(_ElementHost.Child != null && _ElementHost.Child is IDisposable)
				((IDisposable)_ElementHost.Child).Dispose();
			if(String.IsNullOrEmpty(path))
				_ElementHost.Child = null;
			else
				_ElementHost.Child = ReadXamlElement(path);
		}

		private void Path_Changed(IDiffSpread<string> path)
		{
			LoadXaml(path[0]);
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_Path.Changed += this.Path_Changed;
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
		}

		#endregion

	}

	#endregion

}
