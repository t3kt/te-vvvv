using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Lib
{

	#region StructPartDefinition

	public sealed class StructPartDefinition
	{

		#region Static / Constant

		internal const char MultiSliceFlag = '*';

		private static StructPartType? ParsePartTypeOrNull(char c)
		{
			switch(c)
			{
			case 'V':
			case 'v':
				return StructPartType.Value;
			case 'C':
			case 'c':
				return StructPartType.Color;
			case 'S':
			case 's':
				return StructPartType.String;
			default:
				return null;
			}
		}

		internal static StructPartDefinition[] ParseDefinitions(string partTypesKey)
		{
			if(String.IsNullOrEmpty(partTypesKey))
				return new StructPartDefinition[0];
			var parts = new List<StructPartDefinition>();
			for(var i = 0; i < partTypesKey.Length; i++)
			{
				var partType = ParsePartTypeOrNull(partTypesKey[i]);
				if(partType != null)
				{
					var sliceMode = TSliceMode.Single;
					if(i < partTypesKey.Length - 1 && partTypesKey[i + 1] == MultiSliceFlag)
					{
						sliceMode = TSliceMode.Dynamic;
						i++;
					}
					parts.Add(new StructPartDefinition(parts.Count, partType.Value, sliceMode));
				}
			}
			return parts.ToArray();
		}

		#endregion

		#region Fields

		private readonly int _Index;
		private readonly StructPartType _PartType;
		private readonly TSliceMode _SliceMode;

		#endregion

		#region Properties

		public int Index
		{
			get { return _Index; }
		}

		public StructPartType PartType
		{
			get { return _PartType; }
		}

		public TSliceMode SliceMode
		{
			get { return _SliceMode; }
		}

		#endregion

		#region Constructors

		public StructPartDefinition(int index, StructPartType partType, TSliceMode sliceMode)
		{
			Debug.Assert(index >= 0);
			_Index = index;
			_PartType = partType;
			_SliceMode = sliceMode;
		}

		#endregion

		#region Methods

		internal IPluginIn CreateInputPin(IPluginHost host)
		{
			return StructTypeDefinition.CreatePartInputPin(host, _PartType, StructTypeDefinition.GetPartInputName(_Index, _PartType), 1, _SliceMode, TPinVisibility.True);
		}

		internal IPluginOut CreateOutputPin(IPluginHost host)
		{
			return StructTypeDefinition.CreatePartOutputPin(host, _PartType, StructTypeDefinition.GetPartOutputName(_Index, _PartType), 1, _SliceMode, TPinVisibility.True);
		}

		internal IStructPart CreatePart()
		{
			switch(_PartType)
			{
			case StructPartType.Value:
				break;
			case StructPartType.Color:
				break;
			case StructPartType.String:
				break;
			default:
				throw new NotSupportedException();
			}
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			var str = StructTypeDefinition.GetPartTypeAbbreviation(_PartType);
			return _SliceMode == TSliceMode.Single ? str : str + MultiSliceFlag;
		}

		#endregion

	}

	#endregion

}
