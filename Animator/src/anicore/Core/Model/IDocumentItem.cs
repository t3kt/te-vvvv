using System;
using System.Collections;
using System.Linq;

namespace Animator.Core.Model
{

	#region IDocumentItem

	public interface IDocumentItem : IGuidId, INamed, IXElementWritable, IDisposable
	{

	}

	#endregion

}
