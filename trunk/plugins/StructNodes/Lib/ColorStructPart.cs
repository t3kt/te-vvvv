using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.Utils.VColor;
using VVVV.PluginInterfaces.V2;

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

		public RGBAColor Value
		{
			get { return _Value; }
			set { _Value = value; }
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
			Debug.Assert(input is IColorIn);
			((IColorIn)input).GetColor(index, out _Value);
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			Debug.Assert(output is IColorOut);
			((IColorOut)output).SetColor(index, _Value);
		}

		#endregion

		#region IStructPart Members

		object IStructPart.Value
		{
			get { return _Value; }
			set
			{
				Debug.Assert(value is RGBAColor);
				_Value = (RGBAColor)value;
			}
		}

		public void ResetValue()
		{
			_Value = default(RGBAColor);
		}

		#endregion

		#region IStructPart Members

		void IStructPart.ReadSpreadInputValue(object spread, int index)
		{
			Debug.Assert(spread is ISpread<RGBAColor>);
			ReadSpreadInputValue((ISpread<RGBAColor>)spread, index);
		}

		void IStructPart.WriteSpreadOutputValue(object spread, int index)
		{
			Debug.Assert(spread is ISpread<RGBAColor>);
			WriteSpreadOutputValue((ISpread<RGBAColor>)spread, index);
		}

		#endregion

		#region IStructPart<RGBAColor> Members

		public void ReadSpreadInputValue(ISpread<RGBAColor> spread, int index)
		{
			if(spread == null)
				ResetValue();
			else
				_Value = spread[index];
		}

		public void WriteSpreadOutputValue(ISpread<RGBAColor> spread, int index)
		{
			if(spread != null)
				spread[index] = _Value;
		}

		#endregion

	}

	#endregion

}
