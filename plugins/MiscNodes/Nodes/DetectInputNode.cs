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

	#region DetectInputNode<T>

	public abstract class DetectInputNode<T> : IPluginEvaluate, IDisposable, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input")]
		private IDiffSpread<T> _Input;

		[Output("Output", IsBang = true, IsSingle = true)]
		private ISpread<bool> _Output;

		private bool _Changed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void Input_Changed(IDiffSpread<T> input)
		{
			_Changed = true;
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_Input.Changed += this.Input_Changed;
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			_Output[0] = _Changed;
			_Changed = false;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(_Input != null)
				_Input.Changed -= this.Input_Changed;
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

	#region Derived DepectInput Node Classes

	[PluginInfo(Name = TEShared.Names.Nodes.DetectInput,
		Category = TEShared.Names.Categories.Value,
		Author = TEShared.Names.Author)]
	public sealed class DetectValueInput : DetectInputNode<double>
	{

	}

	[PluginInfo(Name = TEShared.Names.Nodes.DetectInput,
		Category = TEShared.Names.Categories.String,
		Author = TEShared.Names.Author)]
	public sealed class DetectStringInput : DetectInputNode<string>
	{

	}

	[PluginInfo(Name = TEShared.Names.Nodes.DetectInput,
		Category = TEShared.Names.Categories.Color,
		Author = TEShared.Names.Author)]
	public sealed class DetectColorInput : DetectInputNode<RGBAColor>
	{

	}

	#endregion

}
