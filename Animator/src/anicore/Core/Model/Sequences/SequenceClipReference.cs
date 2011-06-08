using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceClipReference

	public sealed class SequenceClipReference : ClipReference
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Time _Start;
		private Time? _Duration;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Transport)]
		public Time Start
		{
			get { return this._Start; }
			set
			{
				Require.ArgNotNegative((float)value, "value");
				if(value != this._Start)
				{
					this._Start = value;
					this.OnPropertyChanged("Start");
				}
			}
		}

		[Category(TEShared.Names.Category_Transport)]
		public Time? Duration
		{
			get { return this._Duration; }
			set
			{
				if(value != null)
					Require.ArgPositive((float)value.Value, "value");
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		#endregion

		#region Constructors

		public SequenceClipReference(Guid id, Clip clip)
			: base(id, clip) { }

		public SequenceClipReference([NotNull] XElement element, [NotNull] Document document)
			: base(element, document)
		{
			this.Start = (float)element.Attribute(Schema.seqclipref_start);
			this.Duration = (float?)element.Attribute(Schema.seqclipref_dur);
		}

		#endregion

		#region Methods

		internal override bool IsActive(Transport.Transport transport)
		{
			throw new NotImplementedException();
		}

		protected override Time GetPosition(Transport.Transport transport)
		{
			throw new NotImplementedException();
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.seqclipref)
				.WithContent(
					new XAttribute(Schema.seqclipref_start, (float)this.Start),
					ModelUtil.WriteOptionalValueAttribute(Schema.seqclipref_dur, (float?)this.Duration));
		}

		#endregion

	}

	#endregion

}
