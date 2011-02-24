using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Lib
{

	#region StructTypeRegistry

	internal static class StructTypeRegistry
	{

		private static readonly Dictionary<Guid, StructTypeDefinition> _TypesById;
		private static readonly Dictionary<string, StructTypeDefinition> _TypesByKey;
		private static readonly CountDictionary<Guid> _TypeUsageCounts;
		private static IPluginHost _Host;

		static StructTypeRegistry()
		{
			_TypesById = new Dictionary<Guid, StructTypeDefinition>();
			_TypesByKey = new Dictionary<string, StructTypeDefinition>();
			_TypeUsageCounts = new CountDictionary<Guid>();
			_TypeUsageCounts.CountChanged += (s, e) => Log(TLogType.Debug, "Struct Type Usage Count Changed: key={0} count={1}", e.Key, e.Count);
		}

		internal static event EventHandler<StructTypeEventArgs> TypeRegistered;

		internal static event EventHandler<StructTypeEventArgs> TypeUnregistered;

		internal static event EventHandler<CountChangedEventArgs<Guid>> TypeUsageCountChanged
		{
			add { _TypeUsageCounts.CountChanged += value; }
			remove { _TypeUsageCounts.CountChanged -= value; }
		}

		private static void Log(TLogType logType, string message, params object[] args)
		{
			if(_Host != null)
			{
				if(args != null && args.Length > 0)
					message = String.Format(message, args);
				_Host.Log(logType, message);
			}
		}

		internal static void OfferHost(IPluginHost host)
		{
			if(_Host == null)
				_Host = host;
		}

		private static void OnTypeRegistered(StructTypeDefinition type)
		{
			var handler = TypeRegistered;
			if(handler != null)
				handler(null, new StructTypeEventArgs(type));
			Log(TLogType.Debug, "Struct Type Registered: {0}", type);
		}

		private static void OnTypeUnregistered(StructTypeDefinition type)
		{
			var handler = TypeUnregistered;
			if(handler != null)
				handler(null, new StructTypeEventArgs(type));
			Log(TLogType.Debug, "Struct Type Unregistered: {0}", type);
		}

		internal static ICollection<StructTypeDefinition> RegisteredTypes
		{
			get { return _TypesByKey.Values; }
		}

		internal static StructTypeDefinition GetTypeDefinition(Guid id)
		{
			StructTypeDefinition definition;
			return _TypesById.TryGetValue(id, out definition) ? definition : null;
		}

		internal static int GetTypeUsageCount(Guid id)
		{
			return _TypeUsageCounts.GetCount(id);
		}

		internal static int GetTypeUsageCount(StructTypeDefinition type)
		{
			return type == null ? 0 : _TypeUsageCounts.GetCount(type.Id);
		}

		internal static StructTypeDefinition RequestTypeDefinition(string partTypesKey)
		{
			Log(TLogType.Debug, "Requesting type: {0}...", partTypesKey);
			var filteredKey = StructTypeDefinition.FilterPartTypesKey(partTypesKey);
			if(String.IsNullOrEmpty(filteredKey))
				return null;
			StructTypeDefinition definition;
			if(!_TypesByKey.TryGetValue(filteredKey, out definition))
			{
				definition = new StructTypeDefinition(filteredKey);
				_TypesByKey.Add(filteredKey, definition);
				_TypesById.Add(definition.Id, definition);
				OnTypeRegistered(definition);
			}
			_TypeUsageCounts.IncrementCount(definition.Id);
			return definition;
		}

		internal static bool ReleaseTypeDefinition(Guid id)
		{
			Log(TLogType.Debug, "Releasing type: {0}...", (object)GetTypeDefinition(id) ?? "(null/not found)");
			if(_TypeUsageCounts.DecrementCount(id))
			{
				StructTypeDefinition definition;
				if(_TypesById.TryGetValue(id, out definition))
				{
					_TypesById.Remove(id);
					_TypesByKey.Remove(definition.PartTypesKey);
					OnTypeUnregistered(definition);
				}
				return true;
			}
			return false;
		}

		internal static bool ReleaseTypeDefinition(StructTypeDefinition type)
		{
			return type != null && ReleaseTypeDefinition(type.Id);
		}

	}

	#endregion

}
