using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model.Clips
{

	#region ClipBase

	[ContentProperty("Properties")]
	public abstract class ClipBase : DocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly DocumentNodeCollection<ClipPropertyData> _Properties;

		#endregion

		#region Properties

		public ObservableCollection<ClipPropertyData> Properties
		{
			get { return this._Properties; }
		}

		#endregion

		#region Constructors

		protected ClipBase(Guid id)
			: base(id)
		{
			this._Properties = new DocumentNodeCollection<ClipPropertyData>(this);
			this.ObserveChildCollection("Properties", this._Properties);
		}

		protected ClipBase([NotNull] XElement element, [CanBeNull]AniHost host)
			: base(element)
		{
			this._Properties = new DocumentNodeCollection<ClipPropertyData>(this);
			this.ObserveChildCollection("Properties", this._Properties);
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

		internal abstract void PushTargetValues([NotNull] TargetObject target, [NotNull] ITransportController transport);

		protected XElement WritePropertiesXElement()
		{
			if(this._Properties.Count == 0)
				return null;
			return new XElement(Schema.clipbase_props, ModelUtil.WriteXElements(this._Properties));
		}

		#endregion

	}

	#endregion

}
