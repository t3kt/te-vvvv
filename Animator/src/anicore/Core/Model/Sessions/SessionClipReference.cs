using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Animator.Core.Transport;
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
		private ClipState _State;

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

		public ClipState State
		{
			get { return this._State; }
			set
			{
				if(value != this._State)
				{
					this._State = value;
					this.OnPropertyChanged("State");
				}
			}
		}

		#endregion

		#region Constructors

		public SessionClipReference([NotNull] Clip clip)
			: this(Guid.NewGuid(), clip) { }

		public SessionClipReference(Guid id, [NotNull] Clip clip)
			: base(id, clip) { }

		public SessionClipReference([NotNull] XElement element, [NotNull] Document document)
			: base(element, document)
		{
			this.Row = (int?)element.Attribute(Schema.sesclipref_row);
			this.State = ModelUtil.ParseNullableEnum<ClipState>((string)element.Attribute(Schema.sesclipref_state)) ?? ClipState.Stopped;
		}

		#endregion

		#region Methods

		protected override Time GetPosition(Transport.Transport transport)
		{
			throw new NotImplementedException();
		}

		internal override bool IsActive(Transport.Transport transport)
		{
			return this.State == ClipState.Playing;
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.sesclipref)
				.WithContent(
					ModelUtil.WriteOptionalValueAttribute(Schema.sesclipref_row, this.Row),
					new XAttribute(Schema.sesclipref_state, this.State));
		}

		#endregion

	}

	#endregion

}
