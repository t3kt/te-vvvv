using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region Sequence

	public sealed class Sequence : DocumentSection<SequenceTrack>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Time _Duration;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Transport)]
		public Time Duration
		{
			get { return this._Duration; }
			set
			{
				Require.ArgPositive((float)value, "value");
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		#endregion

		#region Constructors

		public Sequence(Guid id, [NotNull]Document document)
			: base(id, document) { }

		public Sequence([NotNull]Document document)
			: this(Guid.NewGuid(), document) { }

		public Sequence([NotNull] XElement element, [NotNull] Document document)
			: base(element, document)
		{
			this.Duration = (float)element.Attribute(Schema.sequence_dur);
			this.Tracks.AddRange(element.Elements(Schema.seqtrack).Select(e => new SequenceTrack(e, document)));
		}

		#endregion

		#region Methods

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.sequence,
				this.WriteCommonXAttributes(),
				new XAttribute(Schema.sequence_dur, (float)this.Duration),
				ModelUtil.WriteXElements(this.Tracks));
		}

		#endregion

	}

	#endregion

}
