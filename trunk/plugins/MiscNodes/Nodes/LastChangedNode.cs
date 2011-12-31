using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.Hosting.Pins;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;

namespace VVVV.Nodes
{

	#region LastChangedNode_OLD<T>

	[Obsolete]
	internal abstract class LastChangedNode_OLD<T> : IPluginEvaluate,
		//IPartImportsSatisfiedNotification,
		IDisposable
	{

		#region Static / Constant

		private static InputAttribute CreateInputAttribute(int index)
		{
			return new InputAttribute("Input " + (index + 1));
		}

		#endregion

		#region Fields

		//[Import]
		private readonly IPluginHost _Host;

		//[Config("Input Count", MinValue = 1, IsSingle = true)]
		private readonly IDiffSpread<int> _InputCountConfig;

		private readonly Stack<DiffPin<T>> _Inputs = new Stack<DiffPin<T>>();

		[Output("Output")]
		private ISpread<T> _Output;

		[Output("Last Index")]
		private ISpread<int> _LastIndexOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		protected LastChangedNode_OLD(IPluginHost host, IDiffSpread<int> inputCountConfig)
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

		//public void OnImportsSatisfied()
		//{
		//    //RebuildInputs(_InputCountConfig[0]);
		//}

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

	#region LastChangedNode<T>

	public abstract class LastChangedNode<T> : IPluginEvaluate, IPartImportsSatisfiedNotification, IDisposable
	{

		#region Static/Constant

		#endregion

		#region Fields

		[Input("Input", IsPinGroup = true)]
		private IDiffSpread<ISpread<T>> _Inputs;

		[Output("Output")]
		private ISpread<T> _Output;

		[Output("Last Index", IsSingle = true)]
		private ISpread<int> _LastIndexOutput;

		private bool _Dirty;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void Inputs_Changed(IDiffSpread<ISpread<T>> spread)
		{
			_Dirty = true;
		}

		public void OnImportsSatisfied()
		{
			_Inputs.Changed += this.Inputs_Changed;
		}

		public void Evaluate(int spreadMax)
		{
			if(_Dirty)
			{
				for(var i = 0; i < _Inputs.SliceCount; i++)
				{
					var input = (IDiffSpread<T>)_Inputs[i];
					if(input.IsChanged)
					{
						_LastIndexOutput[0] = i;
						_Output.AssignFrom(input);
						break;
					}
				}
				_Dirty = true;
			}
		}

		public void Dispose()
		{
			if(_Inputs != null)
				_Inputs.Changed -= this.Inputs_Changed;
		}

		#endregion

	}

	#endregion

	#region Derived LastChangedNode Classes

	[PluginInfo(
		Name = TEShared.Names.Nodes.LastChanged,
		Category = TEShared.Names.Categories.Value,
		Author = TEShared.Names.Author)]
	public sealed class ValueLastChangedNode : LastChangedNode<double> { }

	[PluginInfo(
		Name = TEShared.Names.Nodes.LastChanged,
		Category = TEShared.Names.Categories.String,
		Author = TEShared.Names.Author)]
	public sealed class StringLastChangedNode : LastChangedNode<string> { }

	[PluginInfo(
		Name = TEShared.Names.Nodes.LastChanged,
		Category = TEShared.Names.Categories.Color,
		Author = TEShared.Names.Author)]
	public sealed class ColorLastChangedNode : LastChangedNode<RGBAColor> { }

	#endregion

}
