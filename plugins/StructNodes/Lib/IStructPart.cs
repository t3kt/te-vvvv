using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region IStructPart

	public interface IStructPart
	{

		StructPartType PartType { get; }

		void ResetValue();

		void ReadInputValue(IPluginIO input, int index);

		void WriteOutputValue(IPluginIO output, int index);

		//void ReadInputValues(IPluginIO input, int sourceOffset, int count);

		//void WriteOutputValues(IPluginIO output, int destOffset);

	}

	#endregion

	#region IStructPart<T>

	internal interface IStructPart<T> : IStructPart
	{

	}

	#endregion

}
