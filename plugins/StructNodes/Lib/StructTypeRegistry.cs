//#define USE_ALT_CORE
#define USE_ALT2_CORE
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

		#region RegistryCore

		private abstract class RegistryCore
		{

			#region Static/Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			public abstract IEnumerable<StructTypeDefinition> RegisteredTypes { get; }

			public abstract IEnumerable<IStructTypeRegistration> Registrations { get; }

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

			public override IEnumerable<StructTypeDefinition> RegisteredTypes
			{
				get { return _TypesByKey.Values; }
			}

			public override IEnumerable<IStructTypeRegistration> Registrations
			{
				get
				{
					return from entry in _TypesByKey
						   select (IStructTypeRegistration)new TypeReg(entry.Value) { Usages = GetTypeUsageCount(entry.Value.Id) };
				}
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

		#region TypeReg

		private sealed class TypeReg : IStructTypeRegistration
		{

			#region Fields

			public readonly StructTypeDefinition TypeDef;
			public int Usages;

			#endregion

			#region Constructors

			public TypeReg(StructTypeDefinition typeDef)
			{
				this.TypeDef = typeDef;
			}

			#endregion

			#region Methods

			public bool Decrement()
			{
				this.Usages--;
				return this.Usages <= 0;
			}

			#endregion

			#region IStructTypeRegistration Members

			Guid IStructTypeRegistration.Id
			{
				get { return this.TypeDef.Id; }
			}

			string IStructTypeRegistration.PartTypesKey
			{
				get { return this.TypeDef.PartTypesKey; }
			}

			IList<StructPartType> IStructTypeRegistration.PartTypes
			{
				get { return this.TypeDef.PartTypes; }
			}

			string IStructTypeRegistration.FriendlyTypeName
			{
				get { return this.TypeDef.FriendlyTypeName; }
			}

			int IStructTypeRegistration.UsageCount
			{
				get { return this.Usages; }
			}

			#endregion

		}

		#endregion

		#region AltRegistryCore

		private sealed class AltRegistryCore : RegistryCore
		{

			#region Nested Types

			#region TypeRegSet<TKey>

			private abstract class TypeRegSet<TKey> : Collection<TypeReg>
			{

				#region Static/Constant

				#endregion

				#region Fields

				private readonly Dictionary<TKey, TypeReg> _Dict;

				#endregion

				#region Properties

				#endregion

				#region Constructors

				protected TypeRegSet(IEqualityComparer<TKey> comparer)
				{
					_Dict = new Dictionary<TKey, TypeReg>(comparer);
				}

				#endregion

				#region Methods

				protected abstract TKey GetKeyForItem(TypeReg reg);

				protected override void ClearItems()
				{
					_Dict.Clear();
					base.ClearItems();
				}

				protected override void InsertItem(int index, TypeReg item)
				{
					var key = GetKeyForItem(item);
					_Dict.Add(key, item);
					base.InsertItem(index, item);
				}

				protected override void RemoveItem(int index)
				{
					var key = GetKeyForItem(base[index]);
					throw new NotImplementedException();
					base.RemoveItem(index);
				}

				public bool TryGetValue(TKey key, out TypeReg reg)
				{
					return _Dict.TryGetValue(key, out reg);
				}

				#endregion

			}

			#endregion

			#region TypeRegIdSet

			private sealed class TypeRegIdSet : KeyedCollection<Guid, TypeReg>
			{

				public TypeRegIdSet()
					: base(null, 0) { }

				protected override Guid GetKeyForItem(TypeReg item)
				{
					return item.TypeDef.Id;
				}

				public bool TryGetValue(Guid key, out TypeReg value)
				{
					return this.Dictionary.TryGetValue(key, out value);
				}

			}

			#endregion

			#region TypeRegKeySet

			private sealed class TypeRegKeySet : KeyedCollection<string, TypeReg>
			{

				public TypeRegKeySet()
					: base(StringComparer.OrdinalIgnoreCase, 0) { }

				protected override string GetKeyForItem(TypeReg item)
				{
					return item.TypeDef.PartTypesKey;
				}

				public bool TryGetValue(string key, out TypeReg value)
				{
					return this.Dictionary.TryGetValue(key, out value);
				}

			}

			#endregion

			#endregion

			#region Static/Constant

			#endregion

			#region Fields

			private readonly TypeRegIdSet _RegById = new TypeRegIdSet();
			private readonly TypeRegKeySet _RegByKey = new TypeRegKeySet();

			#endregion

			#region Properties

			public override IEnumerable<StructTypeDefinition> RegisteredTypes
			{
				get
				{
					return from reg in this._RegById
						   select reg.TypeDef;
				}
			}

			public override IEnumerable<IStructTypeRegistration> Registrations
			{
				get { return _RegByKey.Cast<IStructTypeRegistration>(); }
			}

			#endregion

			#region Events

			public override event EventHandler<CountChangedEventArgs<Guid>> TypeUsageCountChanged;

			#endregion

			#region Constructors

			#endregion

			#region Methods

			private void OnTypeUsageCountChanged(Guid id, int count)
			{
				var handler = this.TypeUsageCountChanged;
				if(handler != null)
					handler(this, new CountChangedEventArgs<Guid>(id, count));
			}

			private void AddRegistration(TypeReg reg)
			{
				this._RegById.Add(reg);
				this._RegByKey.Add(reg);
				this.OnTypeRegistered(reg.TypeDef);
			}

			private void RemoveRegistration(TypeReg reg)
			{
				this._RegById.Remove(reg);
				this._RegByKey.Remove(reg);
				this.OnTypeUnregistered(reg.TypeDef);
			}

			public override StructTypeDefinition GetTypeDefinition(Guid id)
			{
				TypeReg reg;
				return this._RegById.TryGetValue(id, out reg) ? reg.TypeDef : null;
			}

			public override int GetTypeUsageCount(Guid id)
			{
				TypeReg reg;
				return this._RegById.TryGetValue(id, out reg) ? reg.Usages : 0;
			}

			public override StructTypeDefinition RequestTypeDefinition(string partTypesKey)
			{
				Log(TLogType.Debug, "Requesting type: {0}...", partTypesKey);
				var filteredKey = StructTypeDefinition.FilterPartTypesKey(partTypesKey);
				if(String.IsNullOrEmpty(filteredKey))
					return null;
				TypeReg reg;
				if(!this._RegByKey.TryGetValue(filteredKey, out reg))
				{
					reg = new TypeReg(new StructTypeDefinition(filteredKey));
					this.AddRegistration(reg);
				}
				reg.Usages++;
				this.OnTypeUsageCountChanged(reg.TypeDef.Id, reg.Usages);
				return reg.TypeDef;
			}

			public override bool ReleaseTypeDefinition(Guid id)
			{
				Log(TLogType.Debug, "Releasing type: {0}...", (object)this.GetTypeDefinition(id) ?? "(null/not found)");
				TypeReg reg;
				if(!this._RegById.TryGetValue(id, out reg) || !reg.Decrement())
					return false;
				this.RemoveRegistration(reg);
				this.OnTypeUsageCountChanged(reg.TypeDef.Id, 0);
				return true;
			}

			#endregion

		}

		#endregion

		#region AltRegistryCore2

		private sealed class AltRegistryCore2 : RegistryCore
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly Dictionary<Guid, TypeReg> _RegById = new Dictionary<Guid, TypeReg>();
			private readonly Dictionary<string, TypeReg> _RegByKey = new Dictionary<string, TypeReg>(StringComparer.OrdinalIgnoreCase);

			#endregion

			#region Properties

			public override IEnumerable<StructTypeDefinition> RegisteredTypes
			{
				get
				{
					return from reg in this._RegByKey
						   select reg.Value.TypeDef;
				}
			}

			public override IEnumerable<IStructTypeRegistration> Registrations
			{
				get { return _RegByKey.Values.Cast<IStructTypeRegistration>(); }
			}

			#endregion

			#region Events

			public override event EventHandler<CountChangedEventArgs<Guid>> TypeUsageCountChanged;

			#endregion

			#region Constructors

			#endregion

			#region Methods

			private void OnTypeUsageCountChanged(Guid id, int count)
			{
				var handler = this.TypeUsageCountChanged;
				if(handler != null)
					handler(this, new CountChangedEventArgs<Guid>(id, count));
			}

			private void AddRegistration(TypeReg reg)
			{
				this._RegById.Add(reg.TypeDef.Id, reg);
				this._RegByKey.Add(reg.TypeDef.PartTypesKey, reg);
				this.OnTypeRegistered(reg.TypeDef);
			}

			private void RemoveRegistration(TypeReg reg)
			{
				this._RegById.Remove(reg.TypeDef.Id);
				this._RegByKey.Remove(reg.TypeDef.PartTypesKey);
				this.OnTypeUnregistered(reg.TypeDef);
			}

			public override StructTypeDefinition GetTypeDefinition(Guid id)
			{
				TypeReg reg;
				return this._RegById.TryGetValue(id, out reg) ? reg.TypeDef : null;
			}

			public override int GetTypeUsageCount(Guid id)
			{
				TypeReg reg;
				return this._RegById.TryGetValue(id, out reg) ? reg.Usages : 0;
			}

			public override StructTypeDefinition RequestTypeDefinition(string partTypesKey)
			{
				Log(TLogType.Debug, "Requesting type: {0}...", partTypesKey);
				var filteredKey = StructTypeDefinition.FilterPartTypesKey(partTypesKey);
				if(String.IsNullOrEmpty(filteredKey))
					return null;
				TypeReg reg;
				if(!this._RegByKey.TryGetValue(filteredKey, out reg))
				{
					reg = new TypeReg(new StructTypeDefinition(filteredKey));
					this.AddRegistration(reg);
				}
				reg.Usages++;
				this.OnTypeUsageCountChanged(reg.TypeDef.Id, reg.Usages);
				return reg.TypeDef;
			}

			public override bool ReleaseTypeDefinition(Guid id)
			{
				Log(TLogType.Debug, "Releasing type: {0}...", (object)this.GetTypeDefinition(id) ?? "(null/not found)");
				TypeReg reg;
				if(!this._RegById.TryGetValue(id, out reg) || !reg.Decrement())
					return false;
				this.RemoveRegistration(reg);
				this.OnTypeUsageCountChanged(reg.TypeDef.Id, 0);
				return true;
			}

			#endregion

		}

		#endregion

		#endregion

		#region Static/Constant

		private static IPluginHost _Host;
		private static readonly RegistryCore _Core;

		static StructTypeRegistry()
		{
#if USE_ALT_CORE
			_Core = new AltRegistryCore();
#elif USE_ALT2_CORE
			_Core = new AltRegistryCore2();
#else
			_Core = new OldRegistryCore();
#endif
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
			Debug.Assert(host != null);
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

		internal static IEnumerable<StructTypeDefinition> RegisteredTypes
		{
			get { return _Core.RegisteredTypes; }
		}

		internal static IEnumerable<IStructTypeRegistration> Registrations
		{
			get { return _Core.Registrations; }
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

		#endregion

	}

	#endregion

}
