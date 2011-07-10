using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region Sequence

	public sealed class Sequence : DocumentSection<SequenceTrack>
	{

		#region Static / Constant

		private static readonly ItemTypeInfo _ItemType = new ItemTypeInfo(typeof(Sequence));

		#endregion

		#region Fields

		private TimeSpan _Duration;

		#endregion

		#region Properties

		public override ItemTypeInfo ItemType
		{
			get { return _ItemType; }
		}

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Duration
		{
			get { return this._Duration; }
			set
			{
				Require.ArgPositive(value, "value");
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		#endregion

		#region Constructors

		public Sequence(Guid id)
			: base(id) { }

		public Sequence()
			: this(Guid.NewGuid()) { }

		public Sequence([NotNull] XElement element, [NotNull] Document document, [CanBeNull]AniHost host)
			: base(element)
		{
			this.Duration = ModelUtil.ParseTimeSpan(element.Attribute(Schema.sequence_dur));
			this.Tracks.AddRange(element.Elements(Schema.seqtrack).Select(e => new SequenceTrack(e, document, host)));
		}

		#endregion

		#region Methods

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.sequence,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteXAttribute(Schema.sequence_dur, this._Duration),
				ModelUtil.WriteXElements(this.Tracks));
		}

		#endregion

	}

	#endregion

}
