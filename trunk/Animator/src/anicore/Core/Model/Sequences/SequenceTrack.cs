using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceTrack

	public sealed class SequenceTrack : Track<SequenceClipReference>
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

		public SequenceTrack([NotNull] XElement element, [NotNull]Document document)
			: base(element, document)
		{
			this.Clips.AddRange(element.Elements(Schema.seqclipref).Select(e => new SequenceClipReference(e, document)));
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
