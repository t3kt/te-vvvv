using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Lib
{

	#region IStructTypeRegistration

	internal interface IStructTypeRegistration
	{
		Guid Id { get; }
		string PartTypesKey { get; }
		IList<StructPartType> PartTypes { get; }
		string FriendlyTypeName { get; }
		int UsageCount { get; }
	}

	#endregion

	#region StructTypeRegistry

	internal static class StructTypeRegistry
	{

		#region Nested Types

		#region TypeReg

		private sealed class TypeReg : IStructTypeRegistration
		{

			#region Static/Constant

			#endregion

			#region Fields

			public StructTypeDefinition TypeDefinition;
			public int UsageCount;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public TypeReg(StructTypeDefinition typeDefinition)
			{
				this.TypeDefinition = typeDefinition;
			}

			#endregion

			#region Methods

			#endregion

			#region IStructTypeRegistration Members

			public Guid Id
			{
				get { return this.TypeDefinition.Id; }
			}

			public string PartTypesKey
			{
				get { return this.TypeDefinition.PartTypesKey; }
			}

			public IList<StructPartType> PartTypes
			{
				get { return this.TypeDefinition.PartTypes; }
			}

			public string FriendlyTypeName
			{
				get { return this.FriendlyTypeName; }
			}

			int IStructTypeRegistration.UsageCount
			{
				get { return this.UsageCount; }
			}

			#endregion

		}

		#endregion

		#region TypeRegIdSet

		private sealed class TypeRegIdSet : KeyedCollection<Guid, TypeReg>
		{

			#region Static/Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			protected override Guid GetKeyForItem(TypeReg item)
			{
				return item.Id;
			}

			#endregion

		}

		#endregion

		#region TypeRegKeySet

		private sealed class TypeRegKeySet : KeyedCollection<string, TypeReg>
		{

			#region Static/Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public TypeRegKeySet()
				: base(StringComparer.OrdinalIgnoreCase)
			{

			}

			#endregion

			#region Methods

			protected override string GetKeyForItem(TypeReg item)
			{
				return item.PartTypesKey;
			}

			#endregion

		}

		#endregion

		#region RegistryCore

		private abstract class RegistryCore
		{

			#region Static/Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			public abstract ICollection<StructTypeDefinition> RegisteredTypes { get; }

			#endregion

			#region Events

			public event EventHandler<StructTypeEventArgs> TypeRegistered;

			public event EventHandler<StructTypeEventArgs> TypeUnregistered;

			public abstract event EventHandler<CountChangedEventArgs<Guid>> TypeUsageCountChanged;

			#endregion

			#region Constructors

			#endregion

			#region Methods

			protected void OnTypeRegistered(StructTypeDefinition type)
			{
				var handler = this.TypeRegistered;
				if(handler != null)
					handler(null, new StructTypeEventArgs(type));
				Log(TLogType.Debug, "Struct Type Registered: {0}", type);
			}

			protected void OnTypeUnregistered(StructTypeDefinition type)
			{
				var handler = this.TypeUnregistered;
				if(handler != null)
					handler(null, new StructTypeEventArgs(type));
				Log(TLogType.Debug, "Struct Type Unregistered: {0}", type);
			}

			public abstract StructTypeDefinition GetTypeDefinition(Guid id);

			public abstract int GetTypeUsageCount(Guid id);

			public abstract StructTypeDefinition RequestTypeDefinition(string partTypesKey);

			public abstract bool ReleaseTypeDefinition(Guid id);

			#endregion

		}

		#endregion

		#region OldRegistryCore

		private sealed class OldRegistryCore : RegistryCore
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly Dictionary<Guid, StructTypeDefinition> _TypesById = new Dictionary<Guid, StructTypeDefinition>();
			private readonly Dictionary<string, StructTypeDefinition> _TypesByKey = new Dictionary<string, StructTypeDefinition>();
			private readonly CountDictionary<Guid> _TypeUsageCounts = new CountDictionary<Guid>();

			#endregion

			#region Properties

			public override ICollection<StructTypeDefinition> RegisteredTypes
			{
				get { return _TypesByKey.Values; }
			}

			#endregion

			#region Events

			public override event EventHandler<CountChangedEventArgs<Guid>> TypeUsageCountChanged
			{
				add { _TypeUsageCounts.CountChanged += value; }
				remove { _TypeUsageCounts.CountChanged -= value; }
			}

			#endregion

			#region Constructors

			#endregion

			#region Methods

			public override StructTypeDefinition GetTypeDefinition(Guid id)
			{
				StructTypeDefinition definition;
				return _TypesById.TryGetValue(id, out definition) ? definition : null;
			}

			public override int GetTypeUsageCount(Guid id)
			{
				return _TypeUsageCounts.GetCount(id);
			}

			public override StructTypeDefinition RequestTypeDefinition(string partTypesKey)
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

			public override bool ReleaseTypeDefinition(Guid id)
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

			#endregion

		}

		#endregion

		#endregion

		private static IPluginHost _Host;
		private static RegistryCore _Core;

		private static readonly Dictionary<Guid, StructTypeDefinition> _TypesById = new Dictionary<Guid, StructTypeDefinition>();
		private static readonly Dictionary<string, StructTypeDefinition> _TypesByKey = new Dictionary<string, StructTypeDefinition>();
		private static readonly CountDictionary<Guid> _TypeUsageCounts = new CountDictionary<Guid>();

		static StructTypeRegistry()
		{
			_Core = new OldRegistryCore();
		}

		[Conditional("DEBUG")]
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

		internal static event EventHandler<StructTypeEventArgs> TypeRegistered
		{
			add { _Core.TypeRegistered += value; }
			remove { _Core.TypeRegistered -= value; }
		}

		internal static event EventHandler<StructTypeEventArgs> TypeUnregistered
		{
			add { _Core.TypeUnregistered += value; }
			remove { _Core.TypeUnregistered -= value; }
		}

		internal static event EventHandler<CountChangedEventArgs<Guid>> TypeUsageCountChanged
		{
			add { _Core.TypeUsageCountChanged += value; }
			remove { _Core.TypeUsageCountChanged -= value; }
		}

		internal static ICollection<StructTypeDefinition> RegisteredTypes
		{
			get { return _Core.RegisteredTypes; }
		}

		internal static StructTypeDefinition GetTypeDefinition(Guid id)
		{
			return _Core.GetTypeDefinition(id);
		}

		internal static int GetTypeUsageCount(Guid id)
		{
			return _Core.GetTypeUsageCount(id);
		}

		internal static int GetTypeUsageCount(StructTypeDefinition type)
		{
			return type == null ? 0 : GetTypeUsageCount(type.Id);
		}

		internal static StructTypeDefinition RequestTypeDefinition(string partTypesKey)
		{
			return _Core.RequestTypeDefinition(partTypesKey);
		}

		internal static bool ReleaseTypeDefinition(Guid id)
		{
			return _Core.ReleaseTypeDefinition(id);
		}

		internal static bool ReleaseTypeDefinition(StructTypeDefinition type)
		{
			return type != null && ReleaseTypeDefinition(type.Id);
		}

	}

	#endregion

}
