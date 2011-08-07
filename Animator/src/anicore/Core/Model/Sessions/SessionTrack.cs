using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using TESharedAnnotations;

namespace Animator.Core.Model.Sessions
{

	#region SessionTrack

	public sealed class SessionTrack : Track
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly DocumentNodeCollection<SessionClip> _Clips;

		#endregion

		#region Properties

		public ObservableCollection<SessionClip> Clips
		{
			get { return this._Clips; }
		}

		internal override IEnumerable<Clips.ClipBase> ClipsInternal
		{
			get { return this._Clips; }
		}

		#endregion

		#region Constructors

		public SessionTrack(Document document)
			: this(Guid.NewGuid(), document) { }

		public SessionTrack(Guid id, Document document)
			: base(id, document)
		{
			this._Clips = new DocumentNodeCollection<SessionClip>(this);
			this.ObserveChildCollection("Clips", this._Clips);
		}

		public SessionTrack([NotNull] XElement element, Document document, [CanBeNull]AniHost host)
			: base(element, document)
		{
			this._Clips = new DocumentNodeCollection<SessionClip>(this);
			this.ObserveChildCollection("Clips", this._Clips);
			this._Clips.AddRange(element.Elements(Schema.sesclip).Select(e => new SessionClip(e, host ?? AniHost.Current)));
		}

		#endregion

		#region Methods

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.sestrack,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteXElements(this.Clips));
		}

		#endregion

	}

	#endregion

}
