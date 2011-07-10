using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Core.Model.Clips;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region Track

	public abstract class Track : DocumentItem
	{

		#region Static/Constant

		private static readonly ItemTypeInfo _ItemType = new ItemTypeInfo(typeof(Track));

		#endregion

		#region Fields

		protected readonly Document _Document;
		private Guid? _TargetId;
		private TargetObject _Target;

		#endregion

		#region Properties

		public sealed override ItemTypeInfo ItemType
		{
			get { return _ItemType; }
		}

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
				if(this._Target == null && this._TargetId != null && this._Document != null)
					this._Target = this._Document.GetTargetObject(this._TargetId.Value);
				return this._Target;
			}
			set
			{
				this._Target = value;
				this._TargetId = value == null ? (Guid?)null : value.Id;
			}
		}

		internal abstract IEnumerable<ClipBase> ClipsInternal { get; }

		#endregion

		#region Constructors

		protected Track(Guid id, Document document)
			: base(id)
		{
			this._Document = document;
		}

		protected Track([NotNull] XElement element, Document document)
			: base(element)
		{
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

	}

	#endregion

}
