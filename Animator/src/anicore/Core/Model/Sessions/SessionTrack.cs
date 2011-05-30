using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model.Sessions
{

	#region SessionTrack

	public sealed class SessionTrack : TrackBase<SessionClipReference>
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public SessionTrack(Guid id)
			: base(id) { }

		public SessionTrack([NotNull] XElement element, [NotNull]Document document)
			: base(element)
		{
			Require.ArgNotNull(document, "document");
			this.Clips = new ObservableCollection<SessionClipReference>(element.Elements(Schema.sesclipref).Select(e => new SessionClipReference(e, document)));
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
