using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region Track

	public sealed class Track : DocumentItem, IEquatable<Track>
	{

		#region Static / Constant

		[CanBeNull]
		private static Clip ReadClipXElementOrNull([NotNull] XElement element)
		{
			Require.ArgNotNull(element, "element");
			if(element.Name == Schema.track_clips_null)
				return null;
			return Clip.ReadClipXElement(element);
		}

		[NotNull]
		private static XElement WriteClipXElement([CanBeNull]Clip clip)
		{
			if(clip == null)
				return new XElement(Schema.track_clips_null);
			return clip.WriteXElement();
		}

		#endregion

		#region Fields

		private Guid? _OutputId;
		private ObservableCollection<Clip> _Clips;
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

		public Track()
			: this(Guid.NewGuid()) { }

		public Track(Guid id)
		{
			this.Id = id;
			this._Clips = new ObservableCollection<Clip>();
		}

		public Track(XElement element)
		{
			this._Clips = new ObservableCollection<Clip>();
			ReadXElement(element);
		}

		#endregion

		#region Methods

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

		private void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			try
			{
				SuspendNotify();
				this.Id = (Guid)element.Attribute(Schema.track_id);
				this.Name = (string)element.Attribute(Schema.track_name);
				this.OutputId = (Guid?)element.Attribute(Schema.track_output);
				this.TargetKey = (string)element.Attribute(Schema.track_target);
				var clipsElement = element.Element(Schema.track_clips);
				this.Clips = clipsElement == null ? null : new ObservableCollection<Clip>(clipsElement.Elements().Select(ReadClipXElementOrNull));
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
				this.Clips.Count == 0 ? null : new XElement(Schema.track_clips, (object) this.Clips.Select(WriteClipXElement)));
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing)
			{
				if(this._Clips != null)
				{
					foreach(var clip in this._Clips)
						clip.Dispose();
				}
				this.DetachClips(this._Clips);
				this._Clips = null;
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
				   other.Clips.SequenceEqual(this.Clips);
		}

		#endregion

	}

	#endregion

}
