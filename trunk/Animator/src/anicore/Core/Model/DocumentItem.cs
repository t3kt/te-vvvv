using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Model
{

	#region DocumentItem

	public abstract class DocumentItem : IDocumentItem, IInternalDocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public virtual Guid Id { get; protected set; }

		public virtual string Name { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected virtual void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.Id = (Guid)element.Attribute(Schema.id);
			this.Name = (string)element.Attribute(Schema.name);
		}

		#endregion

		#region IDocumentItem Members

		public virtual IDocumentItem Parent { get; internal set; }

		public IDocument Document
		{
			get
			{
				var doc = ModelUtil.GetDocument(this);
				Debug.Assert(doc != null);
				return doc;
			}
		}

		#endregion

		#region IInternalDocumentItem Members

		void IInternalDocumentItem.SetParent(IDocumentItem parent)
		{
			this.Parent = parent;
		}

		#endregion

	}

	#endregion

}
