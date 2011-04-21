using System;
using System.Collections;
using System.Linq;

namespace Animator.Core.Model
{

	#region IDocumentItem

	public interface IDocumentItem : IXElementWritable, IDisposable
	{

		Guid Id { get; }

		string Name { get; }

	}

	#endregion

}
