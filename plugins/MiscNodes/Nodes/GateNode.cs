using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

namespace VVVV.Nodes
{

	#region GateNode<T>

	public abstract class GateNode<T> : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Gate", DefaultValue = 1)]
		private IDiffSpread<bool> _GateInput;

		[Input("Input")]
		private ISpread<T> _ValueInput;

		[Output("Output")]
		private ISpread<T> _ValueOutput;

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
			//if(_GateInput.IsChanged)
			{
				if(_GateInput.Contains(true))
				{
					var count = Math.Max(_ValueInput.SliceCount, _GateInput.SliceCount);
					// ReSharper disable RedundantCheckBeforeAssignment
					if(_ValueOutput.SliceCount != count)
						_ValueOutput.SliceCount = count;
					// ReSharper restore RedundantCheckBeforeAssignment
					for(var i = 0; i < count; i++)
					{
						if(_GateInput[i])
							_ValueOutput[i] = _ValueInput[i];
					}
				}
			}
		}

		#endregion

	}

	#endregion

	#region Derived Gate Node Classes

	[PluginInfo(Name = TEShared.Names.Nodes.Gate,
		Category = TEShared.Names.Categories.Value,
		Author = TEShared.Names.Author)]
	public sealed class ValueGateNode : GateNode<double>
	{

	}

	[PluginInfo(Name = TEShared.Names.Nodes.Gate,
		Category = TEShared.Names.Categories.String,
		Author = TEShared.Names.Author)]
	public sealed class StringGateNode : GateNode<string>
	{

	}

	[PluginInfo(Name = TEShared.Names.Nodes.Gate,
		Category = TEShared.Names.Categories.Color,
		Author = TEShared.Names.Author)]
	public sealed class ColorGateNode : GateNode<RGBAColor>
	{

	}

	[PluginInfo(Name = TEShared.Names.Nodes.Gate,
		Category = TEShared.Names.Categories.Transform,
		Author = TEShared.Names.Author)]
	public sealed class TransformGateNode : GateNode<Matrix4x4>
	{

	}

	[PluginInfo(Name = TEShared.Names.Nodes.Gate,
		Category = TEShared.Names.Categories.Vector,
		Version = TEShared.Names.Versions.TwoD,
		Author = TEShared.Names.Author)]
	public sealed class Vector2DGateNode : GateNode<Vector2D>
	{

	}

	[PluginInfo(Name = TEShared.Names.Nodes.Gate,
		Category = TEShared.Names.Categories.Vector,
		Version = TEShared.Names.Versions.ThreeD,
		Author = TEShared.Names.Author)]
	public sealed class Vector3DGateNode : GateNode<Vector3D>
	{

	}

	[PluginInfo(Name = TEShared.Names.Nodes.Gate,
		Category = TEShared.Names.Categories.Vector,
		Version = TEShared.Names.Versions.FourD,
		Author = TEShared.Names.Author)]
	public sealed class Vector4DGateNode : GateNode<Vector4D>
	{

	}

	#endregion

}
