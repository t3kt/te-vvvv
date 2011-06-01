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
				this.ReadProperty(e);
		}

		#endregion

		#region Methods

		private void Property_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged("Properties");
		}

		private void Add(TargetProperty property)
		{
			Require.ArgNotNull(property, "property");
			this._Properties.Add(property.Name, property);
			property.PropertyChanged += this.Property_PropertyChanged;
			this.OnPropertyChanged("Properties");
		}

		private void ReadProperty(XElement element)
		{
			Require.ArgNotNull(element, "element");
			var property = new TargetProperty(this, element);
			this.Add(property);
		}

		public TargetProperty Add(string name, TargetPropertyType type, object defaultValue)
		{
			var property = new TargetProperty(this, name, type, defaultValue);
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

		internal OutputMessage BuildPropertyValueMessage(TargetProperty property)
		{
			if(property == null)
				return null;
			var key = this.OutputKey + property.Name;
			return new OutputMessage(key, property.Value);
		}

		internal IEnumerable<OutputMessage> BuildPropertyValueMessages()
		{
			if(this._Properties == null)
				return Enumerable.Empty<OutputMessage>();
			return this._Properties.Values.Select(this.BuildPropertyValueMessage).Where(m => m != null);
		}

		internal bool SetValue(string name, object value)
		{
			var property = this.Get(name);
			if(property == null)
				return false;
			property.Value = value;
			return true;
		}

		internal bool ClearValue(string name)
		{
			var property = this.Get(name);
			if(property == null)
				return false;
			property.ClearValue();
			return true;
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
