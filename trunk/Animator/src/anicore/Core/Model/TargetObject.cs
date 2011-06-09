using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region TargetPropertyValueChangedEventArgs

	public sealed class TargetPropertyValueChangedEventArgs : EventArgs
	{

		#region Static/Constant

		internal const string KeyNameSeparator = "#";

		#endregion

		#region Fields

		private readonly string _OutputKey;
		private readonly string _Name;
		private readonly object _Value;

		#endregion

		#region Properties

		public string OutputKey
		{
			get { return this._OutputKey; }
		}

		public string Name
		{
			get { return this._Name; }
		}

		public object Value
		{
			get { return this._Value; }
		}

		#endregion

		#region Constructors

		public TargetPropertyValueChangedEventArgs(string outputKey, string name, object value)
		{
			this._OutputKey = outputKey;
			this._Name = name;
			this._Value = value;
		}

		#endregion

		#region Methods

		private string GetTargetKey()
		{
			if(String.IsNullOrEmpty(this._OutputKey))
				return this._Name ?? String.Empty;
			if(String.IsNullOrEmpty(this._Name))
				return this._OutputKey;
			return this._OutputKey + KeyNameSeparator + this._Name;
		}

		internal OutputMessage BuildOutputMessage()
		{
			return new OutputMessage(this.GetTargetKey(), this._Value);
		}

		#endregion

	}

	#endregion

	#region TargetObject

	public sealed class TargetObject : DocumentItem, IEquatable<TargetObject>
	{

		#region Static / Constant

		internal static StringComparer PropertyNameComparer
		{
			get { return StringComparer.OrdinalIgnoreCase; }
		}

		#endregion

		#region Fields

		private string _OutputKey;
		private readonly Dictionary<string, TargetProperty> _Properties;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Common)]
		public string OutputKey
		{
			get { return this._OutputKey ?? this.Name; }
			set
			{
				if(value != this._OutputKey)
				{
					this._OutputKey = value;
					this.OnPropertyChanged("OutputKey");
				}
			}
		}

		public IEnumerable<TargetProperty> Properties
		{
			get { return this._Properties.Values.OrderBy(p => p.Name, PropertyNameComparer); }
		}

		#endregion

		#region Events

		public event EventHandler<TargetPropertyValueChangedEventArgs> PropertyValueChanged;

		private void OnPropertyValueChanged(string name, object value)
		{
			var handler = this.PropertyValueChanged;
			if(handler != null)
				handler(this, new TargetPropertyValueChangedEventArgs(this.OutputKey, name, value));
		}

		#endregion

		#region Constructors

		public TargetObject()
			: this(Guid.NewGuid()) { }

		public TargetObject(Guid id)
			: base(id)
		{
			this._Properties = new Dictionary<string, TargetProperty>(PropertyNameComparer);
		}

		public TargetObject([NotNull]XElement element)
			: base(element)
		{
			this.OutputKey = (string)element.Attribute(Schema.target_key);
			this._Properties = new Dictionary<string, TargetProperty>(PropertyNameComparer);
			foreach(var e in element.Elements(Schema.target_prop))
				this.Add(new TargetProperty(e));
		}

		#endregion

		#region Methods

		private void Property_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged("Properties");
		}

		private void Property_ValueChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is TargetProperty);
			var prop = (TargetProperty)sender;
			//if(prop != null)
			this.OnPropertyValueChanged(prop.Name, prop.Value);
		}

		private void Add(TargetProperty property)
		{
			Require.ArgNotNull(property, "property");
			this._Properties.Add(property.Name, property);
			property.PropertyChanged += this.Property_PropertyChanged;
			property.ValueChanged += this.Property_ValueChanged;
			this.OnPropertyChanged("Properties");
		}

		public TargetProperty Add(string name, TargetPropertyType type, object defaultValue)
		{
			var property = new TargetProperty(name, type, defaultValue);
			this.Add(property);
			return property;
		}

		public TargetProperty Add(string name, TargetPropertyType type)
		{
			return this.Add(name, type, null);
		}

		public TargetProperty Get(string name)
		{
			TargetProperty property;
			return this._Properties.TryGetValue(name, out property) ? property : null;
		}

		public bool Remove(string name)
		{
			TargetProperty property;
			if(!this._Properties.TryGetValue(name, out property))
				return false;
			property.PropertyChanged -= this.Property_PropertyChanged;
			property.ValueChanged -= this.Property_ValueChanged;
			var removed = this._Properties.Remove(name);
			Debug.Assert(removed);
			this.OnPropertyChanged("Properties");
			return true;
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.target,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteOptionalAttribute(Schema.target_key, this._OutputKey),
				ModelUtil.WriteXElements(this.Properties));
		}

		public bool SetValue(string name, object value)
		{
			var property = this.Get(name);
			if(property == null)
				return false;
			property.Value = value;
			return true;
		}

		public bool SetDefaultValue(string name, object value)
		{
			var property = this.Get(name);
			if(property == null)
				return false;
			property.DefaultValue = value;
			return true;
		}

		public bool ClearValue(string name)
		{
			var property = this.Get(name);
			if(property == null)
				return false;
			property.ClearValue();
			return true;
		}

		public object GetValue(string name)
		{
			object value;
			if(!this.GetValue(name, out value))
				throw new KeyNotFoundException(String.Format("TargetProperty not found: '{0}'", name));
			return value;
		}

		internal bool GetValue(string name, out object value)
		{
			var property = this.Get(name);
			if(property != null)
			{
				value = property.Value;
				return true;
			}
			value = null;
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as TargetObject);
		}

		#endregion

		#region IEquatable<TargetObject> Members

		public bool Equals(TargetObject other)
		{
			if(!base.Equals(other) || this.OutputKey != other.OutputKey)
				return false;
			return this.Properties.SequenceEqual(other.Properties);
		}

		#endregion

	}

	#endregion

}
