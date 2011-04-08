using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Model
{

	#region IDocumentItemContainer

	public interface IDocumentItemContainer
	{

		IDocumentItem GetItem(Guid id);

	}

	#endregion

}
