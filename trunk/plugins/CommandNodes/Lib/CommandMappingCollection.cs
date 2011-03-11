using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region CommandMappingCollection<TKey>

	internal sealed class CommandMappingCollection<TKey> : IEnumerable<KeyValuePair<TKey, string[]>>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Dictionary<TKey, HashSet<string>> _Mappings;

		#endregion

		#region Properties

		public int KeyCount
		{
			get { return _Mappings.Count; }
		}

		public int MappingCount
		{
			get { return _Mappings.Values.Sum(x => x.Count); }
		}

		public int CommandCount
		{
			get { return _Mappings.Values.SelectMany(x => x).Distinct(CommandUtil.CommandIdComparer).Count(); }
		}

		public string[] this[TKey key]
		{
			get
			{
				HashSet<string> set;
				return _Mappings.TryGetValue(key, out set) ? set.ToArray() : CommandUtil.NoCommands;
			}
		}

		#endregion

		#region Constructors

		public CommandMappingCollection()
			: this(null)
		{
		}

		public CommandMappingCollection(IEqualityComparer<TKey> comparer)
		{
			_Mappings = new Dictionary<TKey, HashSet<string>>(comparer);
		}

		#endregion

		#region Methods

		public bool Add(TKey key, string commandId)
		{
			if(String.IsNullOrEmpty(commandId))
				return false;
			HashSet<string> commandIds;
			if(_Mappings.TryGetValue(key, out commandIds))
				return commandIds.Add(commandId);
			_Mappings.Add(key, new HashSet<string>(CommandUtil.CommandIdComparer) { commandId });
			return true;
		}

		public int Add(TKey key, IEnumerable<string> commandIds)
		{
			if(commandIds == null)
				return 0;
			HashSet<string> set;
			if(_Mappings.TryGetValue(key, out set))
			{
				var added = 0;
				foreach(var commandId in commandIds)
				{
					if(!String.IsNullOrEmpty(commandId) && set.Add(commandId))
						added++;
				}
				return added;
			}
			_Mappings.Add(key, set = new HashSet<string>(commandIds, CommandUtil.CommandIdComparer));
			return set.Count;
		}

		public bool ContainsKey(TKey key)
		{
			return _Mappings.ContainsKey(key);
		}

		public bool RemoveKey(TKey key)
		{
			return _Mappings.Remove(key);
		}

		public bool Remove(TKey key, string commandId)
		{
			HashSet<string> set;
			if(!_Mappings.TryGetValue(key, out set) || !set.Remove(commandId))
				return false;
			if(set.Count == 0)
				_Mappings.Remove(key);
			return true;
		}

		public int Remove(TKey key, IEnumerable<string> commandIds)
		{
			HashSet<string> set;
			if(!_Mappings.TryGetValue(key, out set))
				return 0;
			var removed = 0;
			foreach(var commandId in commandIds)
			{
				if(!String.IsNullOrEmpty(commandId) && set.Remove(commandId))
					removed++;
			}
			if(set.Count == 0)
				_Mappings.Remove(key);
			return removed;
		}

		public void Clear()
		{
			_Mappings.Clear();
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,string[]>> Members

		public IEnumerator<KeyValuePair<TKey, string[]>> GetEnumerator()
		{
			return _Mappings.Select(m => new KeyValuePair<TKey, string[]>(m.Key, m.Value.ToArray()))
							.GetEnumerator();
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
