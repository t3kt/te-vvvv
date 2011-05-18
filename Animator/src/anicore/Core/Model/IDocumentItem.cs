using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace Animator.Core.Model
{

	#region IDocumentItem

	public interface IDocumentItem : IXElementWritable, IDisposable
	{

		[Browsable(false)]
		Guid Id { get; }

		[Category(TEShared.Names.Category_Common)]
		[NotifyParentProperty(true)]
		string Name { get; }

	}

	#endregion

}
