using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region IDocumentItem

	public interface IDocumentItem : IGuidId, INamed, IXElementWritable
	{

		IDocumentItem Parent { get; }

		[NotNull]
		IDocument Document { get; }

		[NotNull]
		IEnumerable<IDocumentItem> Children { get; }

		[CanBeNull]
		IDocumentItem GetItem(Guid id);

	}

	#endregion

	#region IInternalDocumentItem

	internal interface IInternalDocumentItem
	{
		void SetParent(IDocumentItem parent);
	}

	#endregion

}
