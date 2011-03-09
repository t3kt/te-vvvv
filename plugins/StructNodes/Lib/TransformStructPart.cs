using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.Utils.VMath;

namespace VVVV.Lib
{

	#region TransformStructPart

	internal struct TransformStructPart : IStructPart<Matrix4x4>
	{

		#region Static / Constant

		public static TransformStructPart Parse(string str)
		{
			//if(String.IsNullOrEmpty(str))
			//    throw new ArgumentNullException("str");
			//var parts = str.Split(',');
			//if(parts.Length != 16)
			//    throw new FormatException(String.Format("Cannot parse string as Transform value: '{0}'", str));
			//var value = new Matrix4x4();
			throw new NotSupportedException();
		}

		#endregion

		#region Fields

		private Matrix4x4 _Value;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public TransformStructPart(Matrix4x4 value)
		{
			_Value = value;
		}

		#endregion

		#region Methods

		#endregion

		#region IStructPart Members

		public StructPartType PartType
		{
			get { return StructPartType.Transform; }
		}

		public void ResetValue()
		{
			_Value = default(Matrix4x4);
		}

		public void ReadInputValue(IPluginIO input, int index)
		{
			Debug.Assert(input is ITransformIn);
			((ITransformIn)input).GetMatrix(index, out _Value);
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			Debug.Assert(output is ITransformOut);
			((ITransformOut)output).SetMatrix(index, _Value);
		}

		#endregion

	}

	#endregion

}
