using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region StructJoinNode

	// incomplete
	internal class StructJoinNode : IPluginEvaluate, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private IPluginHost _Host;
		private StructTypeDefinition _Type;
		private IStringConfig _PartTypesConfig;
		private bool _NeedsTypePropsOutput;
		//private bool _ForceEvaluate;

		private readonly List<IPluginIn> _PartInputs = new List<IPluginIn>();

		private INodeOut _StructOutput;
		private IStringOut _ActualPartTypesOutput;
		private IStringOut _TypeGuidOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		private void Configurate(IPluginConfig config)
		{
			if(config == _PartTypesConfig)
			{
				string partTypesKey;
				_PartTypesConfig.GetString(0, out partTypesKey);
				partTypesKey = StructTypeDefinition.FilterPartTypesKey(partTypesKey);
				if(_Type == null || partTypesKey != _Type.PartTypesKey)
				{
					StructTypeRegistry.ReleaseTypeDefinition(_Type);
					_Type = StructTypeRegistry.RequestTypeDefinition(partTypesKey);
					foreach(var input in _PartInputs)
						_Host.DeletePin(input);
					_PartInputs.Clear();
					if(_Type != null)
					{
						for(var i = 0; i < _Type.PartTypes.Count; i++)
						{
							var partType = _Type.PartTypes[i];
							var input = StructTypeDefinition.CreatePartInputPin(_Host, partType,
								String.Format("Input{0}{1}", i, StructTypeDefinition.GetPartTypeAbbreviation(partType)), 1, TSliceMode.Dynamic, TPinVisibility.True);
							_PartInputs.Add(input);
						}
						if(_StructOutput == null)
							_Host.CreateNodeOutput("Struct Output", TSliceMode.Dynamic, TPinVisibility.True, out _StructOutput);
						_Type.SetStructNodePinSubType(_StructOutput);
					}
					else
					{
						if(_StructOutput != null)
						{
							_Host.DeletePin(_StructOutput);
							_StructOutput = null;
						}
					}
				}
				_NeedsTypePropsOutput = true;
				//_ForceEvaluate = true;
			}
		}

		public void Evaluate(int spreadMax)
		{
			if(_NeedsTypePropsOutput)
			{
				_ActualPartTypesOutput.SetString(0, _Type == null ? null : _Type.PartTypesKey);
				_TypeGuidOutput.SetString(0, _Type == null ? null : _Type.Id.ToString());
				_NeedsTypePropsOutput = false;
			}
			if(_Type != null && _PartInputs.Count != 0 && _PartInputs.Any(p => p.PinIsChanged))
			{
				var count = _StructOutput.SliceCount = Math.Max(spreadMax, _PartInputs.Max(p => p.SliceCount));
				for(var i = 0; i < count; i++)
				{
					var data = _Type.CreateInstance();
					data.ReadInputs(_PartInputs, i);
				}
				throw new NotImplementedException();
			}
		}

		private void SetPluginHost(IPluginHost host)
		{
			_Host = host;
			host.CreateStringConfig("PartTypes", TSliceMode.Single, TPinVisibility.OnlyInspector, out _PartTypesConfig);
			_PartTypesConfig.SetSubType(String.Empty, false);

			host.CreateStringOutput("ActualPartTypes", TSliceMode.Single, TPinVisibility.Hidden, out _ActualPartTypesOutput);
			_ActualPartTypesOutput.SetSubType(String.Empty, false);
			host.CreateStringOutput("TypeGuid", TSliceMode.Single, TPinVisibility.Hidden, out _TypeGuidOutput);
			_TypeGuidOutput.SetSubType(String.Empty, false);

			Configurate(_PartTypesConfig);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
