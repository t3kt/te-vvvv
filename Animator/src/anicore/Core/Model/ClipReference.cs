using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region ClipReference

	public class ClipReference : DocumentItem, IEquatable<ClipReference>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Guid _ClipId;
		private readonly Clip _Clip;

		#endregion

		#region Properties

		[Browsable(false)]
		public Guid ClipId
		{
			get { return this._ClipId; }
		}

		public Clip Clip
		{
			get { return this._Clip; }
		}

		#endregion

		#region Constructors

		public ClipReference(Guid id, Clip clip)
			: base(id)
		{
			this._Clip = clip;
			this._ClipId = clip == null ? Guid.Empty : clip.Id;
		}

		public ClipReference([NotNull] XElement element, [CanBeNull]Document document)
			: base(element)
		{
			this._ClipId = (Guid)element.Attribute(Schema.clipref_clip_id);
			if(document != null)
				this._Clip = document.GetClip(this._ClipId);
		}

		#endregion

		#region Methods

		protected virtual object GetValue(Transport.Transport transport, Clip clip)
		{
			if(clip == null)
				return null;
			throw new NotImplementedException();
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.clipref,
				this.WriteCommonXAttributes(),
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
