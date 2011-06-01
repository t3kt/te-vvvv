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

	public abstract class ClipReference : DocumentItem, IEquatable<ClipReference>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Clip _Clip;

		#endregion

		#region Properties

		[Browsable(false)]
		public Guid ClipId
		{
			get { return this._Clip.Id; }
		}

		public Clip Clip
		{
			get { return this._Clip; }
		}

		#endregion

		#region Constructors

		protected ClipReference(Guid id, [NotNull] Clip clip)
			: base(id)
		{
			Require.ArgNotNull(clip, "clip");
			this._Clip = clip;
		}

		protected ClipReference([NotNull] XElement element, [NotNull]Document document)
			: base(element)
		{
			Require.ArgNotNull(document, "document");
			var clipId = (Guid)element.Attribute(Schema.clipref_clip_id);
			this._Clip = document.GetClip(clipId);
			if(this._Clip == null)
				throw new ModelException(String.Format("Clip not found: {0}", clipId));
		}

		#endregion

		#region Methods

		internal abstract bool IsActiveInternal(Transport.Transport transport);

		protected virtual object GetValue(Transport.Transport transport)
		{
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
			return ReferenceEquals(this._Clip, other._Clip);
		}

		#endregion

	}

	#endregion

}
