using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceTrack

	public sealed class SequenceTrack : Track<SequenceClip>
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public SequenceTrack(Guid id, [NotNull]Document document)
			: base(id, document) { }

		public SequenceTrack([NotNull]Document document)
			: this(Guid.NewGuid(), document) { }

		public SequenceTrack([NotNull] XElement element, [NotNull]Document document, [CanBeNull]AniHost host)
			: base(element, document)
		{
			this.Clips.AddRange(element.Elements(Schema.seqclip).Select(e => new SequenceClip(e, host ?? document.Host)));
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
