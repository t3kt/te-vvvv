using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region LastChangedStringNode

	[PluginInfo(
		Name = TEShared.Names.Nodes.LastChanged,
		Category = TEShared.Names.Categories.String,
		Author = TEShared.Names.Author)]
	public sealed class LastChangedStringNode : LastChangedNode<string>
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public LastChangedStringNode(IPluginHost host, [Config("Input Count", MinValue = 1, IsSingle = true)]IDiffSpread<int> inputCountConfig)
			: base(host, inputCountConfig)
		{
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
