using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region CommandMappingSet

	[TESharedAnnotations.Incomplete]
	internal sealed class CommandMappingSet : IEnumerable<CommandMapping>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly List<CommandMapping> _AllMappings;
		private readonly Dictionary<int, HashSet<string>> _KeyboardCommands;
		private readonly Dictionary<int, HashSet<string>> _MouseCommands;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public CommandMappingSet()
		{
			_AllMappings = new List<CommandMapping>();
			_KeyboardCommands = new Dictionary<int, HashSet<string>>();
			_MouseCommands = new Dictionary<int, HashSet<string>>();
		}

		#endregion

		#region Methods

		private Dictionary<int, HashSet<string>> GetCommandLookup(CommandTriggerType triggerType)
		{
			switch(triggerType)
			{
			case CommandTriggerType.Keyboard:
				return _KeyboardCommands;
			case CommandTriggerType.Mouse:
				return _MouseCommands;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public string[] GetCommands(CommandTriggerType triggerType, int code)
		{
			var lookup = GetCommandLookup(triggerType);
			HashSet<string> cmds;
			return lookup.TryGetValue(code, out cmds) ? cmds.ToArray() : CommandUtil.NoCommands;
		}

		private HashSet<string> GetOrAddSet(CommandTriggerType triggerType, int code)
		{
			var lookup = GetCommandLookup(triggerType);
			HashSet<string> cmds;
			if(!lookup.TryGetValue(code, out cmds))
				lookup.Add(code, cmds = new HashSet<string>(CommandUtil.CommandIdComparer));
			return cmds;
		}

		public bool AddMapping(CommandMapping mapping)
		{
			if(mapping == null)
				return false;
			var cmds = GetOrAddSet(mapping.TriggerType, mapping.Code);
			if(!cmds.Add(mapping.CommandId))
				return false;
			_AllMappings.Add(mapping);
			return true;
		}

		public void Clear()
		{
			_AllMappings.Clear();
			_KeyboardCommands.Clear();
			_MouseCommands.Clear();
		}

		#endregion

		#region IEnumerable<CommandMapping> Members

		public IEnumerator<CommandMapping> GetEnumerator()
		{
			return _AllMappings.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

	}

	#endregion

}
