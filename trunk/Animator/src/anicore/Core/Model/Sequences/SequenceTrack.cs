using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using Animator.Core.Model.Clips;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceTrack

	[ContentProperty("Clips")]
	public sealed class SequenceTrack : Track
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly SequenceClipCollection _Clips;
		private SequenceClip _ActiveClip;
		private TimeSpan _Position;

		#endregion

		#region Properties

		public SequenceClipCollection Clips
		{
			get { return this._Clips; }
		}

		internal override IEnumerable<ClipBase> ClipsInternal
		{
			get { return this._Clips; }
		}

		public SequenceClip ActiveClip
		{
			get { return this._ActiveClip; }
		}

		#endregion

		#region Constructors

		public SequenceTrack(Guid id, Document document)
			: base(id, document)
		{
			this._Clips = new SequenceClipCollection(this);
			this.ObserveChildCollection("Clips", this._Clips);
		}

		public SequenceTrack()
			: this(Guid.NewGuid(), null) { }

		public SequenceTrack(Document document)
			: this(Guid.NewGuid(), document) { }

		public SequenceTrack([NotNull] XElement element, Document document, [CanBeNull]AniHost host)
			: base(element, document)
		{
			var clips = element.Elements(Schema.seqclip).Select(e => new SequenceClip(e, host ?? AniHost.Current)).ToList();
			this._Clips = new SequenceClipCollection(this, clips.Count);
			this.ObserveChildCollection("Clips", this._Clips);
			this._Clips.AddRange(clips);
		}

		#endregion

		#region Methods

		internal void UpdatePosition(TimeSpan position)
		{
			this._Position = position;
			if(this._ActiveClip != null && this._ActiveClip.ContainsPosition(position))
				return;
			foreach(var clip in this._Clips)
			{
				if(clip.ContainsPosition(position))
				{
					this._ActiveClip = clip;
					return;
				}
			}
			this._ActiveClip = null;
		}

		internal void PushTargetValues()
		{
			if(this.Target != null && this._ActiveClip != null)
				this._ActiveClip.PushTargetValues(this.Target, this._Position);
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.seqtrack,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteXElements(this._Clips));
		}

		#endregion

	}

	#endregion

}
