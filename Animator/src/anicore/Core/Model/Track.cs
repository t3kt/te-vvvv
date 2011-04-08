using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Model
{

	#region Track

	public abstract class Track : DocumentItem, ITrack
	{

		#region Static / Constant

		internal static ITrack ReadTrackXElement(IDocument document, IDocumentItem parent, XElement element)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Fields

		private Guid? _OutputId;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override void ReadXElement(XElement element)
		{
			base.ReadXElement(element);
			_OutputId = (Guid?)element.Attribute(Schema.track_output);
			this.Output = _OutputId == null ? null : this.Document.GetOutput(_OutputId.Value);
			this.Clips.ReplaceAll(element.Elements(Schema.clip).Select(this.ReadClipXElement));
		}

		protected virtual IClip ReadClipXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			return Clip.ReadClipXElement(this.Document, this, element);
		}

		#endregion

		#region ITrack Members

		public virtual IOutput Output { get; set; }

		#endregion

		#region IClipContainer Members

		internal abstract DocumentItemCollection<IClip> Clips { get; }

		IEnumerable<IClip> IClipContainer.Clips
		{
			get { return this.Clips; }
		}

		public IClip GetClip(Guid id)
		{
			return this.Clips.GetItem(id);
		}

		public void AddClip(IClip clip)
		{
			this.Clips.Add(clip);
		}

		public void RemoveClip(Guid id)
		{
			this.Clips.Remove(id);
		}

		#endregion

		#region IDocumentItemContainer Members

		public IDocumentItem GetItem(Guid id)
		{
			if(id == this.Id)
				return this;
			return this.Clips.GetItem(id);
		}

		#endregion

	}

	#endregion

}
