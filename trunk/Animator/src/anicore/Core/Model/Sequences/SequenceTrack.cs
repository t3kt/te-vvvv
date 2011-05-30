using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Model.Sequences
{

	#region SequenceTrack

	public sealed class SequenceTrack : TrackBase<SequenceClipReference>
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public SequenceTrack(Guid id)
			: base(id) { }

		public SequenceTrack()
			: this(Guid.NewGuid()) { }

		public SequenceTrack(XElement element, Document document)
			: base(element)
		{
			Require.ArgNotNull(document, "document");
			this.Clips = new ObservableCollection<SequenceClipReference>(element.Elements(Schema.seqclipref).Select(e => new SequenceClipReference(e, document)));
		}

		#endregion

		#region Methods

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.seqtrack,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteXElements(this.Clips));
		}

		#endregion

	}

	#endregion

}
