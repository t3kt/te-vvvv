using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;

namespace VVVV.Lib
{

	#region StructTypeRegistry

	internal static class StructTypeRegistry
	{

		#region Nested Types

		#region TypeReg

		private sealed class TypeReg
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

		}

		#endregion

		#region RegistryCore

		private sealed class RegistryCore
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly Dictionary<Guid, TypeReg> _RegById = new Dictionary<Guid, TypeReg>();
			private readonly Dictionary<string, TypeReg> _RegByKey = new Dictionary<string, TypeReg>(StringComparer.OrdinalIgnoreCase);

			#endregion

			#region Properties

			public IEnumerable<StructTypeDefinition> RegisteredTypes
			{
				get
				{
					return from reg in this._RegByKey
						   select reg.Value.TypeDef;
				}
			}

			#endregion

			#region Events

			public event EventHandler<StructTypeEventArgs> TypeRegistered;

			public event EventHandler<StructTypeEventArgs> TypeUnregistered;

			public event EventHandler<CountChangedEventArgs<Guid>> TypeUsageCountChanged;

			#endregion

			#region Constructors

			#endregion

			#region Methods

			private void OnTypeRegistered(StructTypeDefinition type)
			{
				Log("Struct Type Registered: {0}", type);
				var handler = this.TypeRegistered;
				if(handler != null)
					handler(null, new StructTypeEventArgs(type));
			}

			private void OnTypeUnregistered(StructTypeDefinition type)
			{
				Log("Struct Type Unregistered: {0}", type);
				var handler = this.TypeUnregistered;
				if(handler != null)
					handler(null, new StructTypeEventArgs(type));
			}

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

			private StructTypeDefinition GetTypeDefinition(Guid id)
			{
				TypeReg reg;
				return this._RegById.TryGetValue(id, out reg) ? reg.TypeDef : null;
			}

			public int GetTypeUsageCount(Guid id)
			{
				TypeReg reg;
				return this._RegById.TryGetValue(id, out reg) ? reg.Usages : 0;
			}

			public StructTypeDefinition RequestTypeDefinition(string partTypesKey)
			{
				Log("Requesting type: {0}...", partTypesKey);
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

			public bool ReleaseTypeDefinition(Guid id)
			{
				Log("Releasing type: {0}...", (object)this.GetTypeDefinition(id) ?? "(null/not found)");
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

		private static readonly RegistryCore _Core = new RegistryCore();
		private static ILogger _Logger;
		private static int _LoggerProviderCount;

		[Conditional("DEBUG")]
		private static void Log(LogType logType, string message, params object[] args)
		{
			if(_LoggerProviderCount > 0 && _Logger != null)
				_Logger.Log(logType, message ?? String.Empty, args ?? new object[0]);
		}

		[Conditional("DEBUG")]
		private static void Log(string message, params object[] args)
		{
			Log(LogType.Debug, message, args);
		}

		internal static bool OfferLogger(ILogger logger)
		{
			//if(logger == null)
			//    return false;
			if(_Logger == null)
				_Logger = logger;
			_LoggerProviderCount++;
			return true;
		}

		internal static void RescindLogger()
		{
			_LoggerProviderCount--;
			if(_LoggerProviderCount <= 0)
				_Logger = null;
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

		internal static void ReleaseTypeDefinition(Guid id)
		{
			_Core.ReleaseTypeDefinition(id);
		}

		internal static void ReleaseTypeDefinition(StructTypeDefinition type)
		{
			if(type != null)
				ReleaseTypeDefinition(type.Id);
		}

		#endregion

	}

	#endregion

}
