using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using TESharedAnnotations;

namespace Animator.Core.Model.Sessions
{

	#region SessionTrack

	public sealed class SessionTrack : Track<SessionClip>
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public SessionTrack([NotNull]Document document)
			: this(Guid.NewGuid(), document) { }

		public SessionTrack(Guid id, [NotNull]Document document)
			: base(id, document) { }

		public SessionTrack([NotNull] XElement element, [NotNull]Document document, [CanBeNull]AniHost host)
			: base(element, document)
		{
			this.Clips.AddRange(element.Elements(Schema.sesclip).Select(e => new SessionClip(e, host ?? document.Host)));
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
