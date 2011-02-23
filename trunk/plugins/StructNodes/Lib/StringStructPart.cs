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

	#region StringStructPart

	internal struct StringStructPart : IStructPart<string>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private string _Value;

		#endregion

		#region Properties

		public StructPartType PartType
		{
			get { return StructPartType.String; }
		}

		public string Value
		{
			get { return _Value; }
			set { _Value = value; }
		}

		#endregion

		#region Constructors

		public StringStructPart(string value)
		{
			_Value = value;
		}

		#endregion

		#region Methods

		public void ReadInputValue(IPluginIO input, int index)
		{
			Debug.Assert(input is IStringIn);
			((IStringIn)input).GetString(index, out _Value);
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			Debug.Assert(output is IStringOut);
			((IStringOut)output).SetString(index, _Value);
		}

		#endregion

		#region IStructPart Members

		object IStructPart.Value
		{
			get { return _Value; }
			set
			{
				Debug.Assert(value is string);
				_Value = (string)value;
			}
		}

		public void ResetValue()
		{
			_Value = default(string);
		}

		#endregion

		#region IStructPart Members

		void IStructPart.ReadSpreadInputValue(object spread, int index)
		{
			Debug.Assert(spread is ISpread<string>);
			ReadSpreadInputValue((ISpread<string>)spread, index);
		}

		void IStructPart.WriteSpreadOutputValue(object spread, int index)
		{
			Debug.Assert(spread is ISpread<string>);
			WriteSpreadOutputValue((ISpread<string>)spread, index);
		}

		#endregion

		#region IStructPart<string> Members

		public void ReadSpreadInputValue(ISpread<string> spread, int index)
		{
			if(spread == null)
				ResetValue();
			else
				_Value = spread[index];
		}

		public void WriteSpreadOutputValue(ISpread<string> spread, int index)
		{
			if(spread != null)
				spread[index] = _Value;
		}

		#endregion

	}

	#endregion

}
