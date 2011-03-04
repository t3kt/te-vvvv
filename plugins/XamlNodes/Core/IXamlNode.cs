using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using XamlNodes.Core.Pins;

namespace XamlNodes.Core
{

	#region IXamlNode

	public interface IXamlNode : IPluginEvaluate, IDisposable
	{

		void SetHost(IXamlNodeHost host);

		XamlNodePinCollection<XamlNodeInputPin> InputPins { get; }

		XamlNodePinCollection<XamlNodeConfigPin> ConfigPins { get; }

		XamlNodePinCollection<XamlNodeOutputPin> OutputPins { get; }

	}

	#endregion

}
