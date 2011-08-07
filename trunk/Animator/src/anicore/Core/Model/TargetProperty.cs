using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region TargetPropertyType

	public enum TargetPropertyType
	{
		Value,
		String
	}

	#endregion

	#region TargetProperty

	public sealed class TargetProperty : INotifyPropertyChanged, IXElementWritable, IEquatable<TargetProperty>
	{

		#region TypeHandler

		private abstract class TypeHandler
		{

			public readonly Type ValueType;

			protected TypeHandler(Type valueType)
			{
				this.ValueType = valueType;
			}

			public abstract object ParseValueAttribute(XAttribute attribute);

			public virtual XAttribute WriteValueAttribute(XName name, object value)
			{
				if(value == null)
					return null;
				return new XAttribute(name, value);
			}

			public abstract bool ValuesEqual(object x, object y);

		}

		#endregion

		#region ValueHandler

		private sealed class ValueHandler : TypeHandler
		{

			private static double PrepareValue(object x)
			{
				if(x is double)
					return (double)x;
				if(x is float)
					return (float)x;
				if(x is int)
					return (int)x;
				return Convert.ToDouble(x);
			}

			public ValueHandler() : base(typeof(double)) { }

			public override object ParseValueAttribute(XAttribute attribute)
			{
				return (double?)attribute ?? 0f;
			}

			public override bool ValuesEqual(object x, object y)
			{
				if(x == y)
					return true;
				if(x == null || y == null)
					return false;
				//return (double)x == (double)y;
				var vx = PrepareValue(x);
				var vy = PrepareValue(y);
				return vx == vy;
			}

		}

		#endregion

		#region StringHandler

		private sealed class StringHandler : TypeHandler
		{

			public StringHandler() : base(typeof(string)) { }

			public override object ParseValueAttribute(XAttribute attribute)
			{
				return (string)attribute;
			}

			public override bool ValuesEqual(object x, object y)
			{
				if(x == y)
					return true;
				if(x == null || y == null)
					return false;
				return (string)x == (string)y;
			}

		}

		#endregion

		#region Static / Constant

		private static readonly Dictionary<TargetPropertyType, TypeHandler> _TypeHandlers;

		static TargetProperty()
		{
			_TypeHandlers =
				new Dictionary<TargetPropertyType, TypeHandler>
				{
					{TargetPropertyType.Value, new ValueHandler()},
					{TargetPropertyType.String, new StringHandler()}
				};
		}

		internal static object ParseValueAttribute(TargetPropertyType type, XAttribute attr)
		{
			return _TypeHandlers[type].ParseValueAttribute(attr);
		}

		internal static XAttribute WriteValueAttribute(XName name, TargetPropertyType type, object value)
		{
			return _TypeHandlers[type].WriteValueAttribute(name, value);
		}

		internal static bool ValuesEqual(TargetPropertyType type, object x, object y)
		{
			return _TypeHandlers[type].ValuesEqual(x, y);
		}

		internal static Type GetValueType(TargetPropertyType type)
		{
			return _TypeHandlers[type].ValueType;
		}

		#endregion

		#region Fields

		private readonly string _Name;
		private readonly TargetPropertyType _Type;
		private readonly TypeHandler _Handler;
		private object _DefaultValue;
		private bool _HasValue;
		private object _Value;

		#endregion

		#region Properties

		public string Name
		{
			get { return this._Name; }
		}

		public TargetPropertyType Type
		{
			get { return this._Type; }
		}

		public object DefaultValue
		{
			get { return this._DefaultValue; }
			set
			{
				if(value != this._DefaultValue)
				{
					this._DefaultValue = value;
					this.OnPropertyChanged("DefaultValue");
				}
			}
		}

		public object Value
		{
			get { return this._HasValue ? this._Value : this._DefaultValue; }
			internal set
			{
				if(!this._Handler.ValuesEqual(value, this._Value))
				{
					this._Value = value;
					this._HasValue = true;
					this.OnValueChanged();
				}
			}
		}

		#endregion

		#region Events

		public event EventHandler ValueChanged;

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnValueChanged()
		{
			var handler = this.ValueChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
			this.OnPropertyChanged("Value");
		}

		private void OnPropertyChanged(string name)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Constructors

		internal TargetProperty([NotNull] string name, TargetPropertyType type, object defaultValue)
		{
			Require.ArgNotNullOrEmpty(name, "name");
			this._Name = name;
			this._Type = type;
			this._Handler = _TypeHandlers[type];
			this._DefaultValue = defaultValue;
		}

		internal TargetProperty([NotNull] XElement element)
		{
			Require.ArgNotNull(element, "element");
			this._Name = (string)element.Attribute(Schema.target_prop_name);
			this._Type = (TargetPropertyType)Enum.Parse(typeof(TargetPropertyType), (string)element.Attribute(Schema.target_prop_type));
			this._Handler = _TypeHandlers[this._Type];
			this._DefaultValue = this._Handler.ParseValueAttribute(element.Attribute(Schema.target_prop_default));
		}

		#endregion

		#region Methods

		internal void ClearValue()
		{
			if(this._HasValue)
			{
				this._HasValue = false;
				if(!this._Handler.ValuesEqual(this._Value, this._DefaultValue))
					this.OnValueChanged();
				this._Value = null;
			}
		}

		public override int GetHashCode()
		{
			return this._Name.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as TargetProperty);
		}

		#endregion

		#region IEquatable<TargetProperty> Members

		public bool Equals(TargetProperty other)
		{
			return other != null &&
				   TargetObject.PropertyNameComparer.Equals(this._Name, other._Name) &&
				   this._Type == other._Type &&
				   this._Handler.ValuesEqual(this._DefaultValue, other._DefaultValue);
		}

		#endregion

		#region IXElementWritable Members

		public XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.target_prop,
				new XAttribute(Schema.target_prop_name, this._Name),
				new XAttribute(Schema.target_prop_type, this._Type),
				this._Handler.WriteValueAttribute(Schema.target_prop_default, this._DefaultValue));
		}

		#endregion

	}

	#endregion

}
