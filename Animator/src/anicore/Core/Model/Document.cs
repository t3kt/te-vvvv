using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region Document

	public sealed class Document : IDocumentItem, ISuspendableNotify, INotifyPropertyChanged
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
			private set
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

		public DocumentItemCollection<Output> Outputs
		{
			get { return _Outputs; }
		}

		public DocumentItemCollection<Track> Tracks
		{
			get { return _Tracks; }
		}

		public IEnumerable<Clip> Clips
		{
			get { return this.Tracks.SelectMany(t => t.Clips); }
		}

		#endregion

		#region Constructors

		public Document()
			: this(Guid.NewGuid())
		{
		}

		public Document(Guid id)
		{
			this._Outputs = new DocumentItemCollection<Output>(this);
			this._Tracks = new DocumentItemCollection<Track>(this);
			this.Id = id;
		}

		public Document(XElement element)
			: this()
		{
			ReadXElement(element);
		}

		#endregion

		#region Methods

		private void ReadXElement(XElement element)
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
				var outputsElement = element.Element(Schema.anidoc_outputs);
				this.Outputs.ReplaceAll(outputsElement == null ? null : outputsElement.Elements().Select(this.CreateOutput));
				var tracksElement = element.Element(Schema.anidoc_tracks);
				this.Tracks.ReplaceAll(tracksElement == null ? null : tracksElement.Elements().Select(this.CreateTrack));
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

		public Clip GetClip(Guid id)
		{
			return this.Clips.FindById(id);
		}

		#endregion

		#region Item Instantiation

		internal event EventHandler<ItemInstantiationEventArgs> ItemInstantiated;

		internal event EventHandler<ItemInstantiationEventArgs<Output>> OutputInstantiated;

		internal event EventHandler<ItemInstantiationEventArgs<Track>> TrackInstantiated;

		internal event EventHandler<ItemInstantiationEventArgs<Clip>> ClipInstantiated;

		private void OnItemInstantiated(IDocumentItem parent, DocumentItem item)
		{
			var existingItem = this.GetItem(item.Id);
			if(existingItem != null)
				throw new ArgumentException(String.Format("Id '{0}' already exists in document", item.Id), "item");
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

		public Output CreateOutput(Guid id)
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

		public Track CreateTrack(Guid id)
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

		public Clip CreateClip(Track track, Guid id)
		{
			var clip = new Clip(track, id);
			this.OnClipInstantiated(track, clip);
			return clip;
		}

		internal Clip CreateClip(Track track, XElement element)
		{
			var clip = Clip.ReadClipXElement(track, element);
			this.OnClipInstantiated(track, clip);
			return clip;
		}

		public StepClip CreateStepClip(Track track, Guid id)
		{
			var clip = new StepClip(track, id);
			this.OnClipInstantiated(track, clip);
			return clip;
		}

		#endregion

		#region IDocumentItem Members

		IDocumentItem IDocumentItem.Parent
		{
			get { return null; }
		}

		Document IDocumentItem.Document
		{
			get { return this; }
		}

		public IEnumerable<IDocumentItem> Children
		{
			get
			{
				return this.Outputs.Concat(this.Tracks.Cast<IDocumentItem>());
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

		#region IXElementWritable Members

		public XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.anidoc,
								new XAttribute(Schema.anidoc_id, this.Id),
								ModelUtil.WriteOptionalAttribute(Schema.anidoc_name, this.Name),
								this.Duration == Time.Infinite ? null : new XAttribute(Schema.anidoc_dur, (float)this.Duration),
								this.TriggerAlignment == NoAlignment ? null : new XAttribute(Schema.anidoc_align, this.TriggerAlignment),
								this.Outputs.Count == 0 ? null : new XElement(Schema.anidoc_outputs, this.Outputs.WriteXElements(null)),
								this.Tracks.Count == 0 ? null : new XElement(Schema.anidoc_tracks, this.Tracks.WriteXElements(null)));
		}

		#endregion

		#region INotifyPropertyChanged Members

		private void OnPropertyChanged(string name)
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
