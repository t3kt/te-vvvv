using System;
using System.Collections;
using System.Collections.Generic;
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
		private List<double> _Values;

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
			_Values = new List<double>();
		}

		#endregion

		#region Methods

		public void ResizeValues(int requiredCount)
		{
			Debug.Assert(requiredCount >= 0);
			if(_Values == null)
				_Values = new List<double>(requiredCount);
			if(requiredCount == 0)
			{
				_Values.Clear();
			}
			else if(requiredCount < _Values.Count)//shrink
			{
				_Values.RemoveRange(requiredCount - 1, _Values.Count - requiredCount);
			}
			else if(requiredCount > _Values.Count)
			{
				//while(_Values.Count < requiredCount)
				//    _Values.Add(0);
				_Values.AddRange(Enumerable.Repeat(0.0, requiredCount - _Values.Count));
			}
		}

		public void ReadInputValue(IPluginIO input, int index)
		{
			if(_Values == null)
				_Values = new List<double>();
			Debug.Assert(input is IValueIn || input is IValueConfig);
			if(input is IValueIn)
				((IValueIn)input).GetValue(index, out _Value);
			else if(input is IValueConfig)
				((IValueConfig)input).GetValue(index, out _Value);
		}

		public void ReadInputValues(IPluginIO input, int sourceOffset, int count)
		{
			if(_Values == null)
				_Values = new List<double>();
			Debug.Assert(input is IValueIn || input is IValueConfig);
			ResizeValues(count);
			if(input is IValueIn)
				ReadInputValues((IValueIn)input, sourceOffset, count);
			else if(input is IValueConfig)
				ReadInputValues((IValueConfig)input, sourceOffset, count);
		}

		private void ReadInputValues(IValueIn input, int sourceOffset, int count)
		{
			Debug.Assert(sourceOffset >= 0 && sourceOffset < input.SliceCount);
			for(var i = 0; i < count; i++)
			{
				double value;
				input.GetValue(sourceOffset + i, out value);
				_Values[i] = value;
			}
		}

		private void ReadInputValues(IValueConfig input, int sourceOffset, int count)
		{
			Debug.Assert(sourceOffset >= 0 && sourceOffset < input.SliceCount);
			for(var i = 0; i < count; i++)
			{
				double value;
				input.GetValue(sourceOffset + i, out value);
				_Values[i] = value;
			}
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			Debug.Assert(output is IValueOut);
			((IValueOut)output).SetValue(index, _Value);
		}

		public void WriteOutputValues(IPluginIO output, int destOffset)
		{
			if(_Values == null)
				_Values = new List<double>();
			Debug.Assert(output is IValueOut);
			Debug.Assert(destOffset >= 0);
			var valOut = (IValueOut)output;
			valOut.SliceCount = _Values.Count;
			for(var i = 0; i < _Values.Count; i++)
				valOut.SetValue(destOffset + i, _Values[i]);
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
