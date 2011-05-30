using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region TrackContainer

	public abstract class DocumentSection : DocumentItem, IClipRefContainer
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		internal abstract IEnumerable<Track> TracksInternal { get; }

		#endregion

		#region Constructors

		protected DocumentSection(Guid id)
			: base(id) { }

		protected DocumentSection([NotNull] XElement element)
			: base(element) { }

		#endregion

		#region Methods

		#endregion

		#region IClipRefContainer Members

		public IEnumerable<ClipReference> Clips
		{
			get
			{
				return from track in this.TracksInternal
					   from clip in track.ClipsInternal
					   select clip;
			}
		}

		public IEnumerable<ClipReference> GetActiveClips(Transport.Transport transport)
		{
			return from track in this.TracksInternal
				   from clip in track.GetActiveClips(transport)
				   select clip;
		}

		#endregion

	}

	#endregion

	#region TrackContainer<TTrack>

	public abstract class DocumentSection<TTrack> : DocumentSection
		where TTrack : Track
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ObservableCollection<TTrack> _Tracks;

		#endregion

		#region Properties

		public ObservableCollection<TTrack> Tracks
		{
			get { return this._Tracks; }
		}

		internal sealed override IEnumerable<Track> TracksInternal
		{
			get { return this.Tracks; }
		}

		#endregion

		#region Constructors

		protected DocumentSection(Guid id)
			: base(id)
		{
			this._Tracks = new ObservableCollection<TTrack>();
			this._Tracks.CollectionChanged += this.Tracks_CollectionChanged;
		}

		protected DocumentSection([NotNull] XElement element)
			: base(element)
		{
			this._Tracks = new ObservableCollection<TTrack>();
			this._Tracks.CollectionChanged += this.Tracks_CollectionChanged;
		}

		#endregion

		#region Methods

		private void Tracks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Tracks");
		}

		#endregion

	}

	#endregion

}
