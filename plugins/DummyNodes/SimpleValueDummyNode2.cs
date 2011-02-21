using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region SimpleValueDummyNode2

	[PluginInfo(Name = "Dummy", Category = "Value", Version = "Simple2", Author = "te")]
	public class SimpleValueDummyNode2 : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		//[Import]
		//private IPinFactory _PinFactory;

		private IPluginHost _Host;

		[Import]
		protected ILogger _Logger;

		[Config("InputCount", DefaultValue = 1, AsInt = true, IsSingle = true, MinValue = 0, Visibility = PinVisibility.OnlyInspector)]
		private IDiffSpread<int> _InputCount;

		private IDiffSpread<IDiffSpread<double>> _Inputs;


		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public SimpleValueDummyNode2(IPluginHost host,
			IDiffSpread<int> inputCount)
		{
			_Host = host;
			_InputCount = inputCount;
		}

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
