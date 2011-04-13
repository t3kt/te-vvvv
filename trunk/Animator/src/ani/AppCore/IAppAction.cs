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

		string Name { get; }

		void Perform(object target, object newState, out object oldState, out bool canUndo);

		void Undo(object target, object oldState);

	}

	#endregion

}
