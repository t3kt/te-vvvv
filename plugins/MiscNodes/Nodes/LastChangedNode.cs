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
