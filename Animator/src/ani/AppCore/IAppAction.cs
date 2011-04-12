using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.AppCore
{

	#region IAppAction

	internal interface IAppAction
	{

		bool SupportsUndo { get; }

		string Name { get; }

		object Perform(object target, object newState);

		void Undo(object target, object oldState);

	}

	#endregion

}
