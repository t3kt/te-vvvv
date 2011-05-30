using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region TrackBase<TClipRef>

	public abstract class TrackBase<TClipRef> : DocumentItem
		where TClipRef : ClipReference
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Guid? _TargetId;
		private ObservableCollection<TClipRef> _Clips;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Output)]
		public Guid? TargetId
		{
			get { return this._TargetId; }
			set
			{
				if(value != this._TargetId)
				{
					this._TargetId = value;
					this.OnPropertyChanged("TargetId");
				}
			}
		}

		public ObservableCollection<TClipRef> Clips
		{
			get
			{
				if(this._Clips == null)
				{
					this._Clips = new ObservableCollection<TClipRef>();
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
					this.AttachClips(this._Clips);
				}
			}
		}

		#endregion

		#region Constructors

		protected TrackBase(Guid id)
			: base(id) { }

		protected TrackBase([NotNull] XElement element)
			: base(element)
		{
			this.TargetId = (Guid?)element.Attribute(Schema.track_target);
		}

		#endregion

		#region Methods

		private void AttachClips(ObservableCollection<TClipRef> clips)
		{
			if(clips != null)
				clips.CollectionChanged += this.Clips_CollectionChanged;
		}

		private void DetachClips(ObservableCollection<TClipRef> clips)
		{
			if(clips != null)
				clips.CollectionChanged -= this.Clips_CollectionChanged;
		}

		private void Clips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Clips");
		}

		protected new IEnumerable<XAttribute> WriteCommonXAttributes()
		{
			return base.WriteCommonXAttributes()
				.Concat(new[] { ModelUtil.WriteOptionalValueAttribute(Schema.track_target, this.TargetId) });
		}

		#endregion

	}

	#endregion

}
