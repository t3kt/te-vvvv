using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.Utils.VColor;

namespace VVVV.Lib
{

	#region ColorStructPart

	internal struct ColorStructPart : IStructPart<RGBAColor>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private RGBAColor _Value;

		#endregion

		#region Properties

		public StructPartType PartType
		{
			get { return StructPartType.Color; }
		}

		#endregion

		#region Constructors

		public ColorStructPart(RGBAColor value)
		{
			_Value = value;
		}

		#endregion

		#region Methods

		public void ReadInputValue(IPluginIO input, int index)
		{
			Debug.Assert(input is IColorIn || input is IColorConfig);
			if(input is IColorIn)
				((IColorIn)input).GetColor(index, out _Value);
			else if(input is IColorConfig)
				((IColorConfig)input).GetColor(index, out _Value);
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			Debug.Assert(output is IColorOut);
			((IColorOut)output).SetColor(index, _Value);
		}

		public override string ToString()
		{
			return _Value.ToString();
		}

		#endregion

		#region IStructPart Members

		public void ResetValue()
		{
			_Value = default(RGBAColor);
		}

		#endregion

	}

	#endregion

}
