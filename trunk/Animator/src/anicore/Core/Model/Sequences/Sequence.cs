using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region Sequence

	public sealed class Sequence : DocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Time _Duration;
		private ObservableCollection<SequenceTrack> _Tracks;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Transport)]
		public Time Duration
		{
			get { return this._Duration; }
			set
			{
				Require.ArgPositive((float)value, "value");
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		public ObservableCollection<SequenceTrack> Tracks
		{
			get
			{
				if(this._Tracks == null)
				{
					this._Tracks = new ObservableCollection<SequenceTrack>();
					this.AttachTracks(this._Tracks);
				}
				return this._Tracks;
			}
			set
			{
				if(value != this._Tracks)
				{
					this.DetachTracks(this._Tracks);
					this._Tracks = value;
					this.AttachTracks(value);
					this.OnPropertyChanged("Tracks");
				}
			}
		}

		#endregion

		#region Constructors

		public Sequence(Guid id)
			: base(id) { }

		public Sequence()
			: this(Guid.NewGuid()) { }

		public Sequence([NotNull] XElement element, [NotNull] Document document)
			: base(element)
		{
			Require.ArgNotNull(document, "document");
			this.Duration = (float)element.Attribute(Schema.sequence_dur);
			this.Tracks = new ObservableCollection<SequenceTrack>(element.Elements(Schema.seqtrack).Select(e => new SequenceTrack(e, document)));
		}

		#endregion

		#region Methods

		private void AttachTracks(ObservableCollection<SequenceTrack> tracks)
		{
			if(tracks != null)
				tracks.CollectionChanged += this.Tracks_CollectionChanged;
		}

		private void DetachTracks(ObservableCollection<SequenceTrack> tracks)
		{
			if(tracks != null)
				tracks.CollectionChanged -= this.Tracks_CollectionChanged;
		}

		private void Tracks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Tracks");
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.sequence,
				this.WriteCommonXAttributes(),
				new XAttribute(Schema.sequence_dur, (float)this.Duration),
				ModelUtil.WriteXElements(this._Tracks));
		}

		#endregion

	}

	#endregion

}
