using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

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
			Debug.Assert(input is IValueIn || input is IValueConfig);
			if(input is IValueIn)
				((IValueIn)input).GetValue(index, out _Value);
			else if(input is IValueConfig)
				((IValueConfig)input).GetValue(index, out _Value);
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			Debug.Assert(output is IValueOut);
			((IValueOut)output).SetValue(index, _Value);
		}

		public override string ToString()
		{
			return _Value.ToString();
		}

		#endregion

		#region IStructPart Members

		public void ResetValue()
		{
			_Value = default(double);
		}

		#endregion

	}

	#endregion

}
