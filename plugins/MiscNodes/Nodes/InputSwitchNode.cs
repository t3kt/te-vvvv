using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

namespace VVVV.Nodes
{

	#region InputSwitchNode<T>

	public abstract class InputSwitchNode<T> : IPluginEvaluate
	{

		#region Static/Constant

		#endregion

		#region Fields

		[Input("Switch", MinValue = 0, DefaultValue = 0)]
		private ISpread<int> _SwitchInput;

		[Input("Input", IsPinGroup = true)]
		private ISpread<ISpread<T>> _Inputs;

		[Output("Output")]
		private ISpread<T> _Output;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			var count = Math.Max(_SwitchInput.SliceCount, _SwitchInput.Select(s => _Inputs[s].SliceCount).Max());
			_Output.SliceCount = count;
			for(var i = 0; i < count; i++)
			{
				_Output[i] = _Inputs[_SwitchInput[i]][i];
			}
		}

		#endregion

	}

	#endregion

	#region Derived Switch Node Classes

	//[PluginInfo(Name = TEShared.Names.Nodes.Switch,
	//    Category = TEShared.Names.Categories.Value,
	//    Version = "foo",
	//    Author = TEShared.Names.Author)]
	//public class ValueInputSwitchNode : InputSwitchNode<double>
	//{
	//}

	[PluginInfo(Name = TEShared.Names.Nodes.Switch,
		Category = TEShared.Names.Categories.Boolean,
		Version = TEShared.Names.Versions.Input,
		Author = TEShared.Names.Author)]
	public class BooleanInputSwitchNode : InputSwitchNode<bool>
	{
	}

	[PluginInfo(Name = TEShared.Names.Nodes.Switch,
		Category = TEShared.Names.Categories.Transform,
		Version = TEShared.Names.Versions.Input,
		Author = TEShared.Names.Author)]
	public class TransformInputSwitchNode : InputSwitchNode<Matrix4x4>
	{
	}

	[PluginInfo(Name = TEShared.Names.Nodes.Switch,
		Category = TEShared.Names.Categories.Vector,
		Version = TEShared.Names.Versions.Input + TEShared.Names.AND + TEShared.Names.Versions.TwoD,
		Author = TEShared.Names.Author)]
	public class Vector2DInputSwitchNode : InputSwitchNode<Vector2D>
	{
	}

	[PluginInfo(Name = TEShared.Names.Nodes.Switch,
		Category = TEShared.Names.Categories.Vector,
		Version = TEShared.Names.Versions.Input + TEShared.Names.AND + TEShared.Names.Versions.ThreeD,
		Author = TEShared.Names.Author)]
	public class Vector3DInputSwitchNode : InputSwitchNode<Vector3D>
	{
	}

	[PluginInfo(Name = TEShared.Names.Nodes.Switch,
		Category = TEShared.Names.Categories.Vector,
		Version = TEShared.Names.Versions.Input + TEShared.Names.AND + TEShared.Names.Versions.FourD,
		Author = TEShared.Names.Author)]
	public class Vector4DInputSwitchNode : InputSwitchNode<Vector4D>
	{
	}

	// DOESN'T WORK!!!!!!!!!!!!!!!!!!
	[PluginInfo(Name = TEShared.Names.Nodes.Switch,
		Category = TEShared.Names.Categories.Enumerations,
		Version = TEShared.Names.Versions.Input,
		Author = TEShared.Names.Author)]
	public class EnumInputSwitchNode : InputSwitchNode<EnumEntry>
	{
	}

	#endregion

}
