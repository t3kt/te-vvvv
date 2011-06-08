using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
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

		protected readonly Document _Document;
		private Guid? _TargetId;
		private TargetObject _Target;

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
					this._Target = null;
					this.OnPropertyChanged("TargetId");
				}
			}
		}

		internal TargetObject Target
		{
			get
			{
				if(this._Target == null && this._TargetId != null)
					this._Target = this._Document.GetTargetObject(this._TargetId.Value);
				return this._Target;
			}
			set
			{
				this._Target = value;
				this._TargetId = value == null ? (Guid?)null : value.Id;
			}
		}

		internal abstract IEnumerable<ClipReference> ClipsInternal { get; }

		#endregion

		#region Constructors

		protected Track(Guid id, [NotNull]Document document)
			: base(id)
		{
			Require.ArgNotNull(document, "document");
			this._Document = document;
		}

		protected Track([NotNull] XElement element, [NotNull]Document document)
			: base(element)
		{
			Require.ArgNotNull(document, "document");
			this._Document = document;
			this.TargetId = (Guid?)element.Attribute(Schema.track_target);
		}

		#endregion

		#region Methods

		protected new IEnumerable<XAttribute> WriteCommonXAttributes()
		{
			return base.WriteCommonXAttributes()
				.Concat(new[] { ModelUtil.WriteOptionalValueAttribute(Schema.track_target, this.TargetId) });
		}

		internal virtual void PushTargetChanges([NotNull] Transport.Transport transport)
		{
			if(this.Target != null)
			{
				foreach(var clip in this.ClipsInternal)
					clip.PushTargetChanges(this.Target, transport);
			}
		}

		#endregion

		#region IClipRefContainer Members

		IEnumerable<ClipReference> IClipRefContainer.Clips
		{
			get { return this.ClipsInternal; }
		}

		public virtual IEnumerable<ClipReference> GetActiveClips(Transport.Transport transport)
		{
			return this.ClipsInternal.Where(c => c.IsActive(transport));
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

		protected Track(Guid id, [NotNull]Document document)
			: base(id, document)
		{
			this._Clips = new ObservableCollection<TClipRef>();
			this._Clips.CollectionChanged += this.Clips_CollectionChanged;
		}

		protected Track([NotNull] XElement element, [NotNull]Document document)
			: base(element, document)
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
