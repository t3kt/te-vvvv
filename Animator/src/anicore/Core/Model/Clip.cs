using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using Animator.Core.Runtime;
using Animator.Core.Transport;
using TESharedAnnotations;

[assembly: RegisteredImplementation(typeof(Clip), typeof(Clip))]
[assembly: RegisteredImplementation(typeof(Clip), "clip", typeof(Clip))]

namespace Animator.Core.Model
{

	#region Clip

	[Description("Generic Clip")]
	public class Clip : DocumentItem, IEquatable<Clip>
	{

		#region Static / Constant

		static Clip()
		{
			ImplementationRegistry<Clip>.SetDefault(typeof(Clip));
			ImplementationRegistry<Clip>.RegisterTypes(typeof(Clip).Assembly);
		}

		public static void RegisterType(string elementName, Type type)
		{
			Require.ArgNotNull(elementName, "elementName");
			Require.ArgNotNull(type, "type");
			ImplementationRegistry<Clip>.RegisterType(elementName, type);
		}

		public static void RegisterTypes(Assembly assembly)
		{
			ImplementationRegistry<Clip>.RegisterTypes(assembly);
		}

		[CanBeNull]
		internal static Clip ReadClipXElement([NotNull] XElement element)
		{
			Require.ArgNotNull(element, "element");
			if(element.Name == Schema.@null)
				return null;
			return ImplementationRegistry<Clip>.CreateImplementation(element.Name.ToString(), element);
		}

		[NotNull]
		internal static XElement WriteClipXElement([CanBeNull]Clip clip)
		{
			if(clip == null)
				return new XElement(Schema.@null);
			return clip.WriteXElement();
		}

		#endregion

		#region Fields

		private string _ClipType;
		private Time _Duration;
		private int _TriggerAlignment;
		private string _TargetKey;

		#endregion

		#region Properties

		public string ClipType
		{
			get { return _ClipType; }
			internal set
			{
				if(value != _ClipType)
				{
					_ClipType = value;
					OnClipTypeChanged();
				}
			}
		}

		public string TargetKey
		{
			get { return this._TargetKey; }
			set
			{
				value = value.OrNullIfEmpty();
				if(value != this._TargetKey)
				{
					this._TargetKey = value;
					OnPropertyChanged("TargetKey");
				}
			}
		}

		public Time Duration
		{
			get { return _Duration; }
			set
			{
				if(value != _Duration)
				{
					_Duration = value;
					OnPropertyChanged("Duration");
				}
			}
		}

		public int TriggerAlignment
		{
			get { return _TriggerAlignment; }
			set
			{
				if(value != _TriggerAlignment)
				{
					_TriggerAlignment = value;
					OnPropertyChanged("TriggerAlignment");
				}
			}
		}

		#endregion

		#region Constructors

		public Clip()
			: this(Guid.NewGuid()) { }

		public Clip(Guid id)
		{
			this.Id = id;
		}

		public Clip(XElement element)
		{
			ReadXElement(element);
		}

		#endregion

		#region Methods

		internal virtual object GetValue(Time position)
		{
			return null;
		}

		protected virtual void OnClipTypeChanged()
		{
			OnPropertyChanged("ClipType");
		}

		protected void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			SuspendNotify();
			try
			{
				this.Id = (Guid)element.Attribute(Schema.clip_id);
				this.Name = (string)element.Attribute(Schema.clip_name);
				this.Duration = (float)element.Attribute(Schema.clip_dur);
				this.TriggerAlignment = (int?)element.Attribute(Schema.clip_align) ?? Document.NoAlignment;
				this.ClipType = (string)element.Attribute(Schema.clip_type);
				this.TargetKey = (string)element.Attribute(Schema.clip_target);
			}
			finally
			{
				ResumeNotify();
				OnPropertyChanged(null);
			}
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.clip,
				new XAttribute(Schema.clip_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.clip_name, this.Name),
				new XAttribute(Schema.clip_dur, this.Duration.Beats),
				this.TriggerAlignment == Document.NoAlignment ? null : new XAttribute(Schema.clip_align, this.TriggerAlignment),
				ModelUtil.WriteOptionalAttribute(Schema.clip_type, this.ClipType),
				ModelUtil.WriteOptionalAttribute(Schema.clip_target, this.TargetKey));
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Clip);
		}

		#endregion

		#region IEquatable<Clip> Members

		public virtual bool Equals(Clip other)
		{
			return base.Equals(other) &&
				   other._Duration == this._Duration &&
				   other._TriggerAlignment == this._TriggerAlignment &&
				   other._ClipType == this._ClipType &&
				   other._TargetKey == this._TargetKey;
		}

		#endregion

	}

	#endregion

}
