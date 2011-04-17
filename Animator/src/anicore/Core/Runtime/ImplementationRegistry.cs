using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

	#region ImplementationRegistry<TBase>

	internal static class ImplementationRegistry<TBase>
	{

		private static readonly Type _BaseType = typeof(TBase);
		private static readonly Dictionary<string, Type> _Types = new Dictionary<string, Type>();
		private static readonly HashSet<string> _RegisteredAssemblies = new HashSet<string>();
		private static Type _Default;

		internal static void SetDefault([CanBeNull] Type defaultType)
		{
			if(defaultType != null)
				AssertValid(defaultType);
			_Default = defaultType;
		}

		private static void AssertValid(Type type)
		{
			if(!_BaseType.IsAssignableFrom(type))
				throw new NotSupportedException(String.Format("Type does not implement {0}: '{1}'", _BaseType, type));
		}

		internal static void RegisterType([CanBeNull] string key, [NotNull] Type type)
		{
			Require.ArgNotNull(type, "type");
			AssertValid(type);
			_Types.Add(key ?? String.Empty, type);
		}

		internal static void RegisterTypes([NotNull] Assembly assembly)
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
		internal static Type GetType([CanBeNull] string key)
		{
			Type type;
			if(_Types.TryGetValue(key ?? String.Empty, out type))
				return type;
			return _Default;
		}

		[CanBeNull]
		internal static TBase CreateImplementation([CanBeNull] string key)
		{
			var type = GetType(key);
			if(type == null)
				return default(TBase);
			return (TBase)Activator.CreateInstance(type);
		}

		[CanBeNull]
		internal static TBase CreateImplementation([CanBeNull]string key, params object[] args)
		{
			var type = GetType(key);
			if(type == null)
				return default(TBase);
			return (TBase)Activator.CreateInstance(type, args);
		}

		internal static IEnumerable<KeyValuePair<string, Type>> GetRegisteredTypes()
		{
			return _Types.ToArray();
		}

	}

	#endregion

}
