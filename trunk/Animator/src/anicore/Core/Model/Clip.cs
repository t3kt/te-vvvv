using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region Clip

	public abstract class Clip : DocumentItem, IClip
	{

		#region Static / Constant

		internal static IClip ReadClipXElement(IDocument document, IDocumentItem parent, XElement element)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override void ReadXElement(XElement element)
		{
			base.ReadXElement(element);
			this.Duration = (float)element.Attribute(Schema.clip_dur);
			this.TriggerAlignment = (int?)element.Attribute(Schema.clip_align) ?? Model.Document.NoAlignment;
		}

		#endregion

		#region IClip Members

		public virtual Time Duration { get; set; }

		public virtual int TriggerAlignment { get; set; }

		#endregion

	}

	#endregion

}
