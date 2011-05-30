using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region Track

	public abstract class Track : DocumentItem, IClipRefContainer
	{

		#region Static/Constant

		#endregion

		#region Fields

		private Guid? _TargetId;

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

		internal abstract IEnumerable<ClipReference> ClipsInternal { get; }

		#endregion

		#region Constructors

		protected Track(Guid id)
			: base(id) { }

		protected Track([NotNull] XElement element)
			: base(element)
		{
			this.TargetId = (Guid?)element.Attribute(Schema.track_target);
		}

		#endregion

		#region Methods

		protected new IEnumerable<XAttribute> WriteCommonXAttributes()
		{
			return base.WriteCommonXAttributes()
				.Concat(new[] { ModelUtil.WriteOptionalValueAttribute(Schema.track_target, this.TargetId) });
		}

		#endregion

		#region IClipRefContainer Members

		IEnumerable<ClipReference> IClipRefContainer.Clips
		{
			get { return this.ClipsInternal; }
		}

		public virtual IEnumerable<ClipReference> GetActiveClips(Transport.Transport transport)
		{
			return this.ClipsInternal.Where(c => c.IsActiveInternal(transport));
		}

		#endregion

	}

	#endregion

	#region Track<TClipRef>

	public abstract class Track<TClipRef> : Track
		where TClipRef : ClipReference
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ObservableCollection<TClipRef> _Clips;

		#endregion

		#region Properties

		public ObservableCollection<TClipRef> Clips
		{
			get { return this._Clips; }
		}

		internal sealed override IEnumerable<ClipReference> ClipsInternal
		{
			get { return this.Clips; }
		}

		#endregion

		#region Constructors

		protected Track(Guid id)
			: base(id)
		{
			this._Clips = new ObservableCollection<TClipRef>();
			this._Clips.CollectionChanged += this.Clips_CollectionChanged;
		}

		protected Track([NotNull] XElement element)
			: base(element)
		{
			this._Clips = new ObservableCollection<TClipRef>();
			this._Clips.CollectionChanged += this.Clips_CollectionChanged;
		}

		#endregion

		#region Methods

		private void Clips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Clips");
		}

		#endregion

	}

	#endregion

}
