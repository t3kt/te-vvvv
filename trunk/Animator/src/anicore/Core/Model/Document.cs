using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region Document

	public abstract class Document : IDocument
	{

		#region Static / Constant

		internal const int NoAlignment = -1;

		#endregion

		#region Fields

		#endregion

		#region Properties

		public virtual Guid Id { get; protected set; }

		public virtual string Name { get; set; }

		public virtual Time Duration { get; set; }

		public virtual float BeatsPerMinute { get; set; }

		public virtual int TriggerAlignment { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected virtual void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.Id = (Guid)element.Attribute(Schema.anidoc_id);
			this.Name = (string)element.Attribute(Schema.anidoc_name);
			this.Duration = (float?)element.Attribute(Schema.anidoc_dur) ?? Time.Infinite;
			this.BeatsPerMinute = (float)element.Attribute(Schema.anidoc_bpm);
			Debug.Assert(this.BeatsPerMinute > 0);
			this.TriggerAlignment = (int?)element.Attribute(Schema.anidoc_align) ?? NoAlignment;
			this.Outputs.ReplaceAll(element.Elements(Schema.output).Select(this.ReadOutputXElement));
			this.Tracks.ReplaceAll(element.Elements(Schema.track).Select(this.ReadTrackXElement));
		}

		protected virtual IOutput ReadOutputXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			return Output.ReadOutputXElement(this, this, element);
		}

		protected virtual ITrack ReadTrackXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			return Track.ReadTrackXElement(this, this, element);
		}

		#endregion

		#region IDocumentItem Members

		IDocumentItem IDocumentItem.Parent
		{
			get { return null; }
		}

		IDocument IDocumentItem.Document
		{
			get { return this; }
		}

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

		#region IOutputContainer Members

		internal abstract DocumentItemCollection<IOutput> Outputs { get; }

		IEnumerable<IOutput> IOutputContainer.Outputs
		{
			get { return this.Outputs; }
		}

		public IOutput GetOutput(Guid id)
		{
			return this.Outputs.GetItem(id);
		}

		public void AddOutput(IOutput output)
		{
			this.Outputs.Add(output);
		}

		public void RemoveOutput(Guid id)
		{
			this.Outputs.Remove(id);
		}

		#endregion

		#region ITrackContainer Members

		internal abstract DocumentItemCollection<ITrack> Tracks { get; }

		IEnumerable<ITrack> ITrackContainer.Tracks
		{
			get { return this.Tracks; }
		}

		public ITrack GetTrack(Guid id)
		{
			return this.Tracks.GetItem(id);
		}

		public void AddTrack(ITrack track)
		{
			this.Tracks.Add(track);
		}

		public void RemoveTrack(Guid id)
		{
			this.Tracks.Remove(id);
		}

		#endregion

		#region IDocumentItemContainer Members

		public IDocumentItem GetItem(Guid id)
		{
			if(id == this.Id)
				return this;
			IOutput output;
			if(this.Outputs.TryGetItem(id, out output))
				return output;
			ITrack track;
			if(this.Tracks.TryGetItem(id, out track))
				return track;
			IClip clip;
			if(this.Clips.TryGetItem(id, out clip))
				return clip;
			return null;
		}

		#endregion

	}

	#endregion

}
