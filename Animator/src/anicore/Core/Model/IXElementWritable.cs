using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region IXElementWritable

	public interface IXElementWritable
	{

		[NotNull]
		XElement WriteXElement([CanBeNull] XName name);

	}

	#endregion

}
