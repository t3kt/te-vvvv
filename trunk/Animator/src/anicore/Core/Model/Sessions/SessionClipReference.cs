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

		#endregion

		#region Constructors

		public SessionClipReference(Guid id, Clip clip)
			: base(id, clip) { }

		public SessionClipReference([NotNull] XElement element, [CanBeNull] Document document)
			: base(element, document)
		{
			this.Row = (int?)element.Attribute(Schema.sesclipref_row);
		}

		#endregion

		#region Methods

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.sesclipref)
				.WithContent(ModelUtil.WriteOptionalValueAttribute(Schema.sesclipref_row, this.Row));
		}

		#endregion

	}

	#endregion

}
