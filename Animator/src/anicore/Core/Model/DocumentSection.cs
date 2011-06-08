using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region DocumentSection

	public abstract class DocumentSection : DocumentItem, IClipRefContainer
	{

		#region Static/Constant

		#endregion

		#region Fields

		protected readonly Document _Document;

		#endregion

		#region Properties

		internal abstract IEnumerable<Track> TracksInternal { get; }

		#endregion

		#region Constructors

		protected DocumentSection(Guid id, [NotNull] Document document)
			: base(id)
		{
			Require.ArgNotNull(document, "document");
			this._Document = document;
		}

		protected DocumentSection([NotNull] XElement element, [NotNull] Document document)
			: base(element)
		{
			Require.ArgNotNull(document, "document");
			this._Document = document;
		}

		#endregion

		#region Methods

		public virtual void PushTargetChanges([NotNull] Transport.Transport transport)
		{
			Require.ArgNotNull(transport, "transport");
			foreach(var track in this.TracksInternal)
				track.PushTargetChanges(transport);
		}

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

	#region DocumentSection<TTrack>

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

		protected DocumentSection(Guid id, [NotNull]Document document)
			: base(id, document)
		{
			this._Tracks = new ObservableCollection<TTrack>();
			this._Tracks.CollectionChanged += this.Tracks_CollectionChanged;
		}

		protected DocumentSection([NotNull] XElement element, [NotNull]Document document)
			: base(element, document)
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
