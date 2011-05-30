using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model.Sessions
{

	#region Session

	public sealed class Session : DocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		private int? _Rows;
		private ObservableCollection<SessionTrack> _Tracks;

		#endregion

		#region Properties

		public int? Rows
		{
			get { return this._Rows; }
			set
			{
				if(value != this._Rows)
				{
					this._Rows = value;
					this.OnPropertyChanged("Rows");
				}
			}
		}

		public ObservableCollection<SessionTrack> Tracks
		{
			get
			{
				if(this._Tracks == null)
				{
					this._Tracks = new ObservableCollection<SessionTrack>();
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

		public Session(Guid id)
			: base(id) { }

		public Session()
			: this(Guid.NewGuid()) { }

		public Session([NotNull] XElement element, [NotNull] Document document)
			: base(element)
		{
			Require.ArgNotNull(document, "document");
			this.Rows = (int?)element.Attribute(Schema.session_rows);
			this.Tracks = new ObservableCollection<SessionTrack>(element.Elements(Schema.seqtrack).Select(e => new SessionTrack(e, document)));
		}

		#endregion

		#region Methods

		private void AttachTracks(ObservableCollection<SessionTrack> tracks)
		{
			if(tracks != null)
				tracks.CollectionChanged += this.Tracks_CollectionChanged;
		}

		private void DetachTracks(ObservableCollection<SessionTrack> tracks)
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
			return new XElement(name ?? Schema.session,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteOptionalValueAttribute(Schema.session_rows, this.Rows),
				ModelUtil.WriteXElements(this.Tracks));
		}

		#endregion

	}

	#endregion

}
