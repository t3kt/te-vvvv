using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Runtime
{

	#region IObjectEditor

	public interface IObjectEditor
	{
		object Target { get; set; }
		bool AutoCommit { get; }
		bool Dirty { get; }
		void Commit();
		void Reset();
	}

	#endregion

}
