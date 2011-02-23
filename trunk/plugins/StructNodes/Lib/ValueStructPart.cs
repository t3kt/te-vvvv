using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region ValueStructPart

	internal struct ValueStructPart : IStructPart<double>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private double _Value;

		#endregion

		#region Properties

		public StructPartType PartType
		{
			get { return StructPartType.Value; }
		}

		public double Value
		{
			get { return _Value; }
			set { _Value = value; }
		}

		#endregion

		#region Constructors

		public ValueStructPart(double value)
		{
			_Value = value;
		}

		#endregion

		#region Methods

		public void ReadInputValue(IPluginIO input, int index)
		{
			Debug.Assert(input is IValueIn || input is IValueOut);
			if(input is IValueFastIn)
				((IValueFastIn)input).GetValue(index, out _Value);
			else if(input is IValueIn)
				((IValueIn)input).GetValue(index, out _Value);
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			Debug.Assert(output is IValueOut);
			((IValueOut)output).SetValue(index, _Value);
		}

		#endregion

		#region IStructPart Members

		object IStructPart.Value
		{
			get { return _Value; }
			set
			{
				Debug.Assert(value is double);
				_Value = (double)value;
			}
		}

		public void ResetValue()
		{
			_Value = default(double);
		}

		#endregion

		#region IStructPart Members

		void IStructPart.ReadSpreadInputValue(object spread, int index)
		{
			Debug.Assert(spread is ISpread<double>);
			ReadSpreadInputValue((ISpread<double>)spread, index);
		}

		void IStructPart.WriteSpreadOutputValue(object spread, int index)
		{
			Debug.Assert(spread is ISpread<double>);
			WriteSpreadOutputValue((ISpread<double>)spread, index);
		}

		#endregion

		#region IStructPart<double> Members

		public void ReadSpreadInputValue(ISpread<double> spread, int index)
		{
			if(spread == null)
				ResetValue();
			else
				_Value = spread[index];
		}

		public void WriteSpreadOutputValue(ISpread<double> spread, int index)
		{
			if(spread != null)
				spread[index] = _Value;
		}

		#endregion
	}

	#endregion

}
