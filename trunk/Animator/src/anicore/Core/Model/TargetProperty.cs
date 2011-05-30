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

		#region Static / Constant

		internal static object ParseValueAttribute(TargetPropertyType type, XAttribute attr)
		{
			switch(type)
			{
			case TargetPropertyType.Value:
				return ((float?)attr) ?? 0f;
			case TargetPropertyType.String:
				return (string)attr;
			default:
				return null;
			}
		}

		internal static XAttribute WriteValueAttribute(XName name, TargetPropertyType type, object value)
		{
			if(value == null)
				return null;
			return new XAttribute(name, value);
		}

		private static bool ValuesEqual(TargetPropertyType type, object value1, object value2)
		{
			if(value1 == value2)
				return true;
			if(value1 == null)
				return false;
			if(value2 == null)
				return false;
			switch(type)
			{
			case TargetPropertyType.String:
				return String.Equals(Convert.ToString(value1), Convert.ToString(value2));
			case TargetPropertyType.Value:
				return Convert.ToSingle(value1) == Convert.ToSingle(value2);
			default:
				return false;
			}
		}

		#endregion

		#region Fields

		private readonly string _Name;
		private readonly TargetPropertyType _Type;
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
				this._Value = value;
				this._HasValue = true;
				this.OnValueChanged();
			}
		}

		#endregion

		#region Constructors

		public TargetProperty(string name, TargetPropertyType type)
		{
			this._Name = name;
			this._Type = type;
		}

		public TargetProperty([NotNull]XElement element)
		{
			Require.ArgNotNull(element, "element");
			this._Name = (string)element.Attribute(Schema.target_prop_name);
			this._Type = (TargetPropertyType)Enum.Parse(typeof(TargetPropertyType), (string)element.Attribute(Schema.target_prop_type));
			this._DefaultValue = ParseValueAttribute(this._Type, element.Attribute(Schema.target_prop_default));
		}

		#endregion

		#region Methods

		internal void ClearValue()
		{
			this._HasValue = false;
			this._Value = null;
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
				   ValuesEqual(this._Type,this._DefaultValue, other._DefaultValue);
		}

		#endregion

		#region IXElementWritable Members

		public XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.target_prop,
				new XAttribute(Schema.target_prop_name, this._Name),
				new XAttribute(Schema.target_prop_type, this._Type),
				WriteValueAttribute(Schema.target_prop_default, this._Type, this._DefaultValue));
		}

		#endregion

		#region INotifyPropertyChanged Members

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

	}

	#endregion

}
