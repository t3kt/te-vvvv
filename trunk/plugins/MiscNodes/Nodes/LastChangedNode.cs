using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.Hosting.Pins;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region LastChangedNode<T>

	public abstract class LastChangedNode<T> : IPluginEvaluate, IPartImportsSatisfiedNotification, IDisposable
	{

		#region Static / Constant

		private static InputAttribute CreateInputAttribute(int index)
		{
			return new InputAttribute("Input " + index);
		}

		#endregion

		#region Fields

		//[Import]
		private IPluginHost _Host;

		//[Config("Input Count", MinValue = 1, IsSingle = true)]
		private IDiffSpread<int> _InputCountConfig;

		private readonly Stack<DiffPin<T>> _Inputs = new Stack<DiffPin<T>>();

		[Output("Output")]
		private ISpread<T> _Output;

		[Output("Last Index")]
		private ISpread<int> _LastIndexOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		protected LastChangedNode(IPluginHost host, IDiffSpread<int> inputCountConfig)
		{
			_Host = host;
			_InputCountConfig = inputCountConfig;
			_InputCountConfig.Changed += this.InputCount_Changed;
			//RebuildInputs(_InputCountConfig[0]);
		}

		#endregion

		#region Methods

		private void InputCount_Changed(IDiffSpread<int> inputCountConfig)
		{
			if(inputCountConfig[0] != _Inputs.Count)
			{
				RebuildInputs(inputCountConfig[0]);
			}
		}

		private void RebuildInputs(int count)
		{
			//if(_Inputs == null)
			//    _Inputs = new Stack<DiffPin<T>>();
			if(count == 0)
			{
				foreach(var input in _Inputs)
					input.Dispose();
				_Inputs.Clear();
			}
			while(_Inputs.Count > count)
			{
				_Inputs.Pop().Dispose();
			}
			while(_Inputs.Count < count)
			{
				_Inputs.Push(PinFactory.CreateDiffPin<T>(_Host, CreateInputAttribute(_Inputs.Count)));
			}
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			//if(_InputCountConfig.IsChanged)
			//    RebuildInputs(_InputCountConfig[0]);
			var index = 0;
			foreach(var input in _Inputs)
			{
				if(input.IsChanged)
				{
					_Output.AssignFrom(input.ToList());
					_LastIndexOutput[0] = index;
					_LastIndexOutput[0] = _Inputs.Count - index - 1;
					//_LastIndexOutput[0] = input.PluginIO.Order;
					break;
				}
				index++;
			}
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			//RebuildInputs(_InputCountConfig[0]);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_InputCountConfig.Changed -= this.InputCount_Changed;
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
