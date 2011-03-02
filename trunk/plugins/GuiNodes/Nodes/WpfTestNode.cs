using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	[PluginInfo(Name = "WpfTest",
		Category = TEShared.Names.Categories.GUI,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		InitialComponentMode = TComponentMode.InABox,
		InitialBoxHeight = 100,
		InitialWindowHeight = 100,
		InitialBoxWidth = 100,
		InitialWindowWidth = 100)]
	public partial class WpfTestNode : UserControl, IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public WpfTestNode()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int SpreadMax)
		{
		}

		#endregion

	}
}
