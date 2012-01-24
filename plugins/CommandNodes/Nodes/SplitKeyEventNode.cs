using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region SplitKeyEventNode

	[Obsolete]
	[PluginInfo(Name = CommandNames.Nodes.KeyEvent,
		Category = CommandNames.Categories.Command,
		Version = TEShared.Names.Versions.Split,
		Author = TEShared.Names.Author,
		Warnings = TEShared.Names.Warnings.Obsolete)]
	public sealed class SplitKeyEventNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("KeyEvent")]
		private IDiffSpread<KeyEventInfo> _KeyEventInput;

		[Output("KeyCode")]
		private ISpread<Keys> _KeyCodeOutput;

		[Output("KeyData")]
		private ISpread<Keys> _KeyDataOutput;

		[Output("KeyValue")]
		private ISpread<int> _KeyValueOutput;

		[Output("Control")]
		private ISpread<bool> _ControlOutput;

		[Output("Shift")]
		private ISpread<bool> _ShiftOutput;

		[Output("Alt")]
		private ISpread<bool> _AltOutput;

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
			if(_KeyEventInput.IsChanged)
			{
				var count =
					_KeyCodeOutput.SliceCount =
					_KeyDataOutput.SliceCount =
					_KeyValueOutput.SliceCount =
					_ControlOutput.SliceCount =
					_ShiftOutput.SliceCount =
					_AltOutput.SliceCount =
					_ModifiersOutput.SliceCount = _KeyEventInput.SliceCount;
				for(var i = 0; i < count; i++)
				{
					var evt = _KeyEventInput[i];
					if(evt == null)
					{
						_KeyCodeOutput[i] = _KeyDataOutput[i] = _ModifiersOutput[i] = default(Keys);
						_KeyValueOutput[i] = -1;
						_ControlOutput[i] = _ShiftOutput[i] = _AltOutput[i] = false;
					}
					else
					{
						_KeyCodeOutput[i] = evt.KeyCode;
						_KeyDataOutput[i] = evt.KeyData;
						_KeyValueOutput[i] = evt.KeyValue;
						_ControlOutput[i] = evt.Control;
						_ShiftOutput[i] = evt.Shift;
						_AltOutput[i] = evt.Alt;
						_ModifiersOutput[i] = evt.Modifiers;
					}
				}
			}
		}

		#endregion
	}

	#endregion

}
