using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Animator.Common;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Runtime
{

	#region RegisteredImplementationAttribute

	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class RegisteredImplementationAttribute : Attribute
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly Type _BaseType;

		private readonly string _Key;
		private readonly bool _IsDefault;

		private readonly string _ImplementingTypeName;
		private readonly Type _ImplementingType;

		#endregion

		#region Properties

		public Type BaseType
		{
			get { return this._BaseType; }
		}

		public string Key
		{
			get { return this._Key; }
		}

		public string ImplementingTypeName
		{
			get { return this._ImplementingTypeName; }
		}

		public Type ImplementingType
		{
			get { return this._ImplementingType; }
		}

		public bool IsDefault
		{
			get { return this._IsDefault; }
		}

		#endregion

		#region Constructors

		public RegisteredImplementationAttribute(Type baseType, string key, Type implementingType)
		{
			this._BaseType = baseType;
			this._Key = key;
			this._ImplementingType = implementingType;
		}

		public RegisteredImplementationAttribute(Type baseType, string key, string implementingTypeName)
		{
			this._BaseType = baseType;
			this._Key = key;
			this._ImplementingTypeName = implementingTypeName;
		}

		internal RegisteredImplementationAttribute(Type baseType, string implementatingTypeName)
		{
			this._BaseType = baseType;
			this._IsDefault = true;
			this._ImplementingTypeName = implementatingTypeName;
		}

		internal RegisteredImplementationAttribute(Type baseType, Type implementingType)
		{
			this._BaseType = baseType;
			this._IsDefault = true;
			this._ImplementingType = implementingType;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IImplementationRegistry

	public interface IImplementationRegistry
	{
		void RegisterType([CanBeNull] string key, [NotNull] Type type);
		void RegisterTypes([NotNull] Assembly assembly);
		IEnumerable<KeyValuePair<Type, string>> GetRegisteredTypeDescriptions();
		IEnumerable<KeyValuePair<string, string>> GetRegisteredTypeDescriptionsByKey();
	}

	#endregion

	#region ImplementationRegistry<TBase>

	internal sealed class ImplementationRegistry<TBase> : IImplementationRegistry
	{

		#region Static / Constant

		private static readonly Type _BaseType = typeof(TBase);

		private static void AssertValid(Type type)
		{
			if(!_BaseType.IsAssignableFrom(type))
				throw new NotSupportedException(String.Format("Type does not implement {0}: '{1}'", _BaseType, type));
		}

		#endregion

		#region Fields

		private readonly Dictionary<string, Type> _Types;
		private readonly HashSet<string> _RegisteredAssemblies;
		private Type _DefaultType;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		internal ImplementationRegistry()
		{
			this._Types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
			this._RegisteredAssemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		#endregion

		#region Methods

		internal void SetDefault([CanBeNull] Type defaultType)
		{
			if(defaultType != null)
				AssertValid(defaultType);
			this._DefaultType = defaultType;
		}

		public void RegisterType([CanBeNull] string key, [NotNull] Type type)
		{
			Require.ArgNotNull(type, "type");
			AssertValid(type);
			_Types.Add(key ?? String.Empty, type);
		}

		public void RegisterTypes([NotNull] Assembly assembly)
		{
			Require.ArgNotNull(assembly, "assembly");
			if(_RegisteredAssemblies.Contains(assembly.FullName))
				return;
			var attrs = (RegisteredImplementationAttribute[])assembly.GetCustomAttributes(typeof(RegisteredImplementationAttribute), false);
			foreach(var attr in attrs)
			{
				if(attr.BaseType == _BaseType)
				{
					var type = attr.ImplementingType ?? assembly.GetType(attr.ImplementingTypeName, false, false);
					if(type != null)
					{
						if(attr.IsDefault)
							SetDefault(type);
						else
							RegisterType(attr.Key, type);
					}
				}
			}
			_RegisteredAssemblies.Add(assembly.FullName);
		}

		[CanBeNull]
		internal Type GetType([CanBeNull] string key)
		{
			Type type;
			if(_Types.TryGetValue(key ?? String.Empty, out type))
				return type;
			if(!String.IsNullOrEmpty(key))
			{
				type = Type.GetType(key, false, true);
				if(type != null)
					return type;
			}
			return this._DefaultType;
		}

		[CanBeNull]
		internal TBase CreateImplementation([CanBeNull] string key)
		{
			var type = GetType(key);
			if(type == null)
				return default(TBase);
			return (TBase)Activator.CreateInstance(type);
		}

		[CanBeNull]
		internal TBase CreateImplementation([CanBeNull]string key, params object[] args)
		{
			var type = GetType(key);
			if(type == null)
				return default(TBase);
			return (TBase)Activator.CreateInstance(type, args);
		}

		internal IEnumerable<KeyValuePair<string, Type>> GetRegisteredTypes()
		{
			return _Types.ToArray();
		}

		public IEnumerable<KeyValuePair<Type, string>> GetRegisteredTypeDescriptions()
		{
			if(this._DefaultType != null)
				yield return new KeyValuePair<Type, string>(this._DefaultType, this._DefaultType.GetDescription() ?? this._DefaultType.Name);
			foreach(var t in from entry in this.GetRegisteredTypes()
							 where entry.Value != this._DefaultType
							 select new KeyValuePair<Type, string>(entry.Value, entry.Value.GetDescription() ?? entry.Value.Name))
				yield return t;
		}

		public IEnumerable<KeyValuePair<string, string>> GetRegisteredTypeDescriptionsByKey()
		{
			return from entry in this.GetRegisteredTypeDescriptions()
				   select new KeyValuePair<string, string>(this.GetTypeKey(entry.Key), entry.Value);
		}

		[CanBeNull]
		internal string GetTypeKey([NotNull]Type type)
		{
			foreach(var entry in this._Types)
			{
				if(entry.Value == type)
					return entry.Key;
			}
			return null;
		}

		internal Type GetDefaultType()
		{
			return this._DefaultType;
		}

		#endregion

	}

	#endregion

}
