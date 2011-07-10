using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model.Sessions
{

	#region Session

	public sealed class Session : DocumentSection<SessionTrack>
	{

		#region Static / Constant

		private static readonly ItemTypeInfo _ItemType = new ItemTypeInfo(typeof(Session));

		#endregion

		#region Fields

		private int? _Rows;

		#endregion

		#region Properties

		public override ItemTypeInfo ItemType
		{
			get { return _ItemType; }
		}

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

		public Session(Guid id)
			: base(id) { }

		public Session()
			: this(Guid.NewGuid()) { }

		public Session([NotNull] XElement element, [NotNull] Document document, [CanBeNull]AniHost host)
			: base(element)
		{
			this.Rows = (int?)element.Attribute(Schema.session_rows);
			this.Tracks.AddRange(element.Elements(Schema.seqtrack).Select(e => new SessionTrack(e, document, host)));
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
