using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region Document

	public class Document : IDocument, INotifyPropertyChanged, ISuspendableNotify
	{

		#region Static / Constant

		internal const int NoAlignment = 0;
		internal const float DefaultBeatsPerMinute = 80.0f;

		#endregion

		#region Fields

		private readonly DocumentItemCollection<Output> _Outputs;
		private readonly DocumentItemCollection<Track> _Tracks;
		private bool _NotifySuspended;
		private Guid _Id;
		private string _Name;
		private Time _Duration = Time.Infinite;
		private float _BeatsPerMinute = DefaultBeatsPerMinute;
		private int _TriggerAlignment = NoAlignment;

		#endregion

		#region Properties

		public Guid Id
		{
			get { return _Id; }
			protected set
			{
				if(value != _Id)
				{
					_Id = value;
					OnPropertyChanged("Id");
				}
			}
		}

		public string Name
		{
			get { return _Name; }
			set
			{
				if(value != _Name)
				{
					_Name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		public Time Duration
		{
			get { return _Duration; }
			set
			{
				if(value != _Duration)
				{
					_Duration = value;
					OnPropertyChanged("Duration");
				}
			}
		}

		public float BeatsPerMinute
		{
			get { return _BeatsPerMinute; }
			set
			{
				if(value != _BeatsPerMinute)
				{
					_BeatsPerMinute = value;
					OnPropertyChanged("BeatsPerMinute");
				}
			}
		}

		public int TriggerAlignment
		{
			get { return _TriggerAlignment; }
			set
			{
				if(value != _TriggerAlignment)
				{
					_TriggerAlignment = value;
					OnPropertyChanged("TriggerAlignment");
				}
			}
		}

		protected bool NotifySuspended
		{
			get { return _NotifySuspended; }
		}

		#endregion

		#region Constructors

		public Document()
		{
			_Outputs = new DocumentItemCollection<Output>(this);
			_Tracks = new DocumentItemCollection<Track>(this);
			this.Id = Guid.NewGuid();
		}

		public Document(XElement element)
			: this()
		{
			ReadXElement(element);
		}

		#endregion

		#region Methods

		protected void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			SuspendNotify();
			try
			{
				this.Id = (Guid)element.Attribute(Schema.anidoc_id);
				this.Name = (string)element.Attribute(Schema.anidoc_name);
				this.Duration = (float?)element.Attribute(Schema.anidoc_dur) ?? Time.Infinite;
				this.BeatsPerMinute = (float?)element.Attribute(Schema.anidoc_bpm) ?? DefaultBeatsPerMinute;
				this.TriggerAlignment = (int?)element.Attribute(Schema.anidoc_align) ?? NoAlignment;
				this.Outputs.ReplaceAll(element.Elements(Schema.output).Select(this.CreateOutput));
				this.Tracks.ReplaceAll(element.Elements(Schema.track).Select(this.CreateTrack));
			}
			finally
			{
				ResumeNotify();
				OnPropertyChanged(null);
			}
		}

		internal void SuspendNotify()
		{
			_NotifySuspended = true;
		}

		internal void ResumeNotify()
		{
			_NotifySuspended = false;
		}

		#endregion

		#region Item Instantiation

		internal event EventHandler<ItemInstantiationEventArgs> ItemInstantiated;

		internal event EventHandler<ItemInstantiationEventArgs<Output>> OutputInstantiated;

		internal event EventHandler<ItemInstantiationEventArgs<Track>> TrackInstantiated;

		internal event EventHandler<ItemInstantiationEventArgs<Clip>> ClipInstantiated;

		private void OnItemInstantiated(IDocumentItem parent, DocumentItem item)
		{
			var handler = this.ItemInstantiated;
			if(handler != null)
				handler(this, new ItemInstantiationEventArgs(this, parent, item));
		}

		private void OnOutputInstantiated(Output output)
		{
			var handler = this.OutputInstantiated;
			if(handler != null)
				handler(this, new ItemInstantiationEventArgs<Output>(this, this, output));
			this.OnItemInstantiated(this, output);
		}

		private void OnTrackInstantiated(Track track)
		{
			var handler = this.TrackInstantiated;
			if(handler != null)
				handler(this, new ItemInstantiationEventArgs<Track>(this, this, track));
			this.OnItemInstantiated(this, track);
		}

		private void OnClipInstantiated(Track track, Clip clip)
		{
			var handler = this.ClipInstantiated;
			if(handler != null)
				handler(this, new ItemInstantiationEventArgs<Clip>(this, track, clip));
			this.OnItemInstantiated(track, clip);
		}

		internal Output CreateOutput(Guid id)
		{
			var output = new Output(this, id);
			this.OnOutputInstantiated(output);
			return output;
		}

		internal Output CreateOutput(XElement element)
		{
			var output = new Output(this, element);
			this.OnOutputInstantiated(output);
			return output;
		}

		internal Track CreateTrack(Guid id)
		{
			var track = new Track(this, id);
			this.OnTrackInstantiated(track);
			return track;
		}

		internal Track CreateTrack(XElement element)
		{
			var track = new Track(this, element);
			this.OnTrackInstantiated(track);
			return track;
		}

		internal Clip CreateClip(Track track, Guid id)
		{
			var clip = new Clip(track, id);
			this.OnClipInstantiated(track, clip);
			return clip;
		}

		internal Clip CreateClip(Track track, XElement element)
		{
			var clip = new Clip(track, element);
			this.OnClipInstantiated(track, clip);
			return clip;
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

		public IEnumerable<IDocumentItem> Children
		{
			get
			{
				return this.Outputs.Cast<IDocumentItem>()
					.Concat(this.Tracks.Cast<IDocumentItem>());
			}
		}

		public IDocumentItem GetItem(Guid id)
		{
			if(id == this.Id)
				return this;
			//IOutput output;
			//if(this.Outputs.TryGetItem(id, out output))
			//    return output;
			//ITrack track;
			//if(this.Tracks.TryGetItem(id, out track))
			//    return track;
			//return this.GetClip(id);
			return this.GetOutput(id) ?? this.GetTrack(id) ?? (IDocumentItem)this.GetClip(id);
		}

		#endregion

		#region IClipContainer Members

		public IEnumerable<Clip> Clips
		{
			get { return this.Tracks.SelectMany(t => t.Clips); }
		}

		public Clip GetClip(Guid id)
		{
			return this.Clips.FindById(id);
		}

		void IClipContainer.AddClip(Clip clip)
		{
			throw new NotSupportedException();
		}

		void IClipContainer.RemoveClip(Guid id)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region IOutputContainer Members

		internal DocumentItemCollection<Output> Outputs
		{
			get { return _Outputs; }
		}

		IEnumerable<Output> IOutputContainer.Outputs
		{
			get { return this.Outputs; }
		}

		public Output GetOutput(Guid id)
		{
			return this.Outputs.GetItem(id);
		}

		public void AddOutput(Output output)
		{
			this.Outputs.Add(output);
		}

		public void RemoveOutput(Guid id)
		{
			this.Outputs.Remove(id);
		}

		#endregion

		#region ITrackContainer Members

		internal DocumentItemCollection<Track> Tracks
		{
			get { return _Tracks; }
		}

		IEnumerable<Track> ITrackContainer.Tracks
		{
			get { return this.Tracks; }
		}

		public Track GetTrack(Guid id)
		{
			return this.Tracks.GetItem(id);
		}

		public void AddTrack(Track track)
		{
			this.Tracks.Add(track);
		}

		public void RemoveTrack(Guid id)
		{
			this.Tracks.Remove(id);
		}

		#endregion

		#region IXElementWritable Members

		public virtual XElement WriteXElement(XName name)
		{
			return new XElement(name ?? Schema.anidoc,
								new XAttribute(Schema.anidoc_id, this.Id),
								ModelUtil.WriteOptionalAttribute(Schema.anidoc_name, this.Name),
								this.Duration == Time.Infinite ? null : new XAttribute(Schema.anidoc_dur, (float)this.Duration),
								this.TriggerAlignment == NoAlignment ? null : new XAttribute(Schema.anidoc_align, this.TriggerAlignment),
								this.Outputs.WriteXElements(null),
								this.Tracks.WriteXElements(null));
		}

		#endregion

		#region INotifyPropertyChanged Members

		protected virtual void OnPropertyChanged(string name)
		{
			if(!_NotifySuspended)
			{
				var handler = this.PropertyChanged;
				if(handler != null)
					handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region ISuspendableNotify Members

		void ISuspendableNotify.SuspendNotify()
		{
			SuspendNotify();
		}

		void ISuspendableNotify.ResumeNotify()
		{
			ResumeNotify();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.SuspendNotify();
			this.PropertyChanged = null;
			_Tracks.Dispose();
			_Outputs.Dispose();
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
