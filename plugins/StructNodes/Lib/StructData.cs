using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Lib
{

	#region StructData

	public sealed class StructData
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly StructTypeDefinition _TypeDefinition;

		private readonly IStructPart[] _Parts;

		#endregion

		#region Properties

		public StructTypeDefinition TypeDefinition
		{
			get { return _TypeDefinition; }
		}

		public IList<IStructPart> Parts
		{
			get { return _Parts; }
		}

		#endregion

		#region Constructors

		public StructData(StructTypeDefinition typeDefinition, IStructPart[] parts)
		{
			if(typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");
			if(parts == null)
				throw new ArgumentNullException("parts");
			_TypeDefinition = typeDefinition;
			_Parts = parts;
		}

		#endregion

		#region Methods

		internal void ReadInputs(IList<IPluginIn> inputs, int index)
		{
			foreach(var part in _Parts)
				part.ResetValue();
			if(inputs != null)
			{
				var count = Math.Min(_Parts.Length, inputs.Count);
				for(var i = 0; i < count; i++)
					_Parts[i].ReadInputValue(inputs[i], index % inputs[i].SliceCount);
			}
		}

		internal void WriteOutputs(IList<IPluginOut> outputs, int index)
		{
			if(outputs != null)
			{
				var count = Math.Min(_Parts.Length, outputs.Count);
				for(var i = 0; i < count; i++)
					_Parts[i].WriteOutputValue(outputs[i], index);
			}
		}

		public override string ToString()
		{
			return String.Join(",", _Parts.Select(p => p == null ? "(null)" : p.ToString()).ToArray());
		}

		#endregion

	}

	#endregion

}
