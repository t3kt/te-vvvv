using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region JoinModifiersNode

	[Obsolete]
	[PluginInfo(Name = TEShared.Names.Nodes.Modifiers,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Join,
		Author = TEShared.Names.Author,
		Warnings = TEShared.Names.Warnings.Obsolete)]
	public sealed class JoinModifiersNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Control")]
		private IDiffSpread<bool> _ControlInput;

		[Input("Shift")]
		private IDiffSpread<bool> _ShiftInput;

		[Input("Alt")]
		private IDiffSpread<bool> _AltInput;

		[Output("Modifiers")]
		private ISpread<Keys> _ModifiersOutput;

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
			if(_ControlInput.IsChanged || _ShiftInput.IsChanged || _AltInput.IsChanged)
			{
				var count =
					_ModifiersOutput.SliceCount =
					Math.Max(_ControlInput.SliceCount, Math.Max(_ShiftInput.SliceCount, _AltInput.SliceCount));
				for(var i = 0; i < count; i++)
				{
					_ModifiersOutput[i] = KeyEventInfo.MakeModifiers(_ControlInput[i], _ShiftInput[i], _AltInput[i]);
				}
			}
		}

		#endregion
	}

	#endregion

}
