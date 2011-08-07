using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region Sequence

	public sealed class Sequence : DocumentSection<SequenceTrack>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private TimeSpan _Duration;
		private TimeSpan _Position;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Duration
		{
			get { return this._Duration; }
			set
			{
				Require.ArgPositive(value, "value");
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		#endregion

		#region Constructors

		public Sequence(Guid id)
			: base(id) { }

		public Sequence()
			: this(Guid.NewGuid()) { }

		public Sequence([NotNull] XElement element, [NotNull] Document document, [CanBeNull]AniHost host)
			: base(element)
		{
			this.Duration = ModelUtil.ParseTimeSpan(element.Attribute(Schema.sequence_dur));
			this.Tracks.AddRange(element.Elements(Schema.seqtrack).Select(e => new SequenceTrack(e, document, host)));
		}

		#endregion

		#region Methods

		internal void UpdatePosition(TimeSpan position)
		{
			if(position == this._Position)
				return;
			this._Position = position;
			foreach(var track in this.Tracks)
				track.UpdatePosition(position);
		}

		internal void PushTargetValues()
		{
			foreach(var track in this.Tracks)
				track.PushTargetValues();
		}

		public override void PushTargetValues(ITransportController transport)
		{
			Require.DBG_ArgNotNull(transport,"transport");
			this.UpdatePosition(transport.Position);
			this.PushTargetValues();
			base.PushTargetValues(transport);
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.sequence,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteXAttribute(Schema.sequence_dur, this._Duration),
				ModelUtil.WriteXElements(this.Tracks));
		}

		#endregion

	}

	#endregion

}
