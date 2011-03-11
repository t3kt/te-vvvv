using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region CommandRegistry

	internal static class CommandRegistry
	{

		private static readonly CommandMappingCollection<int> _KeyMappings = new CommandMappingCollection<int>();

		private static readonly CommandMappingCollection<int> _MouseMappings = new CommandMappingCollection<int>();

		internal static int TotalMappingsCount
		{
			get { return _KeyMappings.MappingCount + _MouseMappings.MappingCount; }
		}

		public static void AddKeyMapping(int code, string commandId)
		{
			_KeyMappings.Add(code, commandId);
		}

		public static void AddKeyMappings(IEnumerable<KeyValuePair<int, string>> mappings)
		{
			if(mappings == null)
				return;
			foreach(var mapping in mappings)
				_KeyMappings.Add(mapping.Key, mapping.Value);
		}

		public static void AddMouseMapping(int code, string commandId)
		{
			_MouseMappings.Add(code, commandId);
		}

		public static void AddMouseMappings(IEnumerable<KeyValuePair<int, string>> mappings)
		{
			if(mappings == null)
				return;
			foreach(var mapping in mappings)
				_MouseMappings.Add(mapping.Key, mapping.Value);
		}

		public static void AddMapping(CommandMapping mapping)
		{
			if(mapping == null)
				return;
			switch(mapping.TriggerType)
			{
			case CommandTriggerType.Keyboard:
				_KeyMappings.Add(mapping.Code, mapping.CommandId);
				break;
			case CommandTriggerType.Mouse:
				_MouseMappings.Add(mapping.Code, mapping.CommandId);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static void ClearMappings()
		{
			_KeyMappings.Clear();
			_MouseMappings.Clear();
		}

		public static event EventHandler<CommandEventArgs> CommandTriggered;

		internal static void TriggerCommand(string commandId)
		{
			if(String.IsNullOrEmpty(commandId))
				return;
			var handler = CommandTriggered;
			if(handler != null)
				handler(null, new CommandEventArgs(commandId));
		}

		public static void TriggerKeyCommands(int code)
		{
			var commandIds = _KeyMappings[code];
			foreach(var commandId in commandIds)
				TriggerCommand(commandId);
		}

		public static void TriggerMouseCommands(int code)
		{
			var commandIds = _MouseMappings[code];
			foreach(var commandId in commandIds)
				TriggerCommand(commandId);
		}

		public static void TriggerKeyCommands(int keyCode, CommandModifiers modifiers)
		{
			TriggerKeyCommands(CommandUtil.EncodeKeyCommand(keyCode, modifiers));
		}

		public static void TriggerMouseCommands(MouseButtons buttons, CommandModifiers modifiers)
		{
			TriggerMouseCommands(CommandUtil.EncodeMouseCommand(buttons, modifiers));
		}

		public static void TriggerMouseCommands(bool left, bool middle, bool right, CommandModifiers modifiers)
		{
			TriggerMouseCommands(CommandUtil.EncodeMouseCommand(left, middle, right, modifiers));
		}

	}

	#endregion

}
