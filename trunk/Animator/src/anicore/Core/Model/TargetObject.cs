using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
		private TargetPropertyCollection _Properties;

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

		public TargetPropertyCollection Properties
		{
			get
			{
				if(this._Properties == null)
				{
					this._Properties = new TargetPropertyCollection();
					this.AttachProperties(this._Properties);
				}
				return this._Properties;
			}
			set
			{
				if(value != this._Properties)
				{
					this.DetachProperties(this._Properties);
					this._Properties = value;
					this.AttachProperties(this._Properties);
					this.OnPropertyChanged("Properties");
				}
			}
		}

		#endregion

		#region Constructors

		public TargetObject()
			: this(Guid.NewGuid()) { }

		public TargetObject(Guid id)
			: base(id) { }

		public TargetObject([NotNull]XElement element)
			: base(element)
		{
			this.ReadXElement(element);
		}

		#endregion

		#region Methods

		private void ReadXElement(XElement element)
		{
			this.OutputKey = (string)element.Attribute(Schema.target_key);
			this.Properties = new TargetPropertyCollection(element.Elements(Schema.target_prop).Select(e => new TargetProperty(e)));
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.target,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteOptionalAttribute(Schema.target_key, this._OutputKey),
				ModelUtil.WriteXElements(this._Properties));
		}

		private void AttachProperties(TargetPropertyCollection properties)
		{
			if(properties != null)
				properties.CollectionChanged += this.Properties_CollectionChanged;
		}

		private void DetachProperties(TargetPropertyCollection properties)
		{
			if(properties != null)
				properties.CollectionChanged -= this.Properties_CollectionChanged;
		}

		private void Properties_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Properties");
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
			return this._Properties.Select(this.BuildPropertyValueMessage).Where(m => m != null);
		}

		internal bool SetValue(string name, object value)
		{
			if(this._Properties == null)
				return false;
			TargetProperty property;
			if(!this._Properties.TryGetProperty(name, out property))
				return false;
			property.Value = value;
			return true;
		}

		internal bool ClearValue(string name)
		{
			if(this._Properties == null)
				return false;
			TargetProperty property;
			if(!this._Properties.TryGetProperty(name, out property))
				return false;
			property.ClearValue();
			return true;
		}

		internal bool GetValue(string name, out object value)
		{
			TargetProperty property;
			if(this._Properties != null && this._Properties.TryGetProperty(name, out property))
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
			return this.Properties.OrderBy(p => p.Name, PropertyNameComparer).SequenceEqual(other.Properties.OrderBy(p => p.Name, PropertyNameComparer));
		}

		#endregion

	}

	#endregion

}
