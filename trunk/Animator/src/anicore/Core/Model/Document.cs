using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
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

		private ObservableCollection<Output> _Outputs;
		private ObservableCollection<Track> _Tracks;
		private ObservableCollection<Clip> _Clips;
		private bool _NotifySuspended;
		private Guid _Id;
		private string _Name;
		private Time _Duration = Time.Infinite;
		private float _BeatsPerMinute = DefaultBeatsPerMinute;
		private int _TriggerAlignment = NoAlignment;
		private int? _UIRows;
		private int? _UIColumns;

		private Dictionary<Guid, IOutputTransmitter> _Transmitters;

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

		public ObservableCollection<Output> Outputs
		{
			get
			{
				if(this._Outputs == null)
				{
					this._Outputs = new ObservableCollection<Output>();
					this.AttachOutputs(this._Outputs);
				}
				return this._Outputs;
			}
			set
			{
				if(value != this._Outputs)
				{
					this.DetachOutputs(this._Outputs);
					this._Outputs = value;
					this.AttachOutputs(this._Outputs);
					this.OnPropertyChanged("Outputs");
				}
			}
		}

		public ObservableCollection<Track> Tracks
		{
			get
			{
				if(this._Tracks == null)
				{
					this._Tracks = new ObservableCollection<Track>();
					this.AttachTracks(this._Tracks);
				}
				return this._Tracks;
			}
			set
			{
				if(value != this._Tracks)
				{
					this.DetachTracks(this._Tracks);
					this._Tracks = value;
					this.AttachTracks(this._Tracks);
					this.OnPropertyChanged("Tracks");
				}
			}
		}

		public ObservableCollection<Clip> Clips
		{
			get
			{
				if(this._Clips == null)
				{
					this._Clips = new ObservableCollection<Clip>();
					this.AttachClips(this._Clips);
				}
				return this._Clips;
			}
			set
			{
				if(value != this._Clips)
				{
					this.DetachClips(this._Clips);
					this._Clips = value;
					this.AttachClips(value);
					this.OnPropertyChanged("Clips");
				}
			}
		}

		public int? UIRows
		{
			get { return this._UIRows; }
			set
			{
				if(value != this._UIRows)
				{
					this._UIRows = value;
					this.OnPropertyChanged("UIRows");
				}
			}
		}

		public int? UIColumns
		{
			get { return this._UIColumns; }
			set
			{
				if(value != this._UIColumns)
				{
					this._UIColumns = value;
					this.OnPropertyChanged("UIColumns");
				}
			}
		}

		public IEnumerable<Clip> ActiveClips
		{
			get { return this._Clips == null ? Enumerable.Empty<Clip>() : this._Clips.Where(c => c.IsPlaying); }
		}

		#endregion

		#region Constructors

		public Document()
			: this(Guid.NewGuid())
		{
		}

		public Document(Guid id)
		{
			this.Id = id;
		}

		public Document(XElement element)
		{
			ReadXElement(element);
		}

		#endregion

		#region Methods

		private void RemoveUnusedTransmitters()
		{
			if(this._Transmitters != null)
			{
				if(this._Outputs == null)
				{
					foreach(var t in this._Transmitters.Values)
						t.Dispose();
					this._Transmitters.Clear();
				}
				else
				{
					var unused = this._Transmitters.Where(t => !this._Outputs.Any(o => o.Id == t.Key)).ToList();
					foreach(var t in unused)
					{
						t.Value.Dispose();
						this._Transmitters.Remove(t.Key);
					}
				}
			}
		}

		private void AttachOutputs(ObservableCollection<Output> outputs)
		{
			if(outputs != null)
				outputs.CollectionChanged += this.Outputs_CollectionChanged;
		}

		private void DetachOutputs(ObservableCollection<Output> outputs)
		{
			if(outputs != null)
				outputs.CollectionChanged -= this.Outputs_CollectionChanged;
		}

		private void Outputs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Outputs");
			this.RemoveUnusedTransmitters();
		}

		private void AttachTracks(ObservableCollection<Track> tracks)
		{
			if(tracks != null)
				tracks.CollectionChanged += this.Tracks_CollectionChanged;
		}

		private void DetachTracks(ObservableCollection<Track> tracks)
		{
			if(tracks != null)
				tracks.CollectionChanged -= this.Tracks_CollectionChanged;
		}

		private void Tracks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Tracks");
		}

		private void AttachClips(ObservableCollection<Clip> clips)
		{
			if(clips != null)
				clips.CollectionChanged += this.Clips_CollectionChanged;
		}

		private void DetachClips(ObservableCollection<Clip> clips)
		{
			if(clips != null)
				clips.CollectionChanged -= this.Clips_CollectionChanged;
		}

		private void Clips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Clips");
		}

		internal void SuspendNotify()
		{
			this._NotifySuspended = true;
		}

		internal void ResumeNotify()
		{
			this._NotifySuspended = false;
		}

		public Output GetOutput(Guid id)
		{
			return this.Outputs.FindById(id);
		}

		public Track GetTrack(Guid id)
		{
			return this.Tracks.FindById(id);
		}

		public Clip GetClip(Guid id)
		{
			return this.Clips.FindById(id);
		}

		public IDocumentItem GetItem(Guid id)
		{
			if(id == this.Id)
				return this;
			return this.GetOutput(id) ?? this.GetTrack(id) ?? (DocumentItem)this.GetClip(id);
		}

		public IOutputTransmitter GetTransmitter(Guid id)
		{
			IOutputTransmitter transmitter;
			if(this._Transmitters == null)
				this._Transmitters = new Dictionary<Guid, IOutputTransmitter>();
			if(this._Transmitters.TryGetValue(id, out transmitter))
				return transmitter;
			var output = this.GetOutput(id);
			if(output != null)
			{
				transmitter = OutputTransmitter.CreateTransmitter(output);
				this._Transmitters.Add(id, transmitter);
				return transmitter;
			}
			return null;
		}

		internal void PostClipOutput(Clip clip, ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			if(clip == null || !clip.IsPlaying || clip.OutputId == null)
				return;
			var transmitter = this.GetTransmitter(clip.OutputId.Value);
			if(transmitter == null)
				return;
			var message = clip.BuildOutputMessage(transport);
			transmitter.PostMessage(message);
		}

		public void PostActiveClipOutputs(ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			foreach(var clip in this.ActiveClips.ToArray())
				this.PostClipOutput(clip, transport);
		}

		private void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.SuspendNotify();
			try
			{
				this.Id = (Guid)element.Attribute(Schema.anidoc_id);
				this.Name = (string)element.Attribute(Schema.anidoc_name);
				this.Duration = (float?)element.Attribute(Schema.anidoc_dur) ?? Time.Infinite;
				this.BeatsPerMinute = (float?)element.Attribute(Schema.anidoc_bpm) ?? DefaultBeatsPerMinute;
				this.TriggerAlignment = (int?)element.Attribute(Schema.anidoc_align) ?? NoAlignment;
				var outputsElement = element.Element(Schema.anidoc_outputs);
				this.Outputs = outputsElement == null ? null : new ObservableCollection<Output>(outputsElement.Elements().Select(e => new Output(e)));
				var tracksElement = element.Element(Schema.anidoc_tracks);
				this.Tracks = tracksElement == null ? null : new ObservableCollection<Track>(tracksElement.Elements().Select(e => new Track(e)));
				var clipsElement = element.Element(Schema.anidoc_clips);
				this.Clips = clipsElement == null ? null : new ObservableCollection<Clip>(clipsElement.Elements().Select(Clip.ReadClipXElement));
				this.UIRows = (int?)element.Attribute(Schema.anidoc_ui_rows);
				this.UIColumns = (int?)element.Attribute(Schema.anidoc_ui_cols);
			}
			finally
			{
				this.ResumeNotify();
				this.OnPropertyChanged(null);
			}
		}

		#endregion

		#region IXElementWritable Members

		public XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.anidoc,
				new XAttribute(Schema.anidoc_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.anidoc_name, this.Name),
				this.Duration == Time.Infinite ? null : new XAttribute(Schema.anidoc_dur, (float)this.Duration),
				new XAttribute(Schema.anidoc_bpm, this.BeatsPerMinute),
				this.TriggerAlignment == NoAlignment ? null : new XAttribute(Schema.anidoc_align, this.TriggerAlignment),
				(this._Outputs == null || this._Outputs.Count == 0) ? null : new XElement(Schema.anidoc_outputs, this._Outputs.WriteXElements(null)),
				(this._Tracks == null || this._Tracks.Count == 0) ? null : new XElement(Schema.anidoc_tracks, this._Tracks.WriteXElements(null)),
				(this._Clips == null || this._Clips.Count == 0) ? null : new XElement(Schema.anidoc_clips, this._Clips.WriteXElements(null)),
				ModelUtil.WriteOptionalValueAttribute(Schema.anidoc_ui_rows, this.UIRows),
				ModelUtil.WriteOptionalValueAttribute(Schema.anidoc_ui_cols, this.UIColumns));
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
			this.SuspendNotify();
		}

		void ISuspendableNotify.ResumeNotify()
		{
			this.ResumeNotify();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.SuspendNotify();
			this.PropertyChanged = null;
			if(this._Tracks != null)
			{
				foreach(var output in this._Tracks)
					output.Dispose();
				this.DetachTracks(this._Tracks);
				this._Tracks = null;
			}
			if(this._Outputs != null)
			{
				foreach(var output in this._Outputs)
					output.Dispose();
				this.DetachOutputs(this._Outputs);
				this._Outputs = null;
			}
			if(this._Transmitters != null)
			{
				foreach(var t in this._Transmitters.Values)
					t.Dispose();
				this._Transmitters = null;
			}
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
