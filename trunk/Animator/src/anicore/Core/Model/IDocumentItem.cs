using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region IDocumentItem

	public interface IDocumentItem : IGuidId, IXElementWritable, IDisposable
	{

		[NotNull]
		[Browsable(false)]
		ItemTypeInfo ItemType { get; }

		[Category(TEShared.Names.Category_Common)]
		[NotifyParentProperty(true)]
		string Name { get; }

	}

	#endregion

}
