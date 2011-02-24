using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

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
			Debug.Assert(input is IStringIn || input is IStringConfig);
			if(input is IStringIn)
				((IStringIn)input).GetString(index, out _Value);
			else if(input is IStringConfig)
				((IStringConfig)input).GetString(index, out _Value);
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			Debug.Assert(output is IStringOut);
			((IStringOut)output).SetString(index, _Value);
		}

		public override string ToString()
		{
			return _Value;
		}

		#endregion

		#region IStructPart Members

		public void ResetValue()
		{
			_Value = default(string);
		}

		#endregion

	}

	#endregion

}
