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
using Animator.Core.Transport;
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
			set { this.TryChangeInterval(value); }
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
					this.TryChangeInterval(new Interval(value, this._Interval.Duration));
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
					this.TryChangeInterval(new Interval(this._Interval.Start, value));
			}
		}

		internal TimeSpan End
		{
			get { return this._Interval.End; }
		}

		#endregion

		#region Events

		internal event EventHandler<ValueChangedEventArgs<Interval>> IntervalChanged;

		private void OnIntervalChanged(Interval origInterval)
		{
			var handler = this.IntervalChanged;
			if(handler != null)
				handler(this, new ValueChangedEventArgs<Interval>(origInterval, this._Interval));
		}

		#endregion

		#region Constructors

		public SequenceClip()
			: this(Guid.NewGuid()) { }

		public SequenceClip(Guid id)
			: base(id) { }

		internal SequenceClip(Interval interval)
			: this()
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

		private TryChangeValueDecision TryChangeInterval(Interval interval, bool ignoreSameValue)
		{
			if(!ignoreSameValue && interval == this._Interval)
				return TryChangeValueDecision.None;
			var decision = TryChangeValueUtil.ApplyHandler(this._TryChangeIntervalHandler, this, this._Interval, ref interval);
			if(!TryChangeValueUtil.IsApproved(decision))
				return decision;
			if(!ignoreSameValue && interval == this._Interval)
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

		internal TryChangeValueDecision TryChangeInterval(Interval interval)
		{
			return this.TryChangeInterval(interval, false);
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

		internal bool ApplyIntervalChangeHandler()
		{
			var decision = this.TryChangeInterval(this._Interval, true);
			return TryChangeValueUtil.IsApproved(decision);
		}

		internal bool ContainsPosition(TimeSpan position)
		{
			//return position >= this._Interval.Start && position < this._Interval.End;
			return this._Interval.Contains(position);
		}

		internal bool IsActive(TimeSpan position)
		{
			return this.ContainsPosition(position);
			//return !(position < this.Start) && position < this.End;
		}

		internal double GetPosition(TimeSpan tPosition)
		{
			if(tPosition <= this.Start)
				return 0;
			if(tPosition >= this.End)
				return 1;
			return CommonUtil.MapFloating(tPosition.TotalSeconds, this.Start.TotalSeconds, this.End.TotalSeconds, 0, 1);
		}

		internal override void PushTargetValues(TargetObject target, ITransportController transport)
		{
			Require.DBG_ArgNotNull(target, "target");
			Require.DBG_ArgNotNull(transport, "transport");
			var tpos = transport.Position;
			this.PushTargetValues(target, tpos);
		}

		internal void PushTargetValues(TargetObject target, TimeSpan tPosition)
		{
			Require.DBG_ArgNotNull(target, "target");
			if(!this.IsActive(tPosition))
				return;
			var pos = this.GetPosition(tPosition);
			foreach(var prop in this.Properties)
				target.SetValue(prop.Name, prop.GetValue(pos));
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
