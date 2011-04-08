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

	public class Track : DocumentItem, ITrack
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Guid? _OutputId;
		private IOutput _Output;
		private readonly DocumentItemCollection<IClip> _Clips;

		#endregion

		#region Properties

		public Guid? OutputId
		{
			get { return _OutputId; }
			set
			{
				//if(value != _OutputId)
				{
					_OutputId = value;
					this.Output = value == null ? null : this.Document.GetOutput(value.Value);
				}
			}
		}

		public IOutput Output
		{
			get { return _Output; }
			set
			{
				if(value != _Output)
				{
					_Output = value;
					_OutputId = value == null ? (Guid?)null : value.Id;
					OnOutputChanged();
				}
			}
		}

		#endregion

		#region Constructors

		protected Track(IDocumentItem parent)
		{
			this.Parent = parent;
			_Clips = new DocumentItemCollection<IClip>(this);
		}

		public Track(IDocumentItem parent, Guid id)
			: this(parent)
		{
			this.Id = id;
		}

		public Track(IDocumentItem parent, XElement element)
			: this(parent)
		{
			ReadXElement(element);
		}

		#endregion

		#region Methods

		protected virtual void OnOutputChanged()
		{
			OnPropertyChanged("Output");
		}

		protected void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.Id = (Guid)element.Attribute(Schema.track_id);
			this.Name = (string)element.Attribute(Schema.track_name);
			_OutputId = (Guid?)element.Attribute(Schema.track_output);
			this.Output = _OutputId == null ? null : this.Document.GetOutput(_OutputId.Value);
			this.Clips.ReplaceAll(element.Elements(Schema.clip).Select(this.ReadClipXElement));
		}

		protected virtual IClip ReadClipXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			return new Clip(this, element);
		}

		public override XElement WriteXElement(XName name)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IClipContainer Members

		internal DocumentItemCollection<IClip> Clips
		{
			get { return _Clips; }
		}

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

		public override IDocumentItem GetItem(Guid id)
		{
			return base.GetItem(id) ?? this.GetClip(id);
		}

		#endregion

	}

	#endregion

}
