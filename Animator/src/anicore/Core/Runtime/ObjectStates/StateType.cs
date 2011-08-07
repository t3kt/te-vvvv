using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Runtime.ObjectStates
{

	#region StateType

	internal sealed class StateType : ICustomTypeDescriptor
	{

		#region Provider

		internal sealed class Provider : TypeDescriptionProvider
		{

			#region Static / Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
			{
				//return new StateType(objectType, instance);
				var state = instance as ObjectState;
				if(state != null)
					return state.Type;
				return base.GetTypeDescriptor(objectType, instance);
			}

			#endregion

		}

		#endregion

		#region FilterCache

		private sealed class FilterCache
		{
			public readonly Attribute[] Attributes;
			public readonly PropertyDescriptorCollection FilteredProperties;

			public FilterCache(Attribute[] attributes, PropertyDescriptorCollection filteredProperties)
			{
				this.Attributes = attributes;
				this.FilteredProperties = filteredProperties;
			}

			public bool IsValid(Attribute[] other)
			{
				if(other == null || Attributes == null) return false;
				if(Attributes.Length != other.Length) return false;
				for(int i = 0; i < other.Length; i++)
				{
					if(!Attributes[i].Match(other[i])) return false;
				}
				return true;
			}
		}

		#endregion

		#region Static / Constant

		private static readonly Dictionary<Type, PropertyDescriptorCollection> _StatePropertiesCache = new Dictionary<Type, PropertyDescriptorCollection>();

		private static PropertyDescriptorCollection GetStateProperties([NotNull]Type objectType)
		{
			Require.ArgNotNull(objectType, "objectType");
			PropertyDescriptorCollection stateProperties;
			if(!_StatePropertiesCache.TryGetValue(objectType, out stateProperties))
			{
				var origProps = TypeDescriptor.GetProperties(objectType, new Attribute[] { StatePropertyAttribute.Default });
				stateProperties = new PropertyDescriptorCollection(
					origProps.Cast<PropertyDescriptor>()
						.Select(p => new StatePropertyDescriptor(p))
						.ToArray(), true);
				_StatePropertiesCache.Add(objectType, stateProperties);
			}
			return stateProperties;
		}

		#endregion

		#region Fields

		private readonly Type _ObjectType;
		private readonly object _Target;
		private readonly PropertyDescriptorCollection _StateProperties;

		private FilterCache _FilterCache;
		private PropertyDescriptorCollection _PropertyCache;

		#endregion

		#region Properties

		public Type ObjectType
		{
			get { return this._ObjectType; }
		}

		public PropertyDescriptorCollection StateProperties
		{
			get { return this._StateProperties; }
		}

		#endregion

		#region Constructors

		internal StateType([NotNull] Type objectType, [NotNull]object target)
		{
			Require.ArgNotNull(objectType, "objectType");
			Require.ArgNotNull(target, "target");
			this._ObjectType = objectType;
			this._Target = target;
			this._StateProperties = GetStateProperties(objectType);
		}

		#endregion

		#region Methods

		[NotNull]
		public ObjectState ExtractState()
		{
			var state = new ObjectState(this, this._Target);
			this.ExtractState(state);
			return state;
		}

		[NotNull]
		public void ExtractState([NotNull] ObjectState state)
		{
			Require.ArgNotNull(state, "state");
			foreach(StatePropertyDescriptor property in this._StateProperties)
				property.CopyValueToState(this._Target, state);
		}

		public void SaveState([NotNull] ObjectState state)
		{
			Require.ArgNotNull(state, "state");
			foreach(StatePropertyDescriptor property in this._StateProperties)
				property.CopyValueFromState(state, this._Target);
		}

		internal StatePropertyDescriptor GetProperty(string name)
		{
			return (StatePropertyDescriptor)this._StateProperties[name];
		}

		#endregion

		#region ICustomTypeDescriptor Members

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this._ObjectType, true);
		}

		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this._ObjectType, true);
		}

		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this._Target, true);
		}

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this._Target, true);
		}

		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this._Target, true);
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this._Target, true);
		}

		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this._Target, editorBaseType, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this._Target, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this._Target, true);
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			var filterOptions = PropertyFilterOptions.All;
			if(attributes != null)
			{
				var filterAttr = attributes.OfType<PropertyFilterAttribute>().FirstOrDefault();
				if(filterAttr != null)
				{
					filterOptions = filterAttr.Filter;
					attributes = attributes.Except(new[] { filterAttr }).ToArray();
				}
			}
			var filtering = (attributes != null && attributes.Length > 0);
			var props = this._PropertyCache;
			var cache = this._FilterCache;

			// Use a cached version if we can
			if(filtering && cache != null && cache.IsValid(attributes))
			{
				return cache.FilteredProperties;
			}
			if(!filtering && props != null)
			{
				return props;
			}

			// Create the property collection and filter if necessary
			props = new PropertyDescriptorCollection(null);
			foreach(StatePropertyDescriptor prop in this._StateProperties)
			{
				if(prop.Attributes.Contains(attributes))
					props.Add(prop);
			}

			// Store the computed properties
			if(filtering)
			{
				cache = new FilterCache(attributes, props);
				this._FilterCache = cache;
			}
			else
				this._PropertyCache = props;

			return props;
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return this.GetProperties(null);
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this._Target;
		}

		#endregion

	}

	#endregion

}
