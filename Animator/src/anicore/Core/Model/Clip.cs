using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region Clip

	[Clip(ElementName = "clip", Key = "clip", Description = "Generic Clip")]
	public class Clip : DocumentItem, IEquatable<Clip>, IClip
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Time _Duration;
		private string _TargetKey;
		private Guid? _OutputId;
		private int? _UIRow;
		private int? _UIColumn;

		private bool _IsPlaying;
		private Time _StartTime;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Output)]
		[DisplayName(TEShared.Names.DisplayName_Target)]
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

		[Category(TEShared.Names.Category_Transport)]
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

		[Category(TEShared.Names.Category_Output)]
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

		[Category(TEShared.Names.Category_UI)]
		[DisplayName(TEShared.Names.DisplayName_SessionRow)]
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

		[Category(TEShared.Names.Category_UI)]
		[DisplayName(TEShared.Names.DisplayName_SessionColumn)]
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

		[Browsable(false)]
		public bool IsPlaying
		{
			get { return this._IsPlaying; }
		}

		#endregion

		#region Constructors

		public Clip()
			: this(Guid.NewGuid()) { }

		public Clip(Guid id)
			: base(id) { }

		#endregion

		#region Methods

		internal virtual object GetValue(Time position)
		{
			return null;
		}

		public void Start(Transport.Transport transport)
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

		public Time GetPosition(Transport.Transport transport)
		{
			Require.ArgNotNull(transport, "transport");
			if(!this._IsPlaying)
				return 0;
			return (transport.Position - this._StartTime) % this.Duration;
		}

		public object GetValue(Transport.Transport transport)
		{
			if(!this._IsPlaying)
				return null;
			return this.GetValue(this.GetPosition(transport));
		}

		public virtual OutputMessage BuildOutputMessage(Transport.Transport transport)
		{
			return new OutputMessage(this.TargetKey, this.GetValue(transport));
		}

		public virtual void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.ReadCommonXAttributes(element);
			this.Duration = (float)element.Attribute(Schema.clip_dur);
			this.TargetKey = (string)element.Attribute(Schema.clip_target);
			this.OutputId = (Guid?)element.Attribute(Schema.clip_output);
			this.UIRow = (int?)element.Attribute(Schema.clip_ui_row);
			this.UIColumn = (int?)element.Attribute(Schema.clip_ui_col);
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.clip,
				this.WriteCommonXAttributes(),
				new XAttribute(Schema.clip_dur, this.Duration.Beats),
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
				   other._TargetKey == this._TargetKey &&
				   other._OutputId == this._OutputId;
		}

		#endregion

	}

	#endregion

}
