using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region IDocumentItem

	public interface IDocumentItem : IGuidId, INamed
	{

		IDocumentItem Parent { get; }

		[NotNull]
		IDocument Document { get; }

	}

	#endregion

}
