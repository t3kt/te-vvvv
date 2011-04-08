using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Model
{

	#region DocumentItemCollection<T>

	internal class DocumentItemCollection<T> : KeyedCollection<Guid, T>
		where T : IDocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override Guid GetKeyForItem(T item)
		{
			Require.ArgNotNull(item, "item");
			return item.Id;
		}

		#endregion

	}

	#endregion

}
