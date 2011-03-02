using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

namespace XamlNodes.Core
{

	#region IXamlNodeHost

	public interface IXamlNodeHost : IPluginBase
	{

		IPluginHost PluginHost { get; }

		IXamlNode Node { get; }

	}

	#endregion

}
