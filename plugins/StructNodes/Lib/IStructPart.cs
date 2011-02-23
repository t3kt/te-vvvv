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

		object Value { get; set; }

		void ResetValue();

		void ReadInputValue(IPluginIO input, int index);

		void WriteOutputValue(IPluginIO output, int index);

		void ReadSpreadInputValue(object spread, int index);

		void WriteSpreadOutputValue(object spread, int index);

	}

	#endregion

	#region IStructPart<T>

	internal interface IStructPart<T> : IStructPart
	{

		new T Value { get; set; }

		void ReadSpreadInputValue(ISpread<T> spread, int index);

		void WriteSpreadOutputValue(ISpread<T> spread, int index);

	}

	#endregion

}
