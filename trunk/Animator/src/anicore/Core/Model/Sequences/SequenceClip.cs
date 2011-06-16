using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Model.Clips;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceClip

	public sealed class SequenceClip : ClipBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		private EventHandler<TryChangeValueEventArgs<Tuple<TimeSpan, TimeSpan>>> _TryChangeSpanHandler;

		private TimeSpan _Start;
		private TimeSpan _Duration;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Start
		{
			get { return this._Start; }
			set
			{
				//if(value < TimeSpan.Zero)
				//    value = TimeSpan.Zero;
				//Require.ArgNotNegative(value, "value");
				if(value != this.Start)
					this.ChangeSpan(value, this._Duration);
			}
		}

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Duration
		{
			get { return this._Duration; }
			set
			{
				//Require.ArgNotNegative(value, "value");
				if(value != this._Duration)
					this.ChangeSpan(this._Start, value);
			}
		}

		internal TimeSpan End
		{
			get { return this._Start + this._Duration; }
		}

		#endregion

		#region Constructors

		public SequenceClip()
			: this(Guid.NewGuid()) { }

		public SequenceClip(Guid id)
			: base(id) { }

		public SequenceClip([NotNull] XElement element, [CanBeNull] AniHost host)
			: base(element, host)
		{
			this.Start = ModelUtil.ParseTimeSpan(element.Attribute(Schema.seqclip_start));
			this.Duration = ModelUtil.ParseTimeSpan(element.Attribute(Schema.seqclip_dur));
		}

		#endregion

		#region Methods

		internal void ChangeSpan(TimeSpan start, TimeSpan duration)
		{
			//if(duration < TimeSpan.Zero)
			//    throw new NotImplementedException();
			if(this._TryChangeSpanHandler != null)
			{
				var current = new Tuple<TimeSpan, TimeSpan>(this._Start, this._Duration);
				var requested = new Tuple<TimeSpan, TimeSpan>(start, duration);
				var e = new TryChangeValueEventArgs<Tuple<TimeSpan, TimeSpan>>(current, requested);
				this._TryChangeSpanHandler(this, e);
				switch(e.Decision)
				{
				case TryChangeValueDecision.Denied:
					return;
				case TryChangeValueDecision.ModifiedApproved:
					start = e.NewValue.Item1;
					duration = e.NewValue.Item2;
					break;
				//case TryChangeValueDecision.None:
				//case TryChangeValueDecision.Approved:
				//default:
				//    break;
				}
			}
			if(start != this._Start)
			{
				this._Start = start;
				this.OnPropertyChanged("Start");
			}
			if(duration != this._Duration)
			{
				this._Duration = duration;
				this.OnPropertyChanged("Duration");
			}
		}

		internal void SetSpanChangeHandler(EventHandler<TryChangeValueEventArgs<Tuple<TimeSpan, TimeSpan>>> handler, bool applyNow = false)
		{
			this._TryChangeSpanHandler = handler;
			if(applyNow)
				this.ApplySpanChangeHandler();
		}

		internal void ApplySpanChangeHandler()
		{
			this.ChangeSpan(this._Start, this._Duration);
		}

		internal bool ContainsPosition(TimeSpan position)
		{
			return position >= this._Start && position < this.End;
		}

		internal bool Overlaps([NotNull] SequenceClip other)
		{
			Require.ArgNotNull(other, "other");
			return CommonUtil.IsOverlap(this.Start, this.End, other.Start, other.End);
		}

		internal override bool IsActive(Transport.Transport transport)
		{
			var pos = transport.Position;
			return !(pos < this.Start) && pos < this.End;
		}

		protected override float GetPosition(Transport.Transport transport)
		{
			throw new NotImplementedException();
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.seqclip,
				this.WriteCommonXAttributes(),
				new XAttribute(Schema.seqclip_start, this._Start),
				ModelUtil.WriteXAttribute(Schema.seqclip_dur, this._Duration),
				this.WritePropertiesXElement());
		}

		#endregion

	}

	#endregion

}
