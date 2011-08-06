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

		private EventHandler<TryChangeValueEventArgs<Interval>> _TryChangeIntervalHandler;

		private Interval _Interval;

		#endregion

		#region Properties

		internal Interval Interval
		{
			get { return this._Interval; }
			set { this.ChangeInterval(value); }
		}

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Start
		{
			get { return this._Interval.Start; }
			set
			{
				//if(value < TimeSpan.Zero)
				//    value = TimeSpan.Zero;
				//Require.ArgNotNegative(value, "value");
				if(value != this._Interval.Start)
					this.ChangeInterval(new Interval(value, this._Interval.Duration));
			}
		}

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Duration
		{
			get { return this._Interval.Duration; }
			set
			{
				//Require.ArgNotNegative(value, "value");
				if(value != this._Interval.Duration)
					this.ChangeInterval(new Interval(this._Interval.Start, value));
			}
		}

		internal TimeSpan End
		{
			get { return this._Interval.End; }
		}

		#endregion

		#region Events

		internal event EventHandler<ValueChangedEventArgs<Interval>> IntervalChanged;

		internal void OnIntervalChanged(Interval origInterval)
		{
			var handler = this.IntervalChanged;
			if (handler != null)
				handler(this, new ValueChangedEventArgs<Interval>(origInterval, this._Interval));
		}

		#endregion

		#region Constructors

		public SequenceClip()
			: this(Guid.NewGuid()) { }

		public SequenceClip(Guid id)
			: base(id) { }

		internal SequenceClip(Interval interval)
			:this()
		{
			this._Interval = interval;
		}

		public SequenceClip([NotNull] XElement element, [CanBeNull] AniHost host)
			: base(element, host)
		{
			this._Interval =
				new Interval(ModelUtil.ParseTimeSpan(element.Attribute(Schema.seqclip_start)),
					ModelUtil.ParseTimeSpan(element.Attribute(Schema.seqclip_dur)));
		}

		#endregion

		#region Methods

		internal TryChangeValueDecision TryChangeStart(TimeSpan value)
		{
			//if(value < TimeSpan.Zero)
			//    value = TimeSpan.Zero;
			//Require.ArgNotNegative(value, "value");
			if(value == this._Interval.Start)
				return TryChangeValueDecision.None;
			return this.TryChangeInterval(new Interval(value, this._Interval.Duration));
		}

		internal TryChangeValueDecision TryChangeDuration(TimeSpan value)
		{
			//Require.ArgNotNegative(value, "value");
			if(value == this._Interval.Duration)
				return TryChangeValueDecision.None;
			return this.TryChangeInterval(new Interval(this._Interval.Start, value));
		}

		internal TryChangeValueDecision TryChangeInterval(Interval interval)
		{
			if(interval == this._Interval)
				return TryChangeValueDecision.None;
			var decision = TryChangeValueUtil.ApplyHandler(this._TryChangeIntervalHandler, this, this._Interval, ref interval);
			if(decision != TryChangeValueDecision.Approved)
				return decision;
			if(interval == this._Interval)
				return TryChangeValueDecision.None;
			var origInterval = this._Interval;
			this._Interval = interval;
			this.OnPropertyChanged("Interval");
			this.OnPropertyChanged("Start");
			this.OnPropertyChanged("Duration");
			this.OnPropertyChanged("End");
			this.OnIntervalChanged(origInterval);
			return decision;
		}

		internal bool ChangeInterval(Interval interval)
		{
			var decision = this.TryChangeInterval(interval);
			return TryChangeValueUtil.IsApproved(decision);
		}

		internal void SetIntervalChangeHandler(EventHandler<TryChangeValueEventArgs<Interval>> handler, bool applyNow = false)
		{
			this._TryChangeIntervalHandler = handler;
			if(applyNow)
				this.ApplyIntervalChangeHandler();
		}

		internal void ApplyIntervalChangeHandler()
		{
			this.ChangeInterval(this._Interval);
		}

		internal bool ContainsPosition(TimeSpan position)
		{
			//return position >= this._Interval.Start && position < this._Interval.End;
			return this._Interval.Contains(position);
		}

		internal bool Overlaps([NotNull] SequenceClip other)
		{
			Require.ArgNotNull(other, "other");
			//return CommonUtil.IsOverlap(this.Start, this.End, other.Start, other.End);
			return this._Interval.Overlaps(other._Interval);
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
				ModelUtil.WriteXAttribute(Schema.seqclip_start, this._Interval.Start),
				ModelUtil.WriteXAttribute(Schema.seqclip_dur, this._Interval.Duration),
				this.WritePropertiesXElement());
		}

		#endregion

	}

	#endregion

}
