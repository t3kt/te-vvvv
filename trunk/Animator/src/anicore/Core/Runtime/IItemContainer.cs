using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Runtime
{

	#region IItemContainer<T>

	internal interface IItemContainer<T>
		where T : class, IGuidId
	{

		T FindById(Guid id);

	}

	#endregion

}
