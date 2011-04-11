using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region IDocumentItem

	public interface IDocumentItem : IGuidId, INamed, IXElementWritable, IDisposable
	{

		IDocumentItem Parent { get; }

		[NotNull]
		Document Document { get; }

		[NotNull]
		IEnumerable<IDocumentItem> Children { get; }

		[CanBeNull]
		IDocumentItem GetItem(Guid id);

	}

	#endregion

}
