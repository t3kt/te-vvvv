using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using Animator.Core.Model.Clips;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceTrack

	public sealed class SequenceTrack : Track
	{

		#region ClipCollection

		private sealed class ClipCollection : ObservableCollection<SequenceClip>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly SequenceTrack _Track;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public ClipCollection(SequenceTrack track)
			{
				this._Track = track;
			}

			#endregion

			#region Methods

			protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
			{
				base.OnCollectionChanged(e);
				this._Track.OnPropertyChanged("Clips");
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ClipCollection _Clips;
		private SequenceClip _ActiveClip;

		#endregion

		#region Properties

		public IList<SequenceClip> Clips
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

		public SequenceTrack(Guid id, [NotNull]Document document)
			: base(id, document)
		{
			this._Clips = new ClipCollection(this);
		}

		public SequenceTrack([NotNull]Document document)
			: this(Guid.NewGuid(), document) { }

		public SequenceTrack([NotNull] XElement element, [NotNull]Document document, [CanBeNull]AniHost host)
			: base(element, document)
		{
			this._Clips = new ClipCollection(this);
			this._Clips.AddRange(element.Elements(Schema.seqclip).Select(e => new SequenceClip(e, host ?? document.Host)));
		}

		#endregion

		#region Methods

		internal void UpdatePosition(TimeSpan position)
		{
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
			throw new NotImplementedException();
		}

		internal override bool TryDeleteItem(IDocumentItem item)
		{
			var clip = item as SequenceClip;
			return clip != null && this._Clips.Remove(clip);
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
