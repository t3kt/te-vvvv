using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Common.Diagnostics;
using Animator.Core.Model;

namespace Animator.Core.Runtime
{

	#region RuntimeDocument

	internal sealed class RuntimeDocument : IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly RuntimeTransport _Transport;
		private readonly Document _Document;
		private readonly Dictionary<Guid, RuntimeOutput> _Outputs;
		private readonly Dictionary<Guid, RuntimeTrack> _Tracks;
		private readonly Dictionary<Guid, RuntimeClip> _Clips;

		#endregion

		#region Properties

		public RuntimeTransport Transport
		{
			get { return this._Transport; }
		}

		public Document Document
		{
			get { return this._Document; }
		}

		#endregion

		#region Constructors

		public RuntimeDocument(RuntimeTransport transport, Document document)
		{
			Require.ArgNotNull(transport, "transport");
			Require.ArgNotNull(document, "document");
			this._Transport = transport;
			this._Document = document;
			this._Outputs = new Dictionary<Guid, RuntimeOutput>();
			this._Tracks = new Dictionary<Guid, RuntimeTrack>();
			this._Clips = new Dictionary<Guid, RuntimeClip>();
			this.AttachHandlers();
			//foreach(var item in document.Descendents())
			//    this.AttachItem((DocumentItem)item);
			foreach(var output in document.Outputs)
				this.AttachOutput(output);
			foreach(var track in document.Tracks)
			{
				this.AttachTrack(track);
				foreach(var clip in track.Clips)
					this.AttachClip(clip);
			}
		}

		#endregion

		#region Methods

		private void AttachHandlers()
		{
			this._Document.OutputInstantiated += this.Document_OutputInstantiated;
			this._Document.TrackInstantiated += this.Document_TrackInstantiated;
			this._Document.ClipInstantiated += this.Document_ClipInstantiated;
		}

		private void DetachHandlers()
		{
			this._Document.OutputInstantiated -= this.Document_OutputInstantiated;
			this._Document.TrackInstantiated -= this.Document_TrackInstantiated;
			this._Document.ClipInstantiated -= this.Document_ClipInstantiated;
		}

		public RuntimeOutput GetOutput(Guid id)
		{
			return this._Outputs[id];
		}

		public RuntimeTrack GetTrack(Guid id)
		{
			return this._Tracks[id];
		}

		public RuntimeClip GetClip(Guid id)
		{
			return this._Clips[id];
		}

		public void AttachOutput(Output output)
		{
			Require.ArgNotNull(output, "output");
			this._Outputs.Add(output.Id, RuntimeOutput.CreateOutput(this, output));
		}

		public void AttachTrack(Track track)
		{
			Require.ArgNotNull(track, "track");
			this._Tracks.Add(track.Id, RuntimeTrack.CreateTrack(this, track));
		}

		public void AttachClip(Clip clip)
		{
			Require.ArgNotNull(clip, "clip");
			this._Clips.Add(clip.Id, RuntimeClip.CreateClip(this, clip));
		}

		public void AttachItem(DocumentItem item)
		{
			Require.ArgNotNull(item, "item");
			if(item is Output)
				this.AttachOutput((Output)item);
			else if(item is Track)
				this.AttachTrack((Track)item);
			else if(item is Clip)
				this.AttachClip((Clip)item);
			else
				throw new NotSupportedException();
		}

		public RuntimeDocumentItem GetItem(Guid id)
		{
			RuntimeOutput output;
			if(this._Outputs.TryGetValue(id, out output))
				return output;
			RuntimeTrack track;
			if(this._Tracks.TryGetValue(id, out track))
				return track;
			RuntimeClip clip;
			if(this._Clips.TryGetValue(id, out clip))
				return clip;
			throw new KeyNotFoundException();
		}

		private void Document_OutputInstantiated(object sender, ItemInstantiationEventArgs<Output> e)
		{
			this.AttachOutput(e.Item);
		}

		private void Document_TrackInstantiated(object sender, ItemInstantiationEventArgs<Track> e)
		{
			this.AttachTrack(e.Item);
		}

		private void Document_ClipInstantiated(object sender, ItemInstantiationEventArgs<Clip> e)
		{
			this.AttachClip(e.Item);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.DetachHandlers();
			foreach(var output in this._Outputs.Values)
				output.Dispose();
			foreach(var track in this._Tracks.Values)
				track.Dispose();
			foreach(var clip in this._Clips.Values)
				clip.Dispose();
			this._Outputs.Clear();
			this._Tracks.Clear();
			this._Clips.Clear();
			this._Document.Dispose();
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
