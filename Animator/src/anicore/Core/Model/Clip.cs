using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
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
			return ImplementationRegistry<Clip>.CreateImplementation(element.Name.ToString(), element);
		}

		#endregion

		#region Fields

		private Time _Duration;
		private int _TriggerAlignment;
		private string _TargetKey;
		private Guid? _OutputId;
		private int? _UIRow;
		private int? _UIColumn;

		private bool _IsPlaying;
		private Time _StartTime;

		#endregion

		#region Properties

		public string TargetKey
		{
			get { return this._TargetKey; }
			set
			{
				value = value.OrNullIfEmpty();
				if(value != this._TargetKey)
				{
					this._TargetKey = value;
					this.OnPropertyChanged("TargetKey");
				}
			}
		}

		public Time Duration
		{
			get { return this._Duration; }
			set
			{
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		public int TriggerAlignment
		{
			get { return this._TriggerAlignment; }
			set
			{
				if(value != this._TriggerAlignment)
				{
					this._TriggerAlignment = value;
					this.OnPropertyChanged("TriggerAlignment");
				}
			}
		}

		public Guid? OutputId
		{
			get { return this._OutputId; }
			set
			{
				if(value != this._OutputId)
				{
					this._OutputId = value;
					this.OnPropertyChanged("OutputId");
				}
			}
		}

		public int? UIRow
		{
			get { return this._UIRow; }
			set
			{
				if(value != this._UIRow)
				{
					this._UIRow = value;
					this.OnPropertyChanged("UIRow");
				}
			}
		}

		public int? UIColumn
		{
			get { return this._UIColumn; }
			set
			{
				if(value != this._UIColumn)
				{
					this._UIColumn = value;
					this.OnPropertyChanged("UIColumn");
				}
			}
		}

		public bool IsPlaying
		{
			get { return this._IsPlaying; }
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

		public void Start(ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			this._IsPlaying = true;
			this._StartTime = transport.Position;
			this.OnPropertyChanged("IsPlaying");
		}

		public void Stop()
		{
			this._IsPlaying = false;
			this.OnPropertyChanged("IsPlaying");
		}

		public Time GetPosition(ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			if(!this._IsPlaying)
				return 0;
			return (transport.Position - this._StartTime) % this.Duration;
		}

		public object GetValue(ITransport transport)
		{
			if(!this._IsPlaying)
				return null;
			return this.GetValue(this.GetPosition(transport));
		}

		internal OutputMessage BuildOutputMessage(ITransport transport)
		{
			return new OutputMessage(this.TargetKey, this.GetValue(transport));
		}

		private void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			SuspendNotify();
			try
			{
				this.Id = (Guid)element.Attribute(Schema.clip_id);
				this.Name = (string)element.Attribute(Schema.clip_name);
				this.Duration = (float)element.Attribute(Schema.clip_dur);
				this.TriggerAlignment = (int?)element.Attribute(Schema.clip_align) ?? Document.NoAlignment;
				this.TargetKey = (string)element.Attribute(Schema.clip_target);
				this.OutputId = (Guid?)element.Attribute(Schema.clip_output);
				this.UIRow = (int?)element.Attribute(Schema.clip_ui_row);
				this.UIColumn = (int?)element.Attribute(Schema.clip_ui_col);
			}
			finally
			{
				this.ResumeNotify();
				this.OnPropertyChanged(null);
			}
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.clip,
				new XAttribute(Schema.clip_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.clip_name, this.Name),
				new XAttribute(Schema.clip_dur, this.Duration.Beats),
				this.TriggerAlignment == Document.NoAlignment ? null : new XAttribute(Schema.clip_align, this.TriggerAlignment),
				ModelUtil.WriteOptionalAttribute(Schema.clip_target, this.TargetKey),
				ModelUtil.WriteOptionalValueAttribute(Schema.clip_output, this.OutputId),
				ModelUtil.WriteOptionalValueAttribute(Schema.clip_ui_row, this.UIRow),
				ModelUtil.WriteOptionalValueAttribute(Schema.clip_ui_col, this.UIColumn));
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
				   other._TargetKey == this._TargetKey &&
				   other._OutputId == this._OutputId;
		}

		#endregion

	}

	#endregion

}
