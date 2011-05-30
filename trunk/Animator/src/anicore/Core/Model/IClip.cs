using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.IO;

namespace Animator.Core.Model
{

	#region IClip

	public interface IClip
	{

		object GetValue(Transport.Transport transport);

		OutputMessage BuildOutputMessage(Transport.Transport transport);
	}

	#endregion

}
