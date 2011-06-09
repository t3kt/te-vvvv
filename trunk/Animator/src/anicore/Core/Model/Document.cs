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
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model.Sequences;
using Animator.Core.Model.Sessions;
using Animator.Core.Runtime;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region Document

	public sealed class Document : IDocumentItem, INotifyPropertyChanged, IItemContainer<IDocumentItem>
	{

		#region TransportData

		private sealed class TransportData : IXElementWritable, INotifyPropertyChanged
		{

			#region Static/Constant

			#endregion

			#region Fields

			private float _BeatsPerMinute = DefaultBeatsPerMinute;
			private string _TransportType;
			private Dictionary<string, string> _Parameters;

			#endregion

			#region Properties

			public float BeatsPerMinute
			{
				get { return this._BeatsPerMinute; }
				set
				{
					Require.ArgPositive(value, "value");
					if(value != this._BeatsPerMinute)
					{
						this._BeatsPerMinute = value;
						this.OnPropertyChanged("BeatsPerMinute");
					}
				}
			}

			public string TransportType
			{
				get { return this._TransportType; }
				set
				{
					if(value != this._TransportType)
					{
						this._TransportType = value;
						this.OnPropertyChanged("TransportType");
					}
				}
			}

			public IDictionary<string, string> Parameters
			{
				get { return this._Parameters ?? (this._Parameters = new Dictionary<string, string>()); }
				set { this._Parameters = value == null ? null : value.ToDictionary(x => x.Key, x => x.Value); }
			}

			#endregion

			#region Constructors

			#endregion

			#region Methods

			public void ReadXElement(XElement element)
			{
				Require.ArgNotNull(element, "element");
				this._BeatsPerMinute = (float?)element.Attribute(Schema.transport_bpm) ?? DefaultBeatsPerMinute;
				this._TransportType = (string)element.Attribute(Schema.transport_type);
				this._Parameters = ModelUtil.ReadParametersXElement(element.Element(Schema.transport_params));
			}

			public XElement WriteXElement(XName name = null)
			{
				return new XElement(name ?? Schema.transport,
					new XAttribute(Schema.anidoc_bpm, this.BeatsPerMinute),
					ModelUtil.WriteOptionalAttribute(Schema.transport_type, this.TransportType),
					ModelUtil.WriteParametersXElement(Schema.transport_params, this._Parameters));
			}

			#endregion

			#region INotifyPropertyChanged Members

			public event PropertyChangedEventHandler PropertyChanged;

			private void OnPropertyChanged(string name)
			{
				var handler = this.PropertyChanged;
				if(handler != null)
					handler(this, new PropertyChangedEventArgs(name));
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		public const float DefaultBeatsPerMinute = 80.0f;

		private static readonly Transport.Transport _DefaultTransport = new Transport.Transport.NullTransport();

		#endregion

		#region Fields

		private readonly AniHost _Host;

		private readonly ObservableCollection<Output> _Outputs;
		private readonly ObservableCollection<Clip> _Clips;
		private readonly ObservableCollection<Sequence> _Sequences;
		private readonly ObservableCollection<Session> _Sessions;
		private Guid _Id;
		private string _Name;
		private int? _UIRows;
		private int? _UIColumns;
		private readonly TransportData _TransportData;
		private DocumentSection _ActiveSection;
		private Transport.Transport _Transport;

		#endregion

		#region Properties

		internal AniHost Host
		{
			get { return this._Host; }
		}

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

		[Category(TEShared.Names.Category_Common)]
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

		[Category(TEShared.Names.Category_Transport)]
		[DisplayName(TEShared.Names.DisplayName_BeatsPerMinute)]
		public float BeatsPerMinute
		{
			get { return this._TransportData.BeatsPerMinute; }
			set { this._TransportData.BeatsPerMinute = value; }
		}

		[Category(TEShared.Names.Category_Transport)]
		public string TransportType
		{
			get { return this._TransportData.TransportType; }
			set { this._TransportData.TransportType = value; }
		}

		[Category(TEShared.Names.Category_Transport)]
		[Browsable(false)]
		public IDictionary<string, string> TransportParameters
		{
			get { return this._TransportData.Parameters; }
			set { this._TransportData.Parameters = value; }
		}

		[Browsable(false)]
		public ObservableCollection<Output> Outputs
		{
			get { return this._Outputs; }
		}

		[Browsable(false)]
		public ObservableCollection<Clip> Clips
		{
			get { return this._Clips; }
		}

		[Browsable(false)]
		public ObservableCollection<Sequence> Sequences
		{
			get { return this._Sequences; }
		}

		[Browsable(false)]
		public ObservableCollection<Session> Sessions
		{
			get { return this._Sessions; }
		}

		[Category(TEShared.Names.Category_UI)]
		[DisplayName(TEShared.Names.DisplayName_SessionRows)]
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

		[Category(TEShared.Names.Category_UI)]
		[DisplayName(TEShared.Names.DisplayName_SessionColumns)]
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

		[Browsable(false)]
		public IEnumerable<Clip> ActiveClips
		{
			get { return this._Clips == null ? Enumerable.Empty<Clip>() : this._Clips.Where(c => c.IsPlaying); }
		}

		[Category(TEShared.Names.Category_Transport)]
		[Browsable(false)]
		public Transport.Transport Transport
		{
			get { return this._Transport ?? _DefaultTransport; }
		}

		public DocumentSection ActiveSection
		{
			get { return this._ActiveSection; }
			set
			{
				if(value != this._ActiveSection)
				{
					if(value is Session)
					{
						if(!this._Sessions.Contains((Session)value))
							throw new ModelException("Session is not from this document.");
					}
					else if(value is Sequence)
					{
						if(!this._Sequences.Contains((Sequence)value))
							throw new ModelException("Sequence is not from this document.");
					}
					this._ActiveSection = value;
					this.OnPropertyChanged("ActiveSection");
				}
			}
		}

		internal IEnumerable<DocumentSection> AllSections
		{
			get { return this._Sequences.Concat<DocumentSection>(this._Sessions); }
		}

		#endregion

		#region Constructors

		public Document([CanBeNull] AniHost host = null)
		{
			this._Host = host ?? AniHost.Current;
			this._TransportData = new TransportData();
			this._TransportData.PropertyChanged += this.TransportData_PropertyChanged;
			this._Clips = new ObservableCollection<Clip>();
			this._Clips.CollectionChanged += this.Clips_CollectionChanged;
			this._Sequences = new ObservableCollection<Sequence>();
			this._Sequences.CollectionChanged += this.Sequences_CollectionChanged;
			this._Sessions = new ObservableCollection<Session>();
			this._Sessions.CollectionChanged += this.Sessions_CollectionChanged;
			this._Outputs = new ObservableCollection<Output>();
			this._Outputs.CollectionChanged += this.Outputs_CollectionChanged;
		}

		public Document(Guid id, [CanBeNull] AniHost host = null)
			: this(host)
		{
			this._Id = id;
		}

		public Document([NotNull] XElement element, [CanBeNull] AniHost host = null)
			: this(host)
		{
			this.ReadXElement(element, host);
			this.RebuildTransport();
		}

		public Document([NotNull] XDocument document, [CanBeNull]AniHost host = null)
			: this(host)
		{
			Require.ArgNotNull(document, "document");
			this.ReadXElement(document.Root, host);
			this.RebuildTransport();
		}

		#endregion

		#region Methods

		private void RebuildTransport()
		{
			var d = this._Transport as IDisposable;
			if(d != null)
				d.Dispose();
			this._Transport = this.Host.CreateTransport(this._TransportData.TransportType, this._TransportData.Parameters);
		}

		private void TransportData_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged(e.PropertyName);
			switch(e.PropertyName)
			{
			case "BeatsPerMinute":
				if(this._Transport != null)
					this._Transport.BeatsPerMinute = this._TransportData.BeatsPerMinute;
				break;
			case "TransportType":
				this.RebuildTransport();
				break;
			}
			this.OnPropertyChanged("Transport");
		}

		private void Outputs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Outputs");
		}

		private void Clips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Clips");
		}

		private void Sequences_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Sequences");
		}

		private void Sessions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Sessions");
		}

		public Output GetOutput(Guid id)
		{
			return this.Outputs.FindById(id);
		}

		public Clip GetClip(Guid id)
		{
			return this.Clips.FindById(id);
		}

		public Sequence GetSequence(Guid id)
		{
			return this.Sequences.FindById(id);
		}

		internal TargetObject GetTargetObject(Guid id)
		{
			foreach(var output in this._Outputs)
			{
				var target = output.GetTargetObject(id);
				if(target != null)
					return target;
			}
			return null;
		}

		internal void PostClipOutput(Clip clip, Transport.Transport transport)
		{
			Require.ArgNotNull(transport, "transport");
			if(clip == null || !clip.IsPlaying || clip.OutputId == null)
				return;
			var output = this.GetOutput(clip.OutputId.Value);
			if(output == null)
				return;
			var message = clip.BuildOutputMessage(transport);
			output.PostMessage(message);
		}

		public void PostActiveClipOutputs(Transport.Transport transport)
		{
			Require.ArgNotNull(transport, "transport");
			foreach(var clip in this.ActiveClips.ToArray())
				this.PostClipOutput(clip, transport);
		}

		private void ReadXElement(XElement element, AniHost host)
		{
			Require.ArgNotNull(element, "element");
			if(host == null)
				host = AniHost.Current;

			this.Id = (Guid)element.Attribute(Schema.anidoc_id);
			this.Name = (string)element.Attribute(Schema.anidoc_name);

			var transportElement = element.Element(Schema.transport);
			if(transportElement != null)
			{
				this._TransportData.ReadXElement(transportElement);
			}
			else
			{
				this.BeatsPerMinute = (float?)element.Attribute(Schema.anidoc_bpm) ?? DefaultBeatsPerMinute;
			}

			var outputsElement = element.Element(Schema.anidoc_outputs);
			if(outputsElement != null)
				this.Outputs.AddRange(outputsElement.Elements().Select(host.ReadOutputXElement));
			var clipsElement = element.Element(Schema.anidoc_clips);
			if(clipsElement != null)
				this.Clips.AddRange(clipsElement.Elements().Select(host.ReadClipXElement));
			var sequencesElement = element.Element(Schema.anidoc_sequences);
			if(sequencesElement != null)
				this.Sequences.AddRange(sequencesElement.Elements().Select(e => new Sequence(e, this, host)));
			var sessionsElement = element.Element(Schema.anidoc_sessions);
			if(sessionsElement != null)
				this.Sessions.AddRange(sessionsElement.Elements().Select(e => new Session(e, this, host)));

			this.UIRows = (int?)element.Attribute(Schema.anidoc_ui_rows);
			this.UIColumns = (int?)element.Attribute(Schema.anidoc_ui_cols);
		}

		#endregion

		#region IXElementWritable Members

		public XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.anidoc,
				new XAttribute(Schema.anidoc_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.anidoc_name, this.Name),
				this._TransportData.WriteXElement(),
				ModelUtil.WriteListXElement(this._Outputs, Schema.anidoc_outputs),
				ModelUtil.WriteListXElement(this._Clips, Schema.anidoc_clips),
				ModelUtil.WriteListXElement(this._Sequences, Schema.anidoc_sequences),
				ModelUtil.WriteListXElement(this._Sessions, Schema.anidoc_sessions),
				ModelUtil.WriteOptionalValueAttribute(Schema.anidoc_ui_rows, this.UIRows),
				ModelUtil.WriteOptionalValueAttribute(Schema.anidoc_ui_cols, this.UIColumns));
		}

		public XDocument WriteXDocument()
		{
			return new XDocument(this.WriteXElement());
		}

		#endregion

		#region INotifyPropertyChanged Members

		private void OnPropertyChanged(string name)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.PropertyChanged = null;
			if(this._Transport != null)
				this._Transport.Dispose();
			this._Transport = null;
			this._Outputs.CollectionChanged -= this.Outputs_CollectionChanged;
			foreach(var output in this._Outputs)
				output.Dispose();
			this._Outputs.Clear();
			GC.SuppressFinalize(this);
		}

		#endregion

		#region IItemContainer<IDocumentItem> Members

		public IDocumentItem FindById(Guid id)
		{
			if(this.Id == id)
				return this;
			var clip = this.GetClip(id);
			if(clip != null)
				return clip;
			var output = this.GetOutput(id);
			if(output != null)
				return output;
			var sequence = this.GetSequence(id);
			if(sequence != null)
				return sequence;
			return null;
		}

		#endregion

	}

	#endregion

}
