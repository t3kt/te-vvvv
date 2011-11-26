using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Lib
{

	#region StructPart<T>

	public sealed class StructPart<T> : IStructPart
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly StructPartType _PartType;
		private readonly List<T> _Values = new List<T>();

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public StructPart(StructPartType partType)
		{
			_PartType = partType;
		}

		#endregion

		#region Methods

		#endregion

		#region IStructPart Members

		public StructPartType PartType
		{
			get { return _PartType; }
		}

		public void ResizeValues(int requiredCount)
		{
			Debug.Assert(requiredCount >= 0);
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
				_Values.AddRange(Enumerable.Repeat(default(T), requiredCount - _Values.Count));
			}
		}

		public void ResetValue()
		{
			if(_Values != null)
			{
				for(var i = 0; i < _Values.Count; i++)
					_Values[i] = default(T);
			}
		}

		public void ReadInputValue(IPluginIO input, int index)
		{
			throw new NotImplementedException();
		}

		public void WriteOutputValue(IPluginIO output, int index)
		{
			throw new NotImplementedException();
		}

		public void ReadInputValues(IPluginIO pin, int sourceOffset, int count)
		{
			var input = PinWrapper.WrapInputPin<T>(pin);
			ResizeValues(count);
			Debug.Assert(sourceOffset >= 0 && sourceOffset < input.SliceCount);
			for(var i = 0; i < count; i++)
				_Values[i] = input.GetValue(sourceOffset + i);
		}

		public void WriteOutputValues(IPluginIO pin, int destOffset)
		{
			var output = PinWrapper.WrapOutputPin<T>(pin);
			Debug.Assert(destOffset >= 0);
			output.SliceCount = _Values.Count;
			for(var i = 0; i < _Values.Count; i++)
				output.SetValue(destOffset + i, _Values[i]);
		}

		#endregion
	}

	#endregion

}
