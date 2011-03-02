using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Xaml
{

	#region IXamlNodeContent

	internal interface IXamlNodeContent
	{

		void SetHostNode(IXamlHostNode hostNode);

	}

	#endregion

}
