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

	public sealed class StructTypeDefinition : IEquatable<StructTypeDefinition>
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
				case 'T':
				case 't':
					types.Add(StructPartType.Transform);
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
				case 'T':
				case 't':
					filtered.Append('T');
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
			case StructPartType.Transform:
				return "T";
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
			case StructPartType.Transform:
				return new TransformStructPart();
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		internal static IPluginIn CreatePartInputPin(IPluginHost host, StructPartType partType, string name, int dimension, TSliceMode sliceMode, TPinVisibility visibility)
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
			case StructPartType.Transform:
				ITransformIn transformPin;
				host.CreateTransformInput(name, sliceMode, visibility, out transformPin);
				return transformPin;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		internal static IPluginOut CreatePartOutputPin(IPluginHost host, StructPartType partType, string name, int dimension, TSliceMode sliceMode, TPinVisibility visibility)
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
			case StructPartType.Transform:
				ITransformOut transformPin;
				host.CreateTransformOutput(name, sliceMode, visibility, out transformPin);
				return transformPin;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		internal static string GetPartInputName(int index, StructPartType partType)
		{
			return String.Format("Input{0} ({1})", index, GetPartTypeAbbreviation(partType));
		}

		internal static string GetPartOutputName(int index, StructPartType partType)
		{
			return String.Format("Output{0} ({1})", index, GetPartTypeAbbreviation(partType));
		}

		private static IStructPart ParsePart(StructPartType partType, string str)
		{
			switch(partType)
			{
			case StructPartType.Value:
				return ValueStructPart.Parse(str);
			case StructPartType.Color:
				return ColorStructPart.Parse(str);
			case StructPartType.String:
				return new StringStructPart(str);
			case StructPartType.Transform:
				return TransformStructPart.Parse(str);
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

		internal StructTypeDefinition(string partTypesKey)
		{
			_Id = Guid.NewGuid();
			_PartTypesKey = partTypesKey;
			_PartTypes = ParsePartTypesKey(partTypesKey);
		}

		#endregion

		#region Methods

		public StructData CreateInstance()
		{
			return new StructData(this, _PartTypes.Select(CreatePart).ToArray());
		}

		public StructData ParseInstance(string[] strParts)
		{
			if(strParts == null || strParts.Length == 0)
				return null;
			if(strParts.Length != _PartTypes.Length)
				throw new FormatException(String.Format("Incorrect number of parts for parsing struct of type '{0}': {1}", _PartTypesKey, strParts.Length));
			var parts = new IStructPart[_PartTypes.Length];
			for(var i = 0; i < _PartTypes.Length; i++)
				parts[i] = ParsePart(_PartTypes[i], strParts[i]);
			return new StructData(this, parts);
		}

		internal IList<IPluginIn> CreatePartInputPins(IPluginHost host)
		{
			var inputs = new List<IPluginIn>();
			for(var i = 0; i < _PartTypes.Length; i++)
				inputs.Add(CreatePartInputPin(host, _PartTypes[i], GetPartInputName(i, _PartTypes[i]), 1, TSliceMode.Dynamic, TPinVisibility.True));
			return inputs;
		}

		internal IList<IPluginOut> CreatePartOutputPins(IPluginHost host)
		{
			var outputs = new List<IPluginOut>();
			for(var i = 0; i < _PartTypes.Length; i++)
				outputs.Add(CreatePartOutputPin(host, _PartTypes[i], GetPartOutputName(i, _PartTypes[i]), 1, TSliceMode.Dynamic, TPinVisibility.True));
			return outputs;
		}

		public override string ToString()
		{
			return this.FriendlyTypeName;
		}

		public override bool Equals(object obj)
		{
			if(obj is Guid)
				return (Guid)obj == _Id;
			return obj is StructTypeDefinition &&
				((StructTypeDefinition)obj)._Id == _Id;
		}

		public override int GetHashCode()
		{
			return _Id.GetHashCode();
		}

		#endregion

		#region IEquatable<StructTypeDefinition> Members

		public bool Equals(StructTypeDefinition other)
		{
			return other != null && other._Id.Equals(this._Id);
		}

		#endregion

	}

	#endregion

}
