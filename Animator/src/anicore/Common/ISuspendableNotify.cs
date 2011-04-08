using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Common
{

	#region ISuspendableNotify

	internal interface ISuspendableNotify
	{

		void SuspendNotify();

		void ResumeNotify();

	}

	#endregion

}
