using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace XamlNodes.Core
{

	#region IXamlNodeHost

	public interface IXamlNodeHost : IPluginEvaluate
	{

		IPluginHost PluginHost { get; }

		IXamlNode Node { get; }

		void Refresh();

	}

	#endregion

}
