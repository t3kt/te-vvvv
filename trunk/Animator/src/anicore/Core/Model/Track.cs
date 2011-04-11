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

	public class Track : DocumentItem, IClipContainer
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Guid? _OutputId;
		private readonly DocumentItemCollection<Clip> _Clips;

		#endregion

		#region Properties

		public Guid? OutputId
		{
			get { return _OutputId; }
			set
			{
				if(value != _OutputId)
				{
					_OutputId = value;
					this.OnPropertyChanged("OutputId");
				}
			}
		}

		#endregion

		#region Constructors

		protected Track(IDocumentItem parent)
		{
			this.Parent = parent;
			_Clips = new DocumentItemCollection<Clip>(this);
		}

		public Track(IDocumentItem parent, Guid id)
			: this(parent)
		{
			this.Id = id;
		}

		public Track(Document document, XElement element)
			: this(document)
		{
			ReadXElement(document, element);
		}

		#endregion

		#region Methods

		protected void ReadXElement(Document document, XElement element)
		{
			Require.ArgNotNull(document, "document");
			Require.ArgNotNull(element, "element");
			try
			{
				SuspendNotify();
				this.Id = (Guid)element.Attribute(Schema.track_id);
				this.Name = (string)element.Attribute(Schema.track_name);
				this.OutputId = (Guid?)element.Attribute(Schema.track_output);
				this.Clips.ReplaceAll(element.Elements(Schema.clip).Select(e => document.CreateClip(this, e)));
			}
			finally
			{
				ResumeNotify();
				OnPropertyChanged(null);
			}
		}

		protected virtual Clip ReadClipXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			return new Clip(this, element);
		}

		public override XElement WriteXElement(XName name)
		{
			return new XElement(name ?? Schema.track,
				new XAttribute(Schema.track_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.track_name, this.Name),
				ModelUtil.WriteOptionalValueAttribute(Schema.track_output, this.OutputId),
				this.Clips.WriteXElements(null));
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing)
			{
				_Clips.Dispose();
			}
		}

		#endregion

		#region IClipContainer Members

		internal DocumentItemCollection<Clip> Clips
		{
			get { return _Clips; }
		}

		IEnumerable<Clip> IClipContainer.Clips
		{
			get { return this.Clips; }
		}

		public Clip GetClip(Guid id)
		{
			return this.Clips.GetItem(id);
		}

		public void AddClip(Clip clip)
		{
			this.Clips.Add(clip);
		}

		public void RemoveClip(Guid id)
		{
			this.Clips.Remove(id);
		}

		#endregion

		#region IDocumentItemContainer Members

		public override IDocumentItem GetItem(Guid id)
		{
			return base.GetItem(id) ?? this.GetClip(id);
		}

		#endregion

	}

	#endregion

}
