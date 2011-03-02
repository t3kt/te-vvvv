using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Xaml
{

	#region IXamlHostNode

	internal interface IXamlHostNode
	{

		IPluginHost PluginHost { get; }

		IValueOut AddValueOut(string name, int dimension, string[] dimensionNames, TSliceMode sliceMode, TPinVisibility visibility);

		IStringOut AddStringOut(string name, TSliceMode sliceMode, TPinVisibility visibility);

		IColorOut AddColorOut(string name, TSliceMode sliceMode, TPinVisibility visibility);

	}

	#endregion

}
