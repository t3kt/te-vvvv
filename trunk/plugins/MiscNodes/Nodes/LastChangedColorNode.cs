using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;

namespace VVVV.Nodes
{

	#region LastChangedColorNode

	[PluginInfo(
		Name = MiscNodesShared.Names.Nodes.LastChanged,
		Category = TEShared.Names.Categories.Color,
		Author = TEShared.Names.Author)]
	public sealed class LastChangedColorNode : LastChangedNode<RGBAColor>
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public LastChangedColorNode(IPluginHost host, [Config("Input Count", MinValue = 1, IsSingle = true)]IDiffSpread<int> inputCountConfig)
			: base(host, inputCountConfig)
		{
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
