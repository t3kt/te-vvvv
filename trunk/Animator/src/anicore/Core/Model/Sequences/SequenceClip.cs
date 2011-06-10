using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Model.Clips;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceClip

	public sealed class SequenceClip : ClipBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		private TimeSpan _Start;
		private TimeSpan _Duration;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Start
		{
			get { return this._Start; }
			set
			{
				Require.ArgNotNegative(value, "value");
				if(value != this._Start)
				{
					this._Start = value;
					this.OnPropertyChanged("Start");
				}
			}
		}

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Duration
		{
			get { return this._Duration; }
			set
			{
				Require.ArgNotNegative(value, "value");
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		internal TimeSpan End
		{
			get { return this._Start + this._Duration; }
		}

		#endregion

		#region Constructors

		public SequenceClip(Guid id)
			: base(id) { }

		public SequenceClip([NotNull] XElement element, [CanBeNull] AniHost host)
			: base(element, host)
		{
			this.Start = ModelUtil.ParseTimeSpan(element.Attribute(Schema.seqclip_start));
			this.Duration = ModelUtil.ParseTimeSpan(element.Attribute(Schema.seqclip_dur));
		}

		#endregion

		#region Methods

		internal override bool IsActive(Transport.Transport transport)
		{
			var pos = transport.Position;
			return !(pos < this.Start) && pos < this.End;
		}

		protected override float GetPosition(Transport.Transport transport)
		{
			throw new NotImplementedException();
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.seqclip,
				this.WriteCommonXAttributes(),
				new XAttribute(Schema.seqclip_start, this._Start),
				ModelUtil.WriteXAttribute(Schema.seqclip_dur, this._Duration),
				this.WritePropertiesXElement());
		}

		#endregion

	}

	#endregion

}
