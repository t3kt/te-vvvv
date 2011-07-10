using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Markup;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model.Clips
{

	#region ClipBase

	[ContentProperty("Properties")]
	public abstract class ClipBase : DocumentItem
	{

		#region Static / Constant

		private static readonly ItemTypeInfo _ItemType = new ItemTypeInfo(typeof(ClipBase));

		#endregion

		#region Fields

		private readonly ObservableCollection<ClipPropertyData> _Properties;

		#endregion

		#region Properties

		public sealed override ItemTypeInfo ItemType
		{
			get { return _ItemType; }
		}

		public ObservableCollection<ClipPropertyData> Properties
		{
			get { return this._Properties; }
		}

		#endregion

		#region Constructors

		protected ClipBase(Guid id)
			: base(id)
		{
			this._Properties = new ObservableCollection<ClipPropertyData>();
			this._Properties.CollectionChanged += this.Properties_CollectionChanged;
		}

		protected ClipBase([NotNull] XElement element, [CanBeNull]AniHost host)
			: base(element)
		{
			this._Properties = new ObservableCollection<ClipPropertyData>();
			this._Properties.CollectionChanged += this.Properties_CollectionChanged;
			var propsElement = element.Element(Schema.clipbase_props);
			if(propsElement != null)
			{
				if(host == null)
					host = AniHost.Current;
				this._Properties.AddRange(propsElement.Elements().Select(host.ReadClipPropertyDataXElement));
			}
		}

		#endregion

		#region Methods

		internal abstract bool IsActive([NotNull] Transport.Transport transport);

		protected abstract float GetPosition([NotNull] Transport.Transport transport);

		internal void PushTargetChanges([NotNull] TargetObject target, [NotNull] Transport.Transport transport)
		{
			Require.DBG_ArgNotNull(target, "target");
			Require.DBG_ArgNotNull(transport, "transport");
			if(!this.IsActive(transport))
				return;
			var pos = this.GetPosition(transport);
			foreach(var prop in this._Properties)
				target.SetValue(prop.Name, prop.GetValue(pos));
		}

		internal void PushTargetValues([NotNull]TargetObject target, float position)
		{
			Require.DBG_ArgNotNull(target, "target");
			Require.DBG_ArgGreaterThanOrEqualTo(position, "position", 0f);
			Require.DBG_ArgLessThanOrEqualTo(position, "position", 1f);
			foreach(var prop in this._Properties)
				target.SetValue(prop.Name, prop.GetValue(position));
		}

		protected XElement WritePropertiesXElement()
		{
			if(this._Properties.Count == 0)
				return null;
			return new XElement(Schema.clipbase_props, ModelUtil.WriteXElements(this._Properties));
		}

		private void Properties_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Properties");
		}

		#endregion

	}

	#endregion

}
