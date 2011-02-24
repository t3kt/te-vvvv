using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Lib
{

	#region StructTypeDefinition

	public sealed class StructTypeDefinition
	{

		#region Static / Constant

		private static StructPartType[] ParsePartTypesKey(string partTypesKey)
		{
			if(String.IsNullOrEmpty(partTypesKey))
				return new StructPartType[0];
			var types = new List<StructPartType>();
			foreach(var c in partTypesKey)
			{
				switch(c)
				{
				case 'V':
				case 'v':
					types.Add(StructPartType.Value);
					break;
				case 'C':
				case 'c':
					types.Add(StructPartType.Color);
					break;
				case 'S':
				case 's':
					types.Add(StructPartType.String);
					break;
				}
			}
			return types.ToArray();
		}

		internal static string FilterPartTypesKey(string origKey)
		{
			if(String.IsNullOrEmpty(origKey))
				return null;
			var filtered = new StringBuilder();
			foreach(var c in origKey)
			{
				switch(c)
				{
				case 'V':
				case 'v':
					filtered.Append('V');
					break;
				case 'C':
				case 'c':
					filtered.Append('C');
					break;
				case 'S':
				case 's':
					filtered.Append('S');
					break;
				}
			}
			return filtered.ToString();
		}

		internal static string GetPartTypeAbbreviation(StructPartType partType)
		{
			switch(partType)
			{
			case StructPartType.Value:
				return "V";
			case StructPartType.Color:
				return "C";
			case StructPartType.String:
				return "S";
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private static IStructPart CreatePart(StructPartType partType)
		{
			switch(partType)
			{
			case StructPartType.Value:
				return new ValueStructPart();
			case StructPartType.Color:
				return new ColorStructPart();
			case StructPartType.String:
				return new StringStructPart();
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static IPluginIn CreatePartInputPin(IPluginHost host, StructPartType partType, string name, int dimension, TSliceMode sliceMode, TPinVisibility visibility)
		{
			switch(partType)
			{
			case StructPartType.Value:
				IValueIn valuePin;
				host.CreateValueInput(name, dimension, null, sliceMode, visibility, out valuePin);
				return valuePin;
			case StructPartType.Color:
				IColorIn colorPin;
				host.CreateColorInput(name, sliceMode, visibility, out colorPin);
				return colorPin;
			case StructPartType.String:
				IStringIn stringPin;
				host.CreateStringInput(name, sliceMode, visibility, out stringPin);
				return stringPin;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static IPluginOut CreatePartOutputPin(IPluginHost host, StructPartType partType, string name, int dimension, TSliceMode sliceMode, TPinVisibility visibility)
		{
			switch(partType)
			{
			case StructPartType.Value:
				IValueOut valuePin;
				host.CreateValueOutput(name, dimension, null, sliceMode, visibility, out valuePin);
				return valuePin;
			case StructPartType.Color:
				IColorOut colorPin;
				host.CreateColorOutput(name, sliceMode, visibility, out colorPin);
				return colorPin;
			case StructPartType.String:
				IStringOut stringPin;
				host.CreateStringOutput(name, sliceMode, visibility, out stringPin);
				return stringPin;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		#endregion

		#region Fields

		private readonly Guid _Id;
		private readonly string _PartTypesKey;
		private readonly StructPartType[] _PartTypes;

		#endregion

		#region Properties

		public Guid Id
		{
			get { return _Id; }
		}

		public string PartTypesKey
		{
			get { return _PartTypesKey; }
		}

		public IList<StructPartType> PartTypes
		{
			get { return _PartTypes; }
		}

		public string FriendlyTypeName
		{
			get { return "Struct_" + _PartTypesKey; }
		}

		#endregion

		#region Constructors

		public StructTypeDefinition(string partTypesKey)
		{
			_Id = Guid.NewGuid();
			_PartTypesKey = partTypesKey;
			_PartTypes = ParsePartTypesKey(partTypesKey);
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			return this.FriendlyTypeName;
		}

		private IStructPart[] CreateParts()
		{
			return _PartTypes.Select(CreatePart).ToArray();
		}

		public StructData CreateInstance()
		{
			return new StructData(this, CreateParts());
		}

		public INodeIn CreateStructNodeInputPin(IPluginHost host, string name, TSliceMode sliceMode, TPinVisibility visibility)
		{
			INodeIn pin;
			host.CreateNodeInput(name, sliceMode, visibility, out pin);
			SetStructNodePinSubType(pin);
			return pin;
		}

		public void SetStructNodePinSubType(INodeIn pin)
		{
			pin.SetSubType(new[] { _Id }, this.FriendlyTypeName);
		}

		public INodeOut CreateStructNodeOutputPin(IPluginHost host, string name, TSliceMode sliceMode, TPinVisibility visibility)
		{
			INodeOut pin;
			host.CreateNodeOutput(name, sliceMode, visibility, out pin);
			SetStructNodePinSubType(pin);
			return pin;
		}

		public void SetStructNodePinSubType(INodeOut pin)
		{
			pin.SetSubType(new[] { _Id }, this.FriendlyTypeName);
		}

		public IList<IPluginIn> CreatePartInputPins(IPluginHost host)
		{
			var inputs = new List<IPluginIn>();
			for(var i = 0; i < _PartTypes.Length; i++)
				inputs.Add(CreatePartInputPin(host, _PartTypes[i],
					String.Format("Input{0} ({1})", i, GetPartTypeAbbreviation(_PartTypes[i])), 1, TSliceMode.Dynamic, TPinVisibility.True));
			return inputs;
		}

		public IList<IPluginOut> CreatePartOutputPins(IPluginHost host)
		{
			var outputs = new List<IPluginOut>();
			for(var i = 0; i < _PartTypes.Length; i++)
				outputs.Add(CreatePartOutputPin(host, _PartTypes[i],
					String.Format("Output{0} ({1})", i, GetPartTypeAbbreviation(_PartTypes[i])), 1, TSliceMode.Dynamic, TPinVisibility.True));
			return outputs;
		}

		#endregion

	}

	#endregion

}
