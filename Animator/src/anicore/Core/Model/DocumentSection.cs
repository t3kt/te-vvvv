using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Model.Sequences;
using Animator.Core.Model.Sessions;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region DocumentSection

	public abstract class DocumentSection : DocumentItem
	{

		#region Static/Constant

		internal static DocumentSection ReadSectionXElement([NotNull] XElement element, [NotNull] Document document, [CanBeNull]AniHost host = null)
		{
			Require.ArgNotNull(element, "element");
			if(element.Name == Schema.sequence)
				return new Sequence(element, document, host);
			if(element.Name == Schema.session)
				return new Session(element, document, host);
			return null;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		internal abstract IEnumerable<Track> TracksInternal { get; }

		#endregion

		#region Constructors

		protected DocumentSection(Guid id)
			: base(id)
		{
		}

		protected DocumentSection([NotNull] XElement element)
			: base(element)
		{
		}

		#endregion

		#region Methods

		public virtual void PushTargetValues([NotNull] ITransportController transport)
		{
			Require.ArgNotNull(transport, "transport");
			foreach(var track in this.TracksInternal)
				track.PushTargetValues(transport);
		}

		#endregion

	}

	#endregion

	#region DocumentSection<TTrack>

	[ContentProperty("Tracks")]
	public abstract class DocumentSection<TTrack> : DocumentSection
		where TTrack : Track
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly DocumentNodeCollection<TTrack> _Tracks;

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
			this._Tracks = new DocumentNodeCollection<TTrack>(this);
			this.ObserveChildCollection("Tracks", this._Tracks);
		}

		protected DocumentSection([NotNull] XElement element)
			: base(element)
		{
			this._Tracks = new DocumentNodeCollection<TTrack>(this);
			this.ObserveChildCollection("Tracks", this._Tracks);
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
