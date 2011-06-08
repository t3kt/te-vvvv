using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model.Sessions
{

	#region Session

	public sealed class Session : DocumentSection<SessionTrack>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private int? _Rows;

		#endregion

		#region Properties

		public int? Rows
		{
			get { return this._Rows; }
			set
			{
				if(value != this._Rows)
				{
					this._Rows = value;
					this.OnPropertyChanged("Rows");
				}
			}
		}

		#endregion

		#region Constructors

		public Session(Guid id, [NotNull]Document document)
			: base(id, document) { }

		public Session([NotNull]Document document)
			: this(Guid.NewGuid(), document) { }

		public Session([NotNull] XElement element, [NotNull] Document document)
			: base(element, document)
		{
			this.Rows = (int?)element.Attribute(Schema.session_rows);
			this.Tracks.AddRange(element.Elements(Schema.seqtrack).Select(e => new SessionTrack(e, document)));
		}

		#endregion

		#region Methods

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.session,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteOptionalValueAttribute(Schema.session_rows, this.Rows),
				ModelUtil.WriteXElements(this.Tracks));
		}

		#endregion

	}

	#endregion

}
