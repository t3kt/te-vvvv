using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TESharedAnnotations;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region LastChangedSpectralNode<T>

	[Incomplete]
	public abstract class LastChangedSpectralNode<T>
	{

		#region Static/Constant

		#endregion

		#region Fields

		[Input("Input")]
		private IDiffSpread<T> _Input;

		[Output("Output", IsSingle = true)]
		private ISpread<T> _Output;

		[Output("Last Index", IsSingle = true)]
		private ISpread<int> _LastIndexOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
