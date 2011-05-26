using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Model
{

	#region ClipReference

	public class ClipReference : DocumentItem, IEquatable<ClipReference>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Guid _ClipId;

		#endregion

		#region Properties

		[Browsable(false)]
		public Guid ClipId
		{
			get { return this._ClipId; }
			set
			{
				if(value != this._ClipId)
				{
					this._ClipId = value;
					this.OnPropertyChanged("ClipId");
				}
			}
		}

		#endregion

		#region Constructors

		public ClipReference(Guid id)
		{
			this.Id = id;
		}

		public ClipReference()
			: this(Guid.NewGuid()) { }

		public ClipReference(XElement element)
		{
			this.ReadXElement(element);
		}

		#endregion

		#region Methods

		protected void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.Id = (Guid)element.Attribute(Schema.clipref_id);
			this.Name = (string)element.Attribute(Schema.clipref_name);
			this.ClipId = (Guid)element.Attribute(Schema.clipref_clip_id);
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.clipref,
				new XAttribute(Schema.clipref_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.clipref_name, this.Name),
				new XAttribute(Schema.clipref_clip_id, this.ClipId));
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ClipReference);
		}

		#endregion

		#region IEquatable<ClipReference> Members

		public bool Equals(ClipReference other)
		{
			if(!base.Equals(other))
				return false;
			return this._ClipId == other._ClipId;
		}

		#endregion

	}

	#endregion

}
