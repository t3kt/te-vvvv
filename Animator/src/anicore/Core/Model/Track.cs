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

	public sealed class Track : DocumentItem, IEquatable<Track>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Guid? _OutputId;
		private readonly DocumentItemCollection<Clip> _Clips;
		private string _TargetKey;

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

		public DocumentItemCollection<Clip> Clips
		{
			get { return _Clips; }
		}

		public string TargetKey
		{
			get { return this._TargetKey; }
			set
			{
				value = value.OrNullIfEmpty();
				if(value != this._TargetKey)
				{
					this._TargetKey = value;
					OnPropertyChanged("TargetKey");
				}
			}
		}

		#endregion

		#region Constructors

		private Track(IDocumentItem parent)
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

		private void ReadXElement(Document document, XElement element)
		{
			Require.ArgNotNull(document, "document");
			Require.ArgNotNull(element, "element");
			try
			{
				SuspendNotify();
				this.Id = (Guid)element.Attribute(Schema.track_id);
				this.Name = (string)element.Attribute(Schema.track_name);
				this.OutputId = (Guid?)element.Attribute(Schema.track_output);
				this.TargetKey = (string)element.Attribute(Schema.track_target);
				var clipsElement = element.Element(Schema.track_clips);
				this.Clips.ReplaceAll(clipsElement == null ? null : clipsElement.Elements().Select(e => document.CreateClip(this, e)));
			}
			finally
			{
				ResumeNotify();
				OnPropertyChanged(null);
			}
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.track,
								new XAttribute(Schema.track_id, this.Id),
								ModelUtil.WriteOptionalAttribute(Schema.track_name, this.Name),
								ModelUtil.WriteOptionalValueAttribute(Schema.track_output, this.OutputId),
								ModelUtil.WriteOptionalAttribute(Schema.track_target, this.TargetKey),
								this.Clips.Count == 0 ? null : new XElement(Schema.track_clips, this.Clips.WriteXElements(null)));
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

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing)
			{
				_Clips.Dispose();
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Track);
		}

		#endregion

		#region IEquatable<Track> Members

		public bool Equals(Track other)
		{
			if(!base.Equals(other))
				return false;
			return other._OutputId == this._OutputId &&
				   other._TargetKey == this._TargetKey &&
				   other._Clips.ItemsEqual(this._Clips);
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
