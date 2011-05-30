using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TESharedAnnotations;

namespace Animator.Core.Model.Sessions
{

	#region SessionClipReference

	public sealed class SessionClipReference : ClipReference
	{

		#region Static / Constant

		#endregion

		#region Fields

		private int? _Row;
		private bool _IsActive;

		#endregion

		#region Properties

		public int? Row
		{
			get { return this._Row; }
			set
			{
				if(value != this._Row)
				{
					this._Row = value;
					this.OnPropertyChanged("Row");
				}
			}
		}

		public bool IsActive
		{
			get { return this._IsActive; }
			set
			{
				if(value != this._IsActive)
				{
					this._IsActive = value;
					this.OnPropertyChanged("IsActive");
				}
			}
		}

		#endregion

		#region Constructors

		public SessionClipReference(Clip clip)
			: this(Guid.NewGuid(), clip) {}

		public SessionClipReference(Guid id, Clip clip)
			: base(id, clip) { }

		public SessionClipReference([NotNull] XElement element, [CanBeNull] Document document)
			: base(element, document)
		{
			this.Row = (int?)element.Attribute(Schema.sesclipref_row);
		}

		#endregion

		#region Methods

		internal override bool IsActiveInternal(Transport.Transport transport)
		{
			return this.IsActive;
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.sesclipref)
				.WithContent(ModelUtil.WriteOptionalValueAttribute(Schema.sesclipref_row, this.Row));
		}

		#endregion

	}

	#endregion

}
