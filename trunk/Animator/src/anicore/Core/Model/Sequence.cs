using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region Sequence

	public sealed class Sequence : DocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Time _Duration;
		private ObservableCollection<SequenceClipReference> _Clips;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Transport)]
		public Time Duration
		{
			get { return this._Duration; }
			set
			{
				Require.ArgPositive((float)value, "value");
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		public ObservableCollection<SequenceClipReference> Clips
		{
			get
			{
				if(this._Clips == null)
				{
					this._Clips = new ObservableCollection<SequenceClipReference>();
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

		#endregion

		#region Constructors

		public Sequence(Guid id)
		{
			this.Id = id;
		}

		public Sequence()
			: this(Guid.NewGuid()) { }

		public Sequence(XElement element)
		{
			this.ReadXElement(element);
		}

		#endregion

		#region Methods

		private void AttachClips(ObservableCollection<SequenceClipReference> clips)
		{
			if(clips != null)
				clips.CollectionChanged += this.Clips_CollectionChanged;
		}

		private void DetachClips(ObservableCollection<SequenceClipReference> clips)
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
			this.Id = (Guid)element.Attribute(Schema.sequence_id);
			this.Name = (string)element.Attribute(Schema.sequence_name);
			this.Duration = (float)element.Attribute(Schema.sequence_dur);
			var clipsElement = element.Element(Schema.sequence_clips);
			this.Clips = clipsElement == null ? null : new ObservableCollection<SequenceClipReference>(clipsElement.Elements().Select(e => new SequenceClipReference(e)));
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.sequence,
				new XAttribute(Schema.sequence_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.sequence_name, this.Name),
				new XAttribute(Schema.sequence_dur, (float)this.Duration),
				this.Clips.Count == 0 ? null : new XElement(Schema.sequence_clips, this.Clips.Select(c => c.WriteXElement())));
		}

		#endregion

	}

	#endregion

}
