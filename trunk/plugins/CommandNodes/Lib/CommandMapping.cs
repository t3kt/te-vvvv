using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region CommandTriggerType

	internal enum CommandTriggerType
	{
		None = 0,
		Keyboard = 0x1000000,
		Mouse = 0x2000000
	}

	#endregion

	#region CommandMapping

	internal sealed class CommandMapping
	{

		#region Static / Constant

		internal static CommandMapping Reconstruct(CommandTriggerType triggerType, int code)
		{
			switch(triggerType)
			{
			case CommandTriggerType.Keyboard:
				break;
			case CommandTriggerType.Mouse:
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			throw new NotImplementedException();
		}

		#endregion

		#region Fields

		private readonly CommandTriggerType _TriggerType;
		private readonly MouseButtons _MouseButtons;
		private readonly int _KeyCode;
		private readonly CommandModifiers _Modifiers;
		private readonly string _CommandId;
		private readonly int _Code;

		#endregion

		#region Properties

		public CommandTriggerType TriggerType
		{
			get { return _TriggerType; }
		}

		public MouseButtons MouseButtons
		{
			get { return _MouseButtons; }
		}

		public int KeyCode
		{
			get { return _KeyCode; }
		}

		public CommandModifiers Modifiers
		{
			get { return _Modifiers; }
		}

		public string CommandId
		{
			get { return _CommandId; }
		}

		public int Code
		{
			get { return _Code; }
		}

		#endregion

		#region Constructors

		public CommandMapping(int keyCode, CommandModifiers modifiers, string commandId)
		{
			_KeyCode = keyCode;
			_Modifiers = modifiers;
			_CommandId = commandId;
			_TriggerType = CommandTriggerType.Keyboard;
			_Code = CommandUtil.EncodeKeyCommand(keyCode, modifiers);
		}

		public CommandMapping(MouseButtons mouseButtons, CommandModifiers modifiers, string commandId)
		{
			_MouseButtons = mouseButtons;
			_Modifiers = modifiers;
			_CommandId = commandId;
			_TriggerType = CommandTriggerType.Mouse;
			_Code = CommandUtil.EncodeMouseCommand(mouseButtons, modifiers);
		}

		internal CommandMapping(CommandTriggerType triggerType, int code)
		{
			_TriggerType = triggerType;
			_Code = code;
		}

		#endregion

		#region Methods

		public override int GetHashCode()
		{
			//return _Code | (int)_TriggerType;
			return _Code;
		}

		#endregion

	}

	#endregion

}
